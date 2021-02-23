using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // The layer mask on which we can spawn objects
    [SerializeField]
    private LayerMask spawnableAreaMask;

    // Spawn Controller that handles all the spawnable objects
    private SpawnController spawnController;

    // Game Manager 
    private GameManager gameManager;

    void Start()
    {
        // Grab the instance of the spawn controller 
        spawnController = SpawnController.Instance;

        // Grab the instance of the game manager
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ShootRay(spawnController.GetSelectedObjectToSpawn());
        }
    }

    // Shoots a ray cast, checks if it hits and call spawner method
    private void ShootRay(GameObject selectedObjectToSpawn)
    {
        RaycastHit hit;

        // Shoot a ray from main camera through screen point
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check if ray hits something with the mask "SpawnableArea"
        if ((Physics.Raycast(ray, out hit, 100, spawnableAreaMask)) && (selectedObjectToSpawn != null))
        {
            SpawnObject(selectedObjectToSpawn, hit.point);

        }
    }

    // Spawns the selected object to a certain position
    private void SpawnObject(GameObject selectedObjectToSpawn, Vector3 pos)
    {
        // Check if selectedObjectToSpawn is a vehicle or a light
        if ((selectedObjectToSpawn.GetComponent("Vehicle") as Vehicle) != null)
        {
            if (gameManager.AllowSpawnVehicle())
            {
                // Add to list
                gameManager.vehicles.Add(selectedObjectToSpawn);

                // Create object
                Instantiate(selectedObjectToSpawn, pos, Quaternion.identity);
            }
        }
        else
        {
            if (gameManager.AllowSpawnLights())
            {
                // Add to list
                gameManager.lights.Add(selectedObjectToSpawn);

                // Create object
                Instantiate(selectedObjectToSpawn, pos, Quaternion.identity);
            }

        }
    }
}
