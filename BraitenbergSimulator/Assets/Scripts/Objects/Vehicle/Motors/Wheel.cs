using UnityEngine;

namespace Objects.Vehicle.Motors
{
    public class Wheel : Selectable, Motor
    {
        public Rigidbody body;

        public float baseSpeed;
        public float strength = 10;

        public void SetForce(float force)
        {
            body.AddRelativeTorque(force * strength + baseSpeed, 0, 0);
        }
    }
}