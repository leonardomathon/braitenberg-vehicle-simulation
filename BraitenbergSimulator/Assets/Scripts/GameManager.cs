using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Max vehicles, lights and other objects inside the scene
    public int maxVehicles = 5;
    public int maxLights = 10;
    public int maxVarious = 10;

    // List that holds the Braitenberg vehicles
    public List<GameObject> vehicles = new List<GameObject>();

    // List that holds the lights that affect the vehicles
    public List<Lightbulb> lights = new List<Lightbulb>();

    // List that holds all the other objects spawned in scene
    public List<GameObject> various = new List<GameObject>();

    // Mask for the vehicles
    public LayerMask vehicleMask;

    // Mask for the lights
    public LayerMask lightMask;

    // Mask for the other objects
    public LayerMask variousMask;

    // Camera controller
    private CameraController cameraController;

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

    void Start()
    {
        cameraController = CameraController.Instance;
    }

    public bool AllowSpawnVehicle()
    {
        return vehicles.Count < maxVehicles;
    }

    public bool AllowSpawnLights()
    {
        return lights.Count < maxLights;
    }

    public bool AllowSpawnVarious()
    {
        return various.Count < maxVarious;
    }

    public void ClearScene()
    {
        // Reset camera
        cameraController.ResetTarget();

        // Remove all vehicles from the scene
        foreach (GameObject obj in vehicles)
        {
            Destroy(obj);
        }

        foreach (var light in lights)
        {
            Destroy(light.gameObject);
        }

        foreach (GameObject obj in various)
        {
            Destroy(obj);
        }

        // Remove all objects from list
        vehicles.Clear();
        lights.Clear();
        various.Clear();
    }

    public List<Lightbulb> GetLights() {
        // When we implement cars with lights on them, this function should also return those lights
        return lights;
    }
}
