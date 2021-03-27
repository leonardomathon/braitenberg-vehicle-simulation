using System.Collections.Generic;
using Configurations;

namespace Objects.Light {
	public class Lightbulb : Object {
		public float intensity;
		public float color; // TODO: Possibly replace with fancy emission spectrum if we have time
		
		private ConfigurationFloat configureIntensity;
		
		public float Intensity {
			get => intensity;
			set => intensity = value;
		}

		private new void Start() {
			configureIntensity = new ConfigurationFloat("Intensity", "Strength of this light source", () => Intensity, value => Intensity = value);
		}
		public override List<Configuration> Configuration() {
			return new List<Configuration> {
				configureIntensity
			};
		}
	}
}