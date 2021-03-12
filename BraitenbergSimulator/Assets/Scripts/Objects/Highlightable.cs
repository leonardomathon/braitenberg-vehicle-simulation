using UnityEngine;

namespace Objects {
	public class Highlightable : MonoBehaviour {
		public Material materialDefault;
		public Material materialSelected;
		public Highlightable[] children;

		private bool isSelected;
		private MeshRenderer mesh;

		private void Start() {
			mesh = GetComponent<MeshRenderer>();
			Deselect();
		}
		
		protected bool IsSelected() {
			return isSelected;
		}
		protected void Select() {
			isSelected = true;
			if (mesh != null) {
				mesh.sharedMaterial = materialSelected;
			}
			foreach (var child in children) {
				child.Select();
			}
		}
		protected void Deselect() {
			isSelected = false;
			if (mesh != null) {
				mesh.sharedMaterial = materialDefault;
			}
			foreach (var child in children) {
				child.Deselect();
			}
		}
	}
}