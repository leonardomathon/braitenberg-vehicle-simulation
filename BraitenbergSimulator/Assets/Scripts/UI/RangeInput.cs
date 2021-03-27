using System;
using Configurations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class RangeInput : ConfigurationInput {
		public TextMeshProUGUI title;
		public Slider slider;

		private Configuration configuration;

		public override void SetConfiguration(Configuration configuration) {
			this.configuration = configuration;
			SetTitle(configuration.Name());
			SetValue(configuration.Get());
			if (configuration is ConfigurationRange range) {
				SetMinimum(range.Minimum());
				SetMaximum(range.Maximum());
			}
		}
		public override bool AcceptsConfiguration(Configuration configuration) {
			return configuration is ConfigurationRange;
		}
		private void SetTitle(string title) {
			this.title.text = title;
		}
		private void SetMinimum(float value) {
			slider.minValue = value;
		}
		private void SetMaximum(float value) {
			slider.maxValue = value;
		}
		private void SetValue(object value) {
			slider.value = (float) value;
		}

		public void ValueChanged(float value) {
			try {
				configuration.Set(value);
				SetValue(configuration.Get());
			} catch (FormatException) {}
		}
	}
}