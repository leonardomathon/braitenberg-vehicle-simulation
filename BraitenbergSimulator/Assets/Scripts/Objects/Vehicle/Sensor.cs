using UnityEngine;

namespace Objects.Vehicle {
	public class Sensor : MonoBehaviour {
		public float Measure(Lightbulb[] lights) {
			Debug.Log("Measuring "+lights.Length+" lights");
			return 0;
		}
	}
}