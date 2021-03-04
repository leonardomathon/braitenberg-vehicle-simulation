using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionController : MonoBehaviour
{
    [SerializeField] private GameObject selectedObject;

    [SerializeField] private bool selectedObjectMovable;

    // The layer mask on which we can select objects
    [SerializeField] private LayerMask selectableAreaMasks;

    // The layer mask on wich we can move object
    [SerializeField] private LayerMask moveableAreaMask;

    private GameManager gameManager;

    private SoundManager soundManager;

    private CameraController cameraController;

    // Singleton pattern for SelectionController
    #region singleton
    private static SelectionController _instance;

    public static SelectionController Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    void Start()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;
        cameraController = CameraController.Instance;
    }

    void Update()
    {
        if (!selectedObjectMovable && Input.GetMouseButtonDown(0))
        {
            SelectObject();
        }

        if (selectedObject && Input.GetKeyDown(KeyCode.M))
        {
            MoveSelectedObject();
        }

        if (selectedObject && Input.GetKeyDown(KeyCode.R))
        {
            RotateSelectedObject();
        }

        if (selectedObjectMovable)
        {
            Move();
        }

        if (selectedObjectMovable && Input.GetMouseButtonDown(0))
        {
            PlaceSelectedObject();
        }

        // Check if selected object still exists
        if (selectedObject == null)
        {
            selectedObject = null;
        }
    }

    public GameObject GetSelectedObject()
    {
        return selectedObject;
    }

    private void ResetSelection()
    {
        cameraController.ResetTarget();
    }

    public void DeleteSelectedObject()
    {
        // Deselect before destroying
        DeselectAllObjects();

        // Cleanup object list, brute force
        foreach (GameObject vehicle in gameManager.vehicles)
        {
            if (vehicle == selectedObject)
            {
                gameManager.vehicles.Remove(selectedObject);
                Destroy(selectedObject);
                return;
            }
        }
        foreach (GameObject various in gameManager.various)
        {
            if (various == selectedObject)
            {
                gameManager.various.Remove(selectedObject);
                Destroy(selectedObject);
                return;
            }
        }
        foreach (GameObject light in gameManager.lights)
        {
            if (light == selectedObject)
            {
                gameManager.lights.Remove(selectedObject);
                Destroy(selectedObject);
                return;
            }
        }
    }

    public void SetSelectedObject(GameObject obj)
    {
        // Select all objects to make sure only one object is selected
        DeselectAllObjects();

        // Play object select sound
        soundManager.PlaySelectObjectSound();

        // Set the selected object
        selectedObject = obj;

        // Select the object
        obj.GetComponent<Object>().Select();

        // Let camera orbit around selected object
        cameraController.SetTarget(obj);
    }

    public void ResetSelectedObject()
    {
        // Play object deselect sound
        soundManager.PlayDeselectObjectSound();

        // Reset the selected object
        selectedObject = null;

        // Deselect all objects
        DeselectAllObjects();
    }

    private void SelectObject()
    {
        RaycastHit hit;

        // First check if mouse is currently over UI, do nothing
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (EventSystem.current.currentSelectedGameObject != null) return;

        // Shoot a ray from main camera through screen point
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check if ray hits something with the mask
        if (Physics.Raycast(ray, out hit, 100, selectableAreaMasks))
        {
            GameObject hitObject = hit.transform.gameObject;

            // Check if the hitObject is of type object
            if (hitObject.GetComponent<Object>() != null)
            {
                // Select object only if it is not currently selected
                if (!hitObject.GetComponent<Object>().IsSelected())
                {
                    SetSelectedObject(hitObject);
                }
                // If it is currently selected, deselect it and reset camera
                else
                {
                    ResetSelectedObject();
                }
            }
        }
    }

    private void DeselectAllObjects()
    {
        foreach (GameObject obj in gameManager.vehicles)
        {
            obj.GetComponent<Vehicle>().Deselect();
        }
        foreach (GameObject obj in gameManager.various)
        {
            obj.GetComponent<Obstacle>().Deselect();
        }
        foreach (GameObject obj in gameManager.lights)
        {
            obj.GetComponent<Lightbulb>().Deselect();
        }

        // Let camera orbit the default target
        cameraController.ResetTarget();
    }

    public void MoveSelectedObject()
    {
        cameraController.EnableOverviewCamera();
        selectedObjectMovable = true;
        selectedObject.GetComponent<Object>().Move();
        cameraController.UnfollowTarget();
    }

    private void Move()
    {
        if (!cameraController.CameraIsMoving())
        {
            // Shoot ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, moveableAreaMask))
            {
                Vector3 destinationPos = new Vector3(
                        Mathf.Ceil(hit.point.x),
                        selectedObject.transform.position.y,
                        Mathf.Ceil(hit.point.z)
                );

                selectedObject.transform.position = destinationPos;
            }
        }
    }

    public void RotateSelectedObject()
    {
        // Get current rotation
        Quaternion currentRotation = selectedObject.transform.rotation;

        // Rotate 45 degrees around the y axis
        selectedObject.transform.rotation = currentRotation * Quaternion.Euler(0, 45.0f, 0);
    }

    private void PlaceSelectedObject()
    {
        // Play audiofile
        soundManager.PlayPlaceObjectSound();

        // Disable camera overview mode
        cameraController.DisableOverviewCamera(selectedObject);

        // Set object to unmovable
        selectedObject.GetComponent<Object>().Place();

        // Set movable boolean to false
        selectedObjectMovable = false;

        // Follow the target again
        cameraController.FollowTarget();
    }
}
