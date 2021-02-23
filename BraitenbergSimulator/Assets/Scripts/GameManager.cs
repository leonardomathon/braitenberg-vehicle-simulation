using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Max vehicles and lights inside the scene
    public int maxVehicles = 5;
    public int maxLights = 10;

    // List that holds the Braitenberg vehicles
    public List<GameObject> vehicles = new List<GameObject>();

    // List that holds the lights that affect the vehicles
    public List<GameObject> lights = new List<GameObject>();

    // Mask for the vehicles
    public LayerMask vehicleMask;

    // Mask for the lights
    public LayerMask lightMask;

    // The gameobject that is currently selected
    public GameObject selectedObj;

    // Singleton pattern for GameManager
    #region singleton
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }


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


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectObject();
        }
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
            GameObject cameraController = GameObject.Find("Camera Controller");
            DefaultCameraMovement camera = cameraController.GetComponent<DefaultCameraMovement>();
            camera.setTarget(hitObject);
        }
    }

    public bool AllowSpawnVehicle()
    {
        return vehicles.Count < maxVehicles;
    }

    public bool AllowSpawnLights()
    {
        return lights.Count < maxLights;
    }
}
