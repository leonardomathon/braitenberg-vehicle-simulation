using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    [SerializeField]
    public GameObject selectedObject;

    public GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
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
                GameObject cameraController = GameObject.Find("Camera Controller");
                DefaultCameraMovement camera = cameraController.GetComponent<DefaultCameraMovement>();
                camera.setTarget(hitObject);
            }


        }
    }
}
