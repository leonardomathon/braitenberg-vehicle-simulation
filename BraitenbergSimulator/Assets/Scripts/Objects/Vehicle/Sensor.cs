using System.Collections.Generic;
using Objects.Light;
using UnityEngine;

namespace Objects.Vehicle {
	public class Sensor : Selectable {
		private const int LAYER_MASK = ~(1 << 11);
		
		public float fieldOfView;
		public float sensitivity;

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
			get => transform.localEulerAngles.y;
			set {
				Transform sensorTransform = transform;
				Vector3 rotation = sensorTransform.localEulerAngles;
				rotation.y = value;
				sensorTransform.localEulerAngles = rotation;
			}
		}

		public float Measure(IEnumerable<Lightbulb> lights) {
			var sensorTransform = transform;
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
	}
}