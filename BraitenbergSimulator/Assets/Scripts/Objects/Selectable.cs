using UnityEngine;

namespace Objects {
	public class Selectable : Highlightable {
		// This class simply serves as a wrapper around Highlightable, to indicate the difference between things that
		// are part of a selection and need to change materials, and things that can actually be clicked on.

		public new bool IsSelected() {
			return base.IsSelected();
		}
		public new void Select() {
			base.Select();
		}
		public new void Deselect() {
			base.Deselect();
		}
	}
}