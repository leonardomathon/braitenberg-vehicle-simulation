using System;
using System.Collections.Generic;

namespace Configurations {
	public class ConfigurationSelection<T> : Configuration {
		private readonly string name;
		private readonly string description;
		private readonly Dictionary<T, string> values;
		private readonly Func<T> get;
		private readonly Action<T> set;

		public ConfigurationSelection(string name, string description, Dictionary<T, string> values, Func<T> get, Action<T> set) {
			this.name = name;
			this.description = description;
			this.values = values;
			this.get = get;
			this.set = set;
		}
		
		public string Name() {
			return name;
		}
		public string Description() {
			return description;
		}
		public Dictionary<T, string> Values() {
			return values;
		}

		public object Get() {
			return get();
		}
		public void Set(object value) {
			set((T) value);
		}
		
	}
}