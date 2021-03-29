using System.Collections.Generic;
using Configurations;

namespace Objects.Light {
	public class Lightbulb : Object {
		public float intensity;
		public float color; // TODO: Possibly replace with fancy emission spectrum if we have time
		public UnityEngine.Light sceneLight;
		
		private ConfigurationFloat configureIntensity;
		
		public float Intensity {
			get => intensity;
			set {
				intensity = value;
				sceneLight.intensity = value * 5;
			}
		}

		private new void Start() {
			base.Start();
			sceneLight.intensity = intensity * 5;
			configureIntensity = new ConfigurationFloat("Intensity", "Strength of this light source", () => Intensity, value => Intensity = value);
		}
		public override List<Configuration> Configuration() {
			return new List<Configuration> {
				configureIntensity
			};
		}
	}
}