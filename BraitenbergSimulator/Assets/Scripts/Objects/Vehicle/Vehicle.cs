using Objects.Vehicle.Motors;
using UnityEngine;

namespace Objects.Vehicle {
	[System.Serializable] public class Vehicle : Object {
		[SerializeField] private VehicleType type;

		private VehicleType _type;

		[SerializeField] private Vector3 position;

		public GameObject body;
		public GameObject sensors;

		public Wheel leftWheel;
		public Wheel rightWheel;

		public Sensor leftSensor;
		public Sensor rightSensor;
	
		private GameManager gameManager;
		private VehicleMovement movement;

		// TODO: Maybe not return the average for get, but some indication that the individual values are unique
		public float WheelsBaseSpeed {
			get => (leftWheel.BaseSpeed + rightWheel.BaseSpeed) / 2;
			set {
				leftWheel.BaseSpeed = value;
				rightWheel.BaseSpeed = value;
			}
		}
		public float WheelsStrength {
			get => (leftWheel.Strength + rightWheel.Strength) / 2;
			set {
				leftWheel.Strength = value;
				rightWheel.Strength = value;
			}
		}
		public float SensorsFieldOfView {
			get => (leftSensor.FieldOfView + rightSensor.FieldOfView) / 2;
			set {
				leftSensor.FieldOfView = value;
				rightSensor.FieldOfView = value;
			}
		}
		public float SensorsSensitivity {
			get => (leftSensor.Sensitivity + rightSensor.Sensitivity) / 2;
			set {
				leftSensor.Sensitivity = value;
				rightSensor.Sensitivity = value;
			}
		}
		public float SensorsPosition {
			get => sensors.transform.localPosition.z;
			set {
				Transform sensorsTransform = sensors.transform;
				Vector3 sensorsPosition = sensorsTransform.localPosition;
				sensorsPosition.z = value;
				sensorsTransform.localPosition = sensorsPosition;
			}
		}
		public float SensorsRotation {
			get => (leftSensor.Rotation + rightSensor.Rotation) / 2;
			set {
				leftSensor.Rotation = value;
				rightSensor.Rotation = value;
			}
		}

		private new void Start() {
			AttachMovementScript();
		}
		protected void Update() {
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
		
		// Attach the right movement script to the vehicle object based on VehicleType
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
		public void SetGameManager(GameManager gameManager) {
			this.gameManager = gameManager;
		}
	}
}