﻿using UnityEngine;
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

    private CameraController cameraController;

    void Start()
    {
        gameManager = GameManager.Instance;
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

        if (selectedObjectMovable)
        {
            Move();
        }

        if (selectedObjectMovable && Input.GetMouseButtonDown(0))
        {
            PlaceSelectedObject();
        }
    }

    public void ResetSelection()
    {
        cameraController.ResetTarget();
    }

    private void SetSelectedObject(GameObject obj)
    {
        // Set the selected object
        selectedObject = obj;

        // Select all objects to make sure only one object is selected
        DeselectAllObjects();

        // Select the object
        obj.GetComponent<Object>().Select();

        // Let camera orbit around selected object
        cameraController.SetTarget(obj);
    }

    private void ResetSelectedObject()
    {
        // Reset the selected object
        selectedObject = null;

        // Deselect all objects
        DeselectAllObjects();

        // Let camera orbit the default target
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
    }

    private void MoveSelectedObject()
    {
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
                Vector3 newPos = new Vector3(
                        Mathf.Ceil(hit.point.x),
                        selectedObject.transform.position.y,
                        Mathf.Ceil(hit.point.z)
                );
                selectedObject.transform.position = newPos;
            }
        }

    }

    private void PlaceSelectedObject()
    {
        // Set object to unmovable
        selectedObject.GetComponent<Object>().Place();

        // Set movable boolean to false
        selectedObjectMovable = false;

        // Follow the target again
        cameraController.FollowTarget();
    }
}
