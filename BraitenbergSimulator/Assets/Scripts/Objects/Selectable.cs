using System;
using System.Collections.Generic;
using Configurations;

namespace Objects {
	public class Selectable : Highlightable {
		// This class simply serves as a wrapper around Highlightable, to indicate the difference between things that
		// are part of a selection and need to change materials, and things that can actually be clicked on.

		public string objectName;
		
		public string Name() {
			return objectName;
		}
		public virtual List<Configuration> Configuration() {
			return new List<Configuration>();
		}
		public virtual List<Tuple<Action, SelectableButton>> Actions() {
			return new List<Tuple<Action, SelectableButton>>();
		}

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