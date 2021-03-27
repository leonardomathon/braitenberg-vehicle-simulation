using System.Collections.Generic;
using Configurations;
using Objects.Light;
using UnityEngine;

namespace Objects.Vehicle {
	public class Sensor : Selectable {
		private const int LAYER_MASK = ~(1 << 11);
		
		public float fieldOfView;
		public float sensitivity;
		
		public GameObject body;

		private ConfigurationRange configureRotation;
		private ConfigurationFloat configureSensitivity;
		private ConfigurationRange configureFieldOfView;

		public float FieldOfView {
			// TODO: Enforce value limits? 0-180
			get => fieldOfView;
			set => fieldOfView = value;
		}
		public float Sensitivity {
			get => sensitivity;
			set => sensitivity = value;
		}
		public float Rotation {
			// TODO: Enforce value limits? 0-360
			get => body.transform.localEulerAngles.z;
			set {
				Transform sensorTransform = body.transform;
				Vector3 rotation = sensorTransform.localEulerAngles;
				rotation.z = value;
				sensorTransform.localEulerAngles = rotation;
			}
		}
		
		private new void Start() {
			configureRotation = new ConfigurationRange("Rotation", "Direction of this sensor", 0, 360, () => Rotation, value => Rotation = value);
			configureSensitivity = new ConfigurationFloat("Sensitivity", "Sensitivity to light", () => Sensitivity, value => Sensitivity = value);
			configureFieldOfView = new ConfigurationRange("Field of view", "Viewing angle width of this sensor", 0, 180, () => FieldOfView, value => FieldOfView = value);
		}

		public float Measure(IEnumerable<Lightbulb> lights) {
			var sensorTransform = body.transform;
			var sensorOrigin = sensorTransform.position + sensorTransform.forward * 0.5f;
			
			float total = 0;

			foreach (var bulb in lights) {
				var vector = bulb.transform.position - sensorOrigin;
				var distance = vector.magnitude;
				var angle = Vector3.Angle(transform.forward, vector);
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