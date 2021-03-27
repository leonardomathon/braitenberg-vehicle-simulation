using System;
using Configurations;
using TMPro;
using UnityEngine;

namespace UI {
	public class NumberInput : ConfigurationInput {
		public TextMeshProUGUI title;
		public TMP_InputField input;

		private Configuration configuration;

		public override void SetConfiguration(Configuration configuration) {
			this.configuration = configuration;
			SetTitle(configuration.Name());
			SetValue(configuration.Get());
		}
		public override bool AcceptsConfiguration(Configuration configuration) {
			return configuration is ConfigurationFloat || configuration is ConfigurationRange;
		}
		private void SetTitle(string title) {
			this.title.text = title;
		}
		private void SetValue(object value) {
			input.text = "" + value;
		}

		public void ValueChanged(string value) {
			try {
				configuration.Set(float.Parse(value));
				SetValue(configuration.Get());
			} catch (FormatException) {}
		}
	}
}