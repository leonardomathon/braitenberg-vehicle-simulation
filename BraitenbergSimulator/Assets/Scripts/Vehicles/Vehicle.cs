using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vehicle
{
    private GameObject gameObject;
    private int vehicleId;
    private VehicleType type;
    private Vector3 position;
    [SerializeField]
    private int leftMotorSpeed;

    [SerializeField]
    private int rightMotorSpeed;

    public Vehicle(GameObject gameObject, Vector3 pos)
    {
        this.gameObject = gameObject;
        this.vehicleId = Random.Range(1, 2147483647);
        this.type = VehicleType.Default;
        this.position = pos;
        this.leftMotorSpeed = 1;
        this.rightMotorSpeed = 1;
        AttachMovementScript();
    }

    public Vehicle(GameObject gameObject, VehicleType type)
    {
        this.gameObject = gameObject;
        this.vehicleId = Random.Range(1, 2147483647);
        this.type = type;
        this.position = new Vector3(0, 5, 0);
        this.leftMotorSpeed = 1;
        this.rightMotorSpeed = 1;
        AttachMovementScript();
    }

    public Vehicle(GameObject gameObject, VehicleType type, Vector3 pos)
    {
        this.gameObject = gameObject;
        this.vehicleId = Random.Range(1, 2147483647);
        this.type = type;
        this.position = pos;
        this.leftMotorSpeed = 1;
        this.rightMotorSpeed = 1;
        AttachMovementScript();
    }

    public int GetVehicleId()
    {
        return this.vehicleId;
    }

    public void SetVehicleId(int vehicleId)
    {
        this.vehicleId = vehicleId;
    }

    public VehicleType GetVehicleType()
    {
        return this.type;
    }

    public void SetVehicleType(VehicleType type)
    {
        this.type = type;
        UpdateMovementScript();
    }

    public Vector3 GetPosition()
    {
        return this.position;
    }

    public void SetPosition(Vector3 position)
    {
        this.position = position;
    }

    public int GetLeftMotorSpeed()
    {
        return this.leftMotorSpeed;
    }

    public void SetLeftMotorSpeed(int leftMotorSpeed)
    {
        this.leftMotorSpeed = leftMotorSpeed;
    }

    public int GetRightMotorSpeed()
    {
        return this.rightMotorSpeed;
    }

    public void SetRightMotorSpeed(int rightMotorSpeed)
    {
        this.rightMotorSpeed = rightMotorSpeed;
    }

    // Attach the rigt movementscript to the vehicle object based on VehicleType
    private void AttachMovementScript()
    {
        this.gameObject.AddComponent<VehicleDefaultMovement>();
    }

    // Update movementscript if VehicleType has changed
    private void UpdateMovementScript()
    {


    }
}
