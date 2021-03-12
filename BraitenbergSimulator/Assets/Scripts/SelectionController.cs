using Objects;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionController : MonoBehaviour {
	[SerializeField] private GameObject selectedObject;

	// The layer mask on which we can select objects
	[SerializeField] private LayerMask selectableAreaMasks;

	private GameManager gameManager;

	private CameraController cameraController;

	private void Start() {
		gameManager = GameManager.Instance;
		cameraController = CameraController.Instance;
	}

	private void Update() {
		if (Input.GetMouseButtonDown(0)) {
			SelectObject();
		}
	}

	public void ResetSelection() {
		cameraController.ResetTarget();
	}

	private void SelectObject() {
		// First check if mouse is currently over UI, do nothing
		if (EventSystem.current.IsPointerOverGameObject()) return;
		if (EventSystem.current.currentSelectedGameObject != null) return;

		// Shoot a ray from main camera through screen point
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		// Check if ray hits something with the mask "Vehicle"
		if (Physics.Raycast(ray, out var hit, 100, selectableAreaMasks)) {
			var hitObject = hit.transform.gameObject;
			var selectable = hitObject.GetComponent<Selectable>();

			// Check if the object is a subclass of Selectable
			if (selectable != null) {
				if (!selectable.IsSelected()) {
					// Select object only if it is not currently selected
					DeselectAllObjects();
					selectable.Select();
					cameraController.SetTarget(hitObject);
				} else {
					// If it is currently selected, deselect it and reset camera
					DeselectAllObjects();
					cameraController.ResetTarget();
				}
			}
		}
	}

	private void DeselectAllObjects() {
		foreach (var obj in gameManager.vehicles) {
			obj.GetComponent<Selectable>().Deselect();
		}
		foreach (var obj in gameManager.various) {
			obj.GetComponent<Selectable>().Deselect();
		}
		foreach (var bulb in gameManager.lights) {
			bulb.Deselect();
		}
	}
}