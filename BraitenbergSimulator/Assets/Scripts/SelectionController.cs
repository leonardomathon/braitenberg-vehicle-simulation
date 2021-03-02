using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionController : MonoBehaviour
{
    [SerializeField]
    private GameObject selectedObject;

    // The layer mask on which we can select objects
    [SerializeField]
    private LayerMask selectableAreaMask;

    private GameManager gameManager;

    private CameraController cameraController;

    void Start()
    {
        gameManager = GameManager.Instance;
        cameraController = CameraController.Instance;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectObject();
        }
    }

    public void ResetSelection()
    {
        cameraController.ResetTarget();
    }

    private void SelectObject()
    {
        RaycastHit hit;

        // First check if mouse is currently over UI, do nothing
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (EventSystem.current.currentSelectedGameObject != null) return;

        // Shoot a ray from main camera through screen point
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check if ray hits something with the mask "Vehicle"
        if (Physics.Raycast(ray, out hit, 100, selectableAreaMask))
        {
            GameObject hitObject = hit.transform.gameObject;

            if ((hitObject.GetComponent("Vehicle") as Vehicle) != null)
            {
                // Select object only if it is not currently selected
                if (!hitObject.GetComponent<Vehicle>().IsSelected())
                {
                    DeselectAllObjects();
                    hitObject.GetComponent<Vehicle>().Select();
                    cameraController.SetTarget(hitObject);
                }
                // If it is currently selected, deselect it and reset camera
                else
                {
                    DeselectAllObjects();
                    cameraController.ResetTarget();
                }
            } 

            if ((hitObject.GetComponent("Lightbulb") as Lightbulb) != null) 
            {
                // Select lightsource only if it is not currently selected
                if (!hitObject.GetComponent<Lightbulb>().IsSelected())
                {
                    DeselectAllObjects();
                    hitObject.GetComponent<Lightbulb>().Select();
                    cameraController.SetTarget(hitObject);
                }
                // If the lightsource is currently selected, deselect it and reset camera
                else 
                {
                    DeselectAllObjects();
                    cameraController.ResetTarget();
                }
            }
        }
    }

    private void DeselectAllObjects()
    {
        foreach (GameObject vehicle in gameManager.vehicles)
        {
            vehicle.GetComponent<Vehicle>().Deselect();
        }
        foreach (GameObject light in gameManager.lights)
        {
            light.GetComponent<Lightbulb>().Deselect();
        }
    }
}
