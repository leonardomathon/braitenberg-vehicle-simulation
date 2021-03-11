using System.Linq;
using Objects.Vehicle.Motors;
using UnityEngine;

namespace Objects.Vehicle {
	[System.Serializable] public class Vehicle : Object {
		[SerializeField] private VehicleType type;

		private VehicleType _type;

		[SerializeField] private Vector3 position;

		public GameObject body;

		public Wheel leftWheel;
		public Wheel rightWheel;

		public Sensor leftSensor;
		public Sensor rightSensor;
	
		private GameManager gameManager;
		private VehicleMovement movement;

		private void Start() {
			AttachMovementScript();
		}

		protected override void Update() {
			// base.Update();
			// UpdateMovementScript();
			UpdateBodyRotation();

			var lights = gameManager.GetLights();
			var measurements = new[] {leftSensor.Measure(lights), rightSensor.Measure(lights)};
			var activations = movement.MotorActivation(measurements);

			// Debug.Log(activations.Aggregate("Motors: ", (current, activation) => current + (activation + ", ")));

			leftWheel.SetForce(activations[0]);
			rightWheel.SetForce(activations[1]);
		}

		// Attach the rigt movementscript to the vehicle object based on VehicleType
		private void AttachMovementScript() {
			_type = type;
			SetMovementType(type);
		}
		private void SetMovementType(VehicleType type) {
			switch (type) {
				case VehicleType.Agression:
					movement = new VehicleMovementAgression();
					break;
				case VehicleType.Exploration:
					movement = new VehicleMovementExploration();
					break;
				case VehicleType.Fear:
					movement = new VehicleMovementFear();
					break;
				case VehicleType.Love:
					movement = new VehicleMovementLove();
					break;
				default:
					movement = new VehicleMovementDefault();
					break;
			}
		}

		// Update movementscript if VehicleType has changed
		private void UpdateMovementScript() {
			// TODO: This could and probably should be replaced with some event-like call when the type is actually changed
			if (type != _type) {
				_type = type;
				SetMovementType(type);
			}
		}

		private void UpdateBodyRotation() {
			var transformLocalRotation = body.transform.rotation;
			transformLocalRotation.x = 0;
			transformLocalRotation.z = 0;
			body.transform.rotation = transformLocalRotation;
		}

		public void SetGameManager(GameManager gameManager) {
			this.gameManager = gameManager;
		}
	}
}