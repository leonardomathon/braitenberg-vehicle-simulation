using System.Collections.Generic;
using Configurations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = Objects.Object;
using Selectable = Objects.Selectable;

namespace UI {
	public class SelectionMenuController : MonoBehaviour {
		public GameObject selectionMenu;
		public GameObject selectedObjectToolMenu;
		public GameObject configurations;
		public ConfigurationInput[] configurationUI;

		private TextMeshProUGUI selectedObjectTag;

		private Selectable selectedObject;

		private Button[] toolMenuButtons = new Button[4];

		private SelectionController selectionController;

		void Start() {
			// Get instance of selection controller
			selectionController = SelectionController.Instance;
			selectedObject = selectionController.GetSelectedObject();

			// Get used components
			selectedObjectTag = selectionMenu.transform.Find("SelectedObjectTag").GetComponent<TextMeshProUGUI>();

			// Set toolbar inactive
			selectedObjectToolMenu.SetActive(false);

			// Setup tool buttons
			SetupToolButtons();

			selectionController.ObjectSelected += ObjectSelected;
			selectionController.ObjectDeselected += ObjectDeselected;
		}

		private void ObjectSelected(Selectable selected) {
			selectedObject = selected;
			SetSelectedObjectText(selected.Name());
			SetSelectedConfigurations(selected.Configuration());
		}
		private void ObjectDeselected(Selectable deselected) {
			selectedObject = null;
			ResetSelectedObjectText();
			ResetSelectedConfigurations();
		}

		private void SetupToolButtons() {
			// Get the grid in which the buttons are situated
			GameObject grid = selectedObjectToolMenu.transform.Find("Grid").gameObject;

			// Get all buttons
			for (int i = 0; i < grid.transform.childCount; i++) {
				toolMenuButtons[i] = grid.transform.GetChild(i).GetComponent<Button>();
			}

			// Setup listeners
			toolMenuButtons[0].onClick.AddListener(ClickSelectButton);
			toolMenuButtons[1].onClick.AddListener(ClickDeleteButton);
			toolMenuButtons[2].onClick.AddListener(ClickMoveButton);
			toolMenuButtons[3].onClick.AddListener(ClickRotateButton);
		}

		private void ClickSelectButton() {
			selectionController.ResetSelectedObject();
		}
		private void ClickDeleteButton() {
			selectionController.DeleteSelectedObject();
		}
		private void ClickMoveButton() {
			selectionController.MoveSelectedObject();
		}
		private void ClickRotateButton() {
			selectionController.RotateSelectedObject();
		}

		private void SetSelectedObjectText(string text) {
			// Set the text of the tooltip
			selectedObjectTag.SetText(text);

			// Activate toolbar
			selectedObjectToolMenu.SetActive(true);

			// Update text mesh to prevent bug
			selectedObjectTag.ForceMeshUpdate();
		}
		private void ResetSelectedObjectText() {
			// Set the text of the tooltip
			selectedObjectTag.SetText("No object selected");

			// Deactivate toolbar
			selectedObjectToolMenu.SetActive(false);

			// Update text mesh to prevent bug
			selectedObjectTag.ForceMeshUpdate();
		}
		private void SetSelectedConfigurations(IEnumerable<Configuration> selectedConfigurations) {
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
			foreach (Transform child in configurations.transform) {
				Destroy(child.gameObject);
			}
		}
	}
}