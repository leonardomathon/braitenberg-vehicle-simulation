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
		
		public float BaseSpeed {
			get => baseSpeed;
			set => baseSpeed = value;
		}
		public float Strength {
			get => strength;
			set => strength = value;
		}
		
		private new void Start() {
			configureBaseSpeed = new ConfigurationFloat("Base speed", "Base speed of this motor", () => BaseSpeed, value => BaseSpeed = value);
			configureStrength = new ConfigurationFloat("Motor strength", "Power output of this motor", () => Strength, value => Strength = value);
		}
		
		public void SetForce(float force) {
			body.AddRelativeTorque(force * strength + baseSpeed, 0, 0);
		}
		
		public override List<Configuration> Configuration() {
			return new List<Configuration> {
				configureStrength,
				configureBaseSpeed
			};
		}
	}
}