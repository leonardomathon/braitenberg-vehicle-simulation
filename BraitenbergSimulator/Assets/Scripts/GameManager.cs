using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Max vehicles and lights inside the scene
    public int maxVehicles =  5;
    public int maxLights = 10;

    // List that holds the Braitenberg vehicles
    public List<GameObject> vehicles = new List<GameObject>();

    // List that holds the lights that affect the vehicles
    public List<GameObject> lights = new List<GameObject>();

    // The layer mask on which we can spawn objects
    public LayerMask spawnableAreaMask;

    // Mask for the vehicles
    public LayerMask vehicleMask;

    // Mask for the lights
    public LayerMask lightMask;

    // The gameobject that is currently selected
    public GameObject selectedObj;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            SpawnObject();
        }
        if (Input.GetMouseButtonDown(0)) {
            SelectObject();
        }
    }
    
    // Shoots a ray cast, checks if it hits and spawns selected object
    private void SpawnObject() 
    {
        RaycastHit hit;

        // Shoot a ray from main camera through screen point
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check if ray hits something with the mask "SpawnableArea"
        if (Physics.Raycast(ray, out hit, 100, spawnableAreaMask))
        {
            // Check if the vehicles list is not yet full
            if (vehicles.Count < maxVehicles) {
                // Add object to vehicle list
                vehicles.Add(selectedObj);
                // Create object
                Instantiate(selectedObj, hit.point, Quaternion.identity);
                
            }
            
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
            // Find vehicle and destroy it
            Destroy(hit.transform.gameObject);
            
        }
    }
}
