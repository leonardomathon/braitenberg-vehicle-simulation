using UnityEngine;

namespace Objects.Vehicle.Motors {
	public class Wheel : MonoBehaviour, Motor {
		public Rigidbody body;
		
		public void SetForce(float force) {
			body.AddRelativeTorque(force, 0, 0);
		}
	}
}