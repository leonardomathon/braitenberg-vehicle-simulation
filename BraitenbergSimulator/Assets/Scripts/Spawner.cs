using Objects.Light;
using Objects.Vehicle;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = Objects.Object;

public class Spawner : MonoBehaviour
{
    // The layer mask on which we can spawn objects
    [SerializeField] private LayerMask spawnableAreaMask;

    // The parent object to which the spawned object will be assigned to
    [SerializeField] private GameObject parentObject;

    // Spawn Controller that handles all the spawnable objects
    private SpawnController spawnController;

    // Game Manager 
    private GameManager gameManager;

    private void Start()
    {
        // Grab the instance of the spawn controller 
        spawnController = SpawnController.Instance;

        // Grab the instance of the game manager
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && spawnController.selectedObjectToSpawn != null)
        {
            ShootRay(spawnController.selectedObjectToSpawn);
        }
    }

    // Shoots a ray cast, checks if it hits and call spawner method
    private void ShootRay(GameObject selectedObjectToSpawn)
    {
        // First check if mouse is currently over UI, do nothing
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (EventSystem.current.currentSelectedGameObject != null) return;

        // Shoot a ray from main camera through screen point
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check if ray hits something with the mask "SpawnableArea"
        if ((Physics.Raycast(ray, out var hit, 100, spawnableAreaMask)) && (selectedObjectToSpawn != null))
        {
            SpawnObject(selectedObjectToSpawn, hit.point);
        }
    }

    // Spawns the selected object to a certain position
    private void SpawnObject(GameObject selectedObjectToSpawn, Vector3 pos)
    {
        pos.y += 1;

        // Check if selectedObjectToSpawn is a vehicle, else its a light or a various object
        if (selectedObjectToSpawn.GetComponent<Vehicle>() != null)
        {
            if (gameManager.AllowSpawnVehicle())
            {
                // Create object
                var instantiatedObject = Instantiate(selectedObjectToSpawn, pos, Quaternion.identity, parentObject.transform);
                var vehicle = instantiatedObject.GetComponent<Vehicle>();
                vehicle.SetGameManager(gameManager);

                // Add to list
                gameManager.vehicles.Add(vehicle);
            }
        }
        else
        {
            if (spawnController.gameObjectToSpawnableObject[selectedObjectToSpawn] == SpawnableObject.Light)
            {
                if (gameManager.AllowSpawnLights())
                {
                    // Avoid clipping of the light by increasing y level
                    pos.y += 0.1f;

                    // Create gameobject and store it temporarily in a variable
                    var instantiatedObject = Instantiate(selectedObjectToSpawn, pos, Quaternion.identity, parentObject.transform);

                    // Add created gameobject to list
                    gameManager.lights.Add(instantiatedObject.GetComponent<Lightbulb>());
                }
            }
            else
            {
                if (gameManager.AllowSpawnVarious())
                {
                    // Avoid clipping of the light by increasing y level
                    pos.y += 0.1f;

                    // Create gameobject and store it temporarily in a variable
                    var instantiatedObject = Instantiate(selectedObjectToSpawn, pos, Quaternion.identity, parentObject.transform);

                    // Add created gameobject to list
                    gameManager.various.Add(instantiatedObject.GetComponent<Object>());
                }
            }
        }
    }
}