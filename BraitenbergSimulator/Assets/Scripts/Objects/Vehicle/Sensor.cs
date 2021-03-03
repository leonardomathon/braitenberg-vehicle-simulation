using System.Collections.Generic;
using Objects.Light;
using UnityEngine;

namespace Objects.Vehicle {
	public class Sensor : MonoBehaviour {
		private const int LAYER_MASK = ~(1 << 11);
		
		public float fieldOfView;
		public float sensitivity;

		private List<Lightbulb> lights = new List<Lightbulb>();

		public float Measure(List<Lightbulb> lights) {
			var sensorOrigin = transform.position + (transform.forward * 0.5f);
			
			float total = 0;

			foreach (var light in lights) {
				var vector = light.transform.position - sensorOrigin;
				var distance = vector.magnitude;
				var angle = Vector3.Angle(transform.forward, vector);
				var obstructed = Physics.Raycast(sensorOrigin, vector, distance, LAYER_MASK); 
				// TODO: Clean, but not optimal, raycast if done even if it's out of the field of view
				
				if (angle <= fieldOfView && !obstructed) {
					total += Intensity(angle, distance, light.intensity, light.color);
				}
			}

			this.lights = lights;

			return total;
		}
		
		private float Intensity(float angle, float distance, float sourceIntensity, float sourceColor) {
			return sensitivity * (sourceIntensity / distance / angle); // TODO: This is a terrible formula
		}

		private void OnDrawGizmos() {
			// Draw a yellow sphere at the transform's position

			var sensorOrigin = transform.position + (transform.forward * 0.5f);
			
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(sensorOrigin, 0.1f);

			foreach (var light in this.lights) {
				Gizmos.DrawLine(sensorOrigin, light.transform.position);
			}
		}
	}
}