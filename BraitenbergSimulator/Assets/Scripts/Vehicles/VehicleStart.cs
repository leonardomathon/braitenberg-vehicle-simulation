using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleStart : MonoBehaviour
{
    public Vehicle vehicle;
    public VehicleType vehicleType;
    void Start()
    {
        vehicle = new Vehicle(gameObject, vehicleType);
    }

    void Update()
    {

    }

}
