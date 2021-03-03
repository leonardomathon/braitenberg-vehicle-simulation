using UnityEngine;

namespace Objects.Vehicle.Motors {
	public class Wheel : Motor {
		public WheelCollider collider;
		public Transform visual;
		
		// public void FixedUpdate() {
		// 	float motor = maxMotorTorque * Input.GetAxis("Vertical");
		// 	float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
		//
		// 	foreach (AxleInfo axleInfo in axleInfos) {
		// 		if (axleInfo.steering) {
		// 			axleInfo.leftWheel.steerAngle = steering;
		// 			axleInfo.rightWheel.steerAngle = steering;
		// 		}
		// 		if (axleInfo.motor) {
		// 			axleInfo.leftWheel.motorTorque = motor;
		// 			axleInfo.rightWheel.motorTorque = motor;
		// 		}
		// 		ApplyLocalPositionToVisuals(axleInfo.leftWheel);
		// 		ApplyLocalPositionToVisuals(axleInfo.rightWheel);
		// 	}
		// }
		
		public void SetForce(float force) {
			throw new System.NotImplementedException();
		}

		public void SetVisualPosition() {
			collider.GetWorldPose(out var position, out var rotation);

			var transform = visual.transform;
			transform.position = position;
			transform.rotation = rotation;
		}
	}
}