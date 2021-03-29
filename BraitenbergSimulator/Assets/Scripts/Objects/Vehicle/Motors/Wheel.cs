using System.Collections.Generic;
using Configurations;
using UnityEngine;

namespace Objects.Vehicle.Motors {
	public class Wheel : Selectable, Motor {
		public float baseSpeed;
		public float strength = 10;

		public Rigidbody body;

		private ConfigurationFloat configureBaseSpeed;
		private ConfigurationFloat configureStrength;
		private ConfigurationFloat configureMass;
		private ConfigurationFloat configureDrag;
		private ConfigurationFloat configureAngularDrag;
		
		public float BaseSpeed {
			get => baseSpeed;
			set => baseSpeed = value;
		}
		public float Strength {
			get => strength;
			set => strength = value;
		}
		public float Mass {
			get => body.mass;
			set => body.mass = value;
		}
		public float Drag {
			get => body.drag;
			set => body.drag = value;
		}
		public float AngularDrag {
			get => body.angularDrag;
			set => body.angularDrag = value;
		}
		
		private new void Start() {
			configureBaseSpeed = new ConfigurationFloat("Base speed", "Base speed of this motor", () => BaseSpeed, value => BaseSpeed = value);
			configureStrength = new ConfigurationFloat("Motor strength", "Power output of this motor", () => Strength, value => Strength = value);
			configureMass = new ConfigurationFloat("Mass", "Physical mass of this wheel", () => Mass, value => Mass = value);
			configureDrag = new ConfigurationFloat("Drag", "Drag of this wheel", () => Drag, value => Drag = value);
			configureAngularDrag = new ConfigurationFloat("Angular drag", "Angular drag of this wheel", () => AngularDrag, value => AngularDrag = value);
		}
		
		public void SetForce(float force) {
			body.AddRelativeTorque(force * strength + baseSpeed, 0, 0);
		}
		
		public override List<Configuration> Configuration() {
			return new List<Configuration> {
				configureStrength,
				configureBaseSpeed,
				configureMass,
				configureDrag,
				configureAngularDrag
			};
		}
	}
}