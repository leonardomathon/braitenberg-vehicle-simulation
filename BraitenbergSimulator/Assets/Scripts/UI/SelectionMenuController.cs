using System;
using System.Collections.Generic;
using System.Linq;
using Configurations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = Objects.Object;
using Selectable = Objects.Selectable;

namespace UI {
	public class SelectionMenuController : MonoBehaviour {
		public TextMeshProUGUI selectedObjectTag;
		public GameObject configurations;
		public GameObject actionsContainer;
		public GameObject actions;

		public UIButton[] actionUI = new UIButton[4];
		public ConfigurationInput[] configurationUI;

		private Dictionary<SelectableButton, UIButton> actionButtons;
		
		private SelectionController selectionController;

		private void Start() {
			// Get instance of selection controller
			selectionController = SelectionController.Instance;

			actionButtons = new Dictionary<SelectableButton, UIButton> {
				{SelectableButton.Deselect, actionUI[0]}, 
				{SelectableButton.Delete, actionUI[1]}, 
				{SelectableButton.Move, actionUI[2]}, 
				{SelectableButton.Rotate, actionUI[3]}
			};

			selectionController.ObjectSelected += ObjectSelected;
			selectionController.ObjectDeselected += ObjectDeselected;
		}

		private void ObjectSelected(Selectable selected) {
			SetSelectedObjectText(selected.Name());
			SetSelectedConfigurations(selected.Configuration());
			SetSelectedActions(AppendDeselectButton(selected.Actions()));
		}
		private void ObjectDeselected(Selectable deselected) {
			ResetSelectedObjectText();
			ResetSelectedConfigurations();
			ResetSelectedActions();
		}

		private List<Tuple<Action, SelectableButton>> AppendDeselectButton(List<Tuple<Action, SelectableButton>> actions) {
			actions.Insert(0, new Tuple<Action, SelectableButton>(ClickDeselectButton, SelectableButton.Deselect));
			return actions;
		}
		private void ClickDeselectButton() {
			selectionController.ResetSelectedObject();
		}

		private void SetSelectedObjectText(string text) {
			// Set the text of the tooltip
			selectedObjectTag.SetText(text);

			// Update text mesh to prevent bug
			selectedObjectTag.ForceMeshUpdate();
		}
		private void ResetSelectedObjectText() {
			// Set the text of the tooltip
			selectedObjectTag.SetText("No object selected");

			// Update text mesh to prevent bug
			selectedObjectTag.ForceMeshUpdate();
		}
		
		private void SetSelectedConfigurations(IReadOnlyCollection<Configuration> selectedConfigurations) {
			ResetSelectedConfigurations();
			if (selectedConfigurations.Count > 0) {
				configurations.SetActive(true);
			}
			foreach (var configuration in selectedConfigurations) {
				// For each configuration, find the highest 'priority' input element that accepts it
				foreach (var ui in configurationUI) {
					if (ui.AcceptsConfiguration(configuration)) {
						// If accepted, instantiate it and continue to the next configuration
						ConfigurationInput element = Instantiate(ui, Vector3.zero, Quaternion.identity, configurations.transform);
						element.SetConfiguration(configuration);
						break;
					}
				}
			}
		}
		private void ResetSelectedConfigurations() {
			configurations.SetActive(false);
			foreach (Transform child in configurations.transform) {
				Destroy(child.gameObject);
			}
		}
		
		private void SetSelectedActions(IReadOnlyCollection<Tuple<Action, SelectableButton>> selectedActions) {
			ResetSelectedActions();
			if (selectedActions.Count > 0) {
				actionsContainer.SetActive(true);
			}
			foreach (var (action, button) in selectedActions) {
				UIButton element = Instantiate(actionButtons[button], Vector3.zero, Quaternion.identity, actions.transform);
				element.SetAction(action);
			}
		}
		private void ResetSelectedActions() {
			actionsContainer.SetActive(false);
			foreach (Transform child in actions.transform) {
				Destroy(child.gameObject);
			}
		}
	}
}