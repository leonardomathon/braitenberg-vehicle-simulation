using System;
using System.Collections.Generic;
using Configurations;
using Objects.Light;
using UnityEngine;

namespace Objects.Vehicle {
	public class Sensor : Selectable {
		private const int LAYER_MASK = ~(1 << 11);
		
		public float fieldOfView;
		public float sensitivity;
		public bool invertRotation;
		
		public GameObject body;
		public SensorMesh sensorMesh;

		private ConfigurationRange configureRotation;
		private ConfigurationFloat configureSensitivity;
		private ConfigurationRange configureFieldOfView;

		public float FieldOfView {
			// TODO: Enforce value limits? 0-180
			get => fieldOfView;
			set {
				fieldOfView = value;
				sensorMesh.SetAngle(value * 2);
			}
		}
		public float Sensitivity {
			get => sensitivity;
			set => sensitivity = value;
		}
		public float Rotation {
			// TODO: Enforce value limits? 0-360
			get {
				if (invertRotation) {
					float result = 360 - body.transform.localEulerAngles.y;
					if (result % 360 == 0) {
						return 0;
					}
					return result;
				}
				return body.transform.localEulerAngles.y;
			}
			set {
				Transform sensorTransform = body.transform;
				Vector3 rotation = sensorTransform.localEulerAngles;
				if (invertRotation) {
					rotation.y = 360 - value;
				} else {
					rotation.y = value;
				}
				sensorTransform.localEulerAngles = rotation;
			}
		}

		private new void Start() {
			sensorMesh.SetAngle(fieldOfView * 2);
			configureRotation = new ConfigurationRange("Rotation", "Direction of this sensor", 0, 359.999f, () => Rotation, value => Rotation = value);
			configureSensitivity = new ConfigurationFloat("Sensitivity", "Sensitivity to light", () => Sensitivity, value => Sensitivity = value);
			configureFieldOfView = new ConfigurationRange("Field of view", "Viewing angle width of this sensor", 0, 180, () => FieldOfView, value => FieldOfView = value);
		}

		public float Measure(IEnumerable<Lightbulb> lights) {
			var sensorTransform = body.transform;
			var sensorOrigin = sensorTransform.position;
			
			float total = 0;

			foreach (var bulb in lights) {
				var vector = bulb.transform.position - sensorOrigin;
				var distance = vector.magnitude;
				var angle = Vector3.Angle(sensorTransform.right, vector); // For some reason with the custom mesh we need .right and not .forward here
				var obstructed = Physics.Raycast(sensorOrigin, vector, distance, LAYER_MASK); 
				// TODO: It's clean, but not optimal, raycast if done even if it's out of the field of view
				
				if (angle <= fieldOfView && !obstructed) {
					total += Intensity(angle, distance, bulb.intensity, bulb.color);
				}
			}
			
			return total;
		}
		private float Intensity(float angle, float distance, float sourceIntensity, float sourceColor) {
			return sensitivity * (sourceIntensity / distance / angle); // TODO: This is a terrible formula
		}
		
		public override List<Configuration> Configuration() {
			return new List<Configuration> {
				configureRotation,
				configureSensitivity,
				configureFieldOfView
			};
		}
	}
}