using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    [SerializeField]
    private GameObject selectedObject;

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

        // Shoot a ray from main camera through screen point
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check if ray hits something with the mask "Vehicle"
        if (Physics.Raycast(ray, out hit, 100))
        {
            GameObject hitObject = hit.transform.gameObject;

            if ((hitObject.GetComponent("Vehicle") as Vehicle) != null)
            {
                cameraController.SetTarget(hitObject);
            }


        }
    }
}
