using System;

namespace Configurations {
	public class ConfigurationFloat : Configuration {
		private readonly string name;
		private readonly string description;
		private readonly Func<float> get;
		private readonly Action<float> set;

		public ConfigurationFloat(string name, string description, Func<float> get, Action<float> set) {
			this.name = name;
			this.description = description;
			this.get = get;
			this.set = set;
		}
		
		public string Name() {
			return name;
		}
		public string Description() {
			return description;
		}
		public object Get() {
			return get();
		}
		public void Set(object value) {
			set((float) value);
		}
	}
}