using UnityEngine;

namespace Objects {
	public class StaticBody : MonoBehaviour {
		// Add to an object with rigidbody/collider to make it not rotate in local space
		
		private void Update() {
			var transformLocal = transform;
			var transformLocalRotation = transformLocal.localRotation;
			transformLocalRotation.x = 0;
			transformLocalRotation.y = 0;
			transformLocalRotation.z = 0;
			transformLocal.localRotation = transformLocalRotation;
		}
	}
}