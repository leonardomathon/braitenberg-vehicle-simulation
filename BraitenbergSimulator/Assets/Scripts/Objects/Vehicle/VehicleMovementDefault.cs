using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDefaultMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody rb;
    // Set time to 0
    private float time = 0f;

    void Start()
    {
        target = gameObject.GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Increase time
        time = time + Time.deltaTime;
        // If two seconds have passed, move cube and reset time
        if (time > 2f)
        {
            // rb.velocity = RandomVector(-2f, 2f);
            time = 0f;
        }
    }

    // Returns random vector in specified range
    private Vector3 RandomVector(float min, float max)
    {
        var x = Random.Range(min, max);
        var y = Random.Range(0f, max * 5);
        var z = Random.Range(min, max);
        return new Vector3(x, y, z);
    }
}
