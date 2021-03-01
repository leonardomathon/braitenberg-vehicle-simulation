using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject[] spawnableObjects = new GameObject[5];

    public Dictionary<SpawnableObject, GameObject> spawnableObjectToGameObject;

    public Dictionary<GameObject, SpawnableObject> gameObjectToSpawnableObject;

    public GameObject selectedObjectToSpawn;

    private GameManager gameManager;

    // Singleton pattern for SpawnController
    #region singleton
    private static SpawnController _instance;

    public static SpawnController Instance { get { return _instance; } }


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
        gameManager = GameManager.Instance;

        spawnableObjectToGameObject = new Dictionary<SpawnableObject, GameObject>(){
            { SpawnableObject.Light, spawnableObjects[0] },
            { SpawnableObject.Aggression, spawnableObjects[1] },
            { SpawnableObject.Exploration, spawnableObjects[2] },
            { SpawnableObject.Fear, spawnableObjects[3] },
            { SpawnableObject.Love, spawnableObjects[4] }
        };

        gameObjectToSpawnableObject = new Dictionary<GameObject, SpawnableObject>(){
            { spawnableObjects[0], SpawnableObject.Light },
            { spawnableObjects[1], SpawnableObject.Aggression },
            { spawnableObjects[2], SpawnableObject.Exploration },
            { spawnableObjects[3], SpawnableObject.Fear },
            { spawnableObjects[4], SpawnableObject.Love },
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectObjectToSpawn(SpawnableObject.Light);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectObjectToSpawn(SpawnableObject.Aggression);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectObjectToSpawn(SpawnableObject.Exploration);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectObjectToSpawn(SpawnableObject.Fear);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectObjectToSpawn(SpawnableObject.Love);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            gameManager.ClearScene();
        }
    }

    public void SelectObjectToSpawn(SpawnableObject obj)
    {
        GameObject toSpawn = spawnableObjectToGameObject[obj];

        if (toSpawn != null)
        {
            selectedObjectToSpawn = toSpawn;
        }
    }

    public void DeselectObjectToSpawn()
    {
        selectedObjectToSpawn = null;
    }

}