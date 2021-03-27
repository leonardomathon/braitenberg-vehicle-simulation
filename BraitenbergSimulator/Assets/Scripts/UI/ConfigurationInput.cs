using Configurations;
using UnityEngine;

namespace UI {
	public abstract class ConfigurationInput : MonoBehaviour {
		public abstract void SetConfiguration(Configuration configuration);
		public abstract bool AcceptsConfiguration(Configuration configuration);
	}
}