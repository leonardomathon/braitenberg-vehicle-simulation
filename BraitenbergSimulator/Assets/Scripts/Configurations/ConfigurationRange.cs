using System;

namespace Configurations {
	public class ConfigurationRange : Configuration {
		private readonly string name;
		private readonly string description;
		private readonly float minimum;
		private readonly float maximum;
		private readonly Func<float> get;
		private readonly Action<float> set;

		public ConfigurationRange(string name, string description, float minimum, float maximum, Func<float> get, Action<float> set) {
			this.name = name;
			this.description = description;
			this.minimum = minimum;
			this.maximum = maximum;
			this.get = get;
			this.set = set;
		}
		
		public string Name() {
			return name;
		}
		public string Description() {
			return description;
		}
		public float Minimum() {
			return minimum;
		}
		public float Maximum() {
			return maximum;
		}

		public object Get() {
			return get();
		}
		public void Set(object value) {
			set(Math.Max(Math.Min((float) value, maximum), minimum));
		}
	}
}