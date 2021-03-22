using UnityEngine;

namespace Objects.Vehicle.Motors {
	public class Wheel : Selectable, Motor {
		public float baseSpeed;
		public float strength = 10;

		public Rigidbody body;
		
		public float BaseSpeed {
			get => baseSpeed;
			set => baseSpeed = value;
		}
		public float Strength {
			get => strength;
			set => strength = value;
		}
		
		public void SetForce(float force) {
			body.AddRelativeTorque(force * strength + baseSpeed, 0, 0);
		}
	}
}