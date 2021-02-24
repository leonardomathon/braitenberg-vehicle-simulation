﻿using System.Collections;
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

    public void ClearScene()
    {
        // Reset camera
        cameraController.ResetTarget();

        // Remove all vehicles from the scene
        foreach (GameObject vehicle in vehicles)
        {
            Destroy(vehicle);
        }

        foreach (GameObject light in lights)
        {
            Destroy(light);
        }

        // Remove all objects from list
        vehicles.Clear();
        lights.Clear();

    }
}