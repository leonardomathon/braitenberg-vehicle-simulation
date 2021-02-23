using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spawnableObjects = new GameObject[5];

    [SerializeField]
    private GameObject selectedObjectToSpawn;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectObjectToSpawn(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectObjectToSpawn(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectObjectToSpawn(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectObjectToSpawn(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectObjectToSpawn(5);
        }
    }

    private void SelectObjectToSpawn(int keyCode)
    {
        if (spawnableObjects[keyCode - 1] != null)
        {
            selectedObjectToSpawn = spawnableObjects[keyCode - 1];
        }
    }

    public GameObject GetSelectedObjectToSpawn()
    {
        return this.selectedObjectToSpawn;
    }
}