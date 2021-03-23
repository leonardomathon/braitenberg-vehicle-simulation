using System;

namespace Configurations {
	public class ConfigurationBoolean : Configuration {
		private readonly string name;
		private readonly string description;
		private readonly Func<bool> get;
		private readonly Action<bool> set;

		public ConfigurationBoolean(string name, string description, Func<bool> get, Action<bool> set) {
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
			set((bool) value);
		}
		
	}
}