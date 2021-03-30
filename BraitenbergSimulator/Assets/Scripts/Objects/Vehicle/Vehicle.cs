using System.Collections.Generic;
using Configurations;
using Objects.Vehicle.Motors;
using Objects.Light;
using UnityEngine;

namespace Objects.Vehicle {
	[System.Serializable] public class Vehicle : Object {
		[SerializeField] private VehicleType type;
		private VehicleType _type;

		public GameObject body;
		public GameObject sensors;
		public Rigidbody rigidBody;

		public Wheel leftWheel;
		public Wheel rightWheel;

		public Sensor leftSensor;
		public Sensor rightSensor;

		public float initialSensorSensorRotation;

		private VehicleMovement movement;

		private ConfigurationFloat configureMass;
		
		private ConfigurationRange configureSensorsPosition;
		private ConfigurationRange configureSensorsRotation;
		private ConfigurationFloat configureSensorsSensitivity;
		private ConfigurationRange configureSensorsFieldOfView;

		private ConfigurationFloat configureWheelsBaseSpeed;
		private ConfigurationFloat configureWheelsStrength;
		private ConfigurationFloat configureWheelsMass;
		private ConfigurationFloat configureWheelsDrag;
		private ConfigurationFloat configureWheelsAngularDrag;

		// TODO: Maybe not return the average for get, but some indication that the individual values are unique
		public float Mass {
			get => rigidBody.mass;
			set => rigidBody.mass = value;
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
		public float WheelsMass {
			get => (leftWheel.Mass + rightWheel.Mass) / 2;
			set {
				leftWheel.Mass = value;
				rightWheel.Mass = value;
			}
		}
		public float WheelsDrag {
			get => (leftWheel.Drag + rightWheel.Drag) / 2;
			set {
				leftWheel.Drag = value;
				rightWheel.Drag = value;
			}
		}
		public float WheelsAngularDrag {
			get => (leftWheel.AngularDrag + rightWheel.AngularDrag) / 2;
			set {
				leftWheel.AngularDrag = value;
				rightWheel.AngularDrag = value;
			}
		}

		private new void Start() {
			base.Start();
			AttachMovementScript();

			SensorsRotation = initialSensorSensorRotation;

			configureMass = new ConfigurationFloat("Vehicle mass", "Physical mass of the vehicle", () => Mass, value => Mass = value);
			configureSensorsPosition = new ConfigurationRange("Sensor positions", "Forward/backwards offset of sensors", -1, 1, () => SensorsPosition, value => SensorsPosition = value);
			configureSensorsRotation = new ConfigurationRange("Sensor rotations", "Rotation of both sensors", 0, 359.999f, () => SensorsRotation, value => SensorsRotation = value);
			configureSensorsSensitivity = new ConfigurationFloat("Sensor sensitivities", "Sensitivity to light of both sensors", () => SensorsSensitivity, value => SensorsSensitivity = value);
			configureSensorsFieldOfView = new ConfigurationRange("Sensor fields of view", "Viewing angle width of both sensors", 0, 180, () => SensorsFieldOfView, value => SensorsFieldOfView = value);
			configureWheelsBaseSpeed = new ConfigurationFloat("Motor base speeds", "Base speed of both motors", () => WheelsBaseSpeed, value => WheelsBaseSpeed = value);
			configureWheelsStrength = new ConfigurationFloat("Motor strengths", "Power output of both motors", () => WheelsStrength, value => WheelsStrength = value);
			configureWheelsMass = new ConfigurationFloat("Wheel mass", "Physical mass of both wheels", () => WheelsMass, value => WheelsMass = value);
			configureWheelsDrag = new ConfigurationFloat("Wheel drag", "Drag of both wheels", () => WheelsDrag, value => WheelsDrag = value);
			configureWheelsAngularDrag = new ConfigurationFloat("Wheel angular drag", "Angular drag of both wheels", () => WheelsAngularDrag, value => WheelsAngularDrag = value);
		}
		protected new void Update() {
			base.Update();
			UpdateBodyRotation();

			List<Lightbulb> lights = gameManager.GetLights();
			float[] measurements = {leftSensor.Measure(lights), rightSensor.Measure(lights)};
			float[] activations = movement.MotorActivation(measurements);

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
			var transformRotation = body.transform.rotation;
			transformRotation.x = 0;
			transformRotation.z = 0;
			body.transform.rotation = transformRotation;
			
			var transformLocalRotation = body.transform.localRotation;
			transformLocalRotation.y = 0;
			body.transform.localRotation = transformLocalRotation;
		}

		// Attach the right movement script to the vehicle object based on VehicleType
		private void AttachMovementScript() {
			_type = type;
			SetMovementType(type);
		}
		private void SetMovementType(VehicleType type) {
			switch (type) {
				case VehicleType.Aggression:
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

		public override List<Configuration> Configuration() {
			return new List<Configuration> {
				configureMass,
				configureSensorsPosition,
				configureSensorsRotation,
				configureSensorsSensitivity,
				configureSensorsFieldOfView,
				configureWheelsBaseSpeed,
				configureWheelsStrength,
				configureWheelsMass,
				configureWheelsDrag,
				configureWheelsAngularDrag
			};
		}
	}
}