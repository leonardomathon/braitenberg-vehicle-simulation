using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovementFear : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody rb;

    void Start()
    {
        target = gameObject.GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();
    }
}
