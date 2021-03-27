using Objects;
using Objects.Light;
using Objects.Vehicle;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = Objects.Object;

public class SelectionController : MonoBehaviour {
	// The layer mask on which we can select objects
	public LayerMask selectableLayers;
	
	private Selectable selectedObject;
	private bool hasSelectedObject;

	private GameManager gameManager;
	private SoundManager soundManager;
	private CameraController cameraController;

	public delegate void SelectHandler(Selectable selected);
	public delegate void DeselectHandler(Selectable deselected);
	public event SelectHandler ObjectSelected;
	public event DeselectHandler ObjectDeselected;

	// Singleton pattern for SelectionController
	#region singleton

	public static SelectionController Instance {get; private set;}

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
		} else {
			Instance = this;
		}
	}

	#endregion

	private void Start() {
		gameManager = GameManager.Instance;
		soundManager = SoundManager.Instance;
		cameraController = CameraController.Instance;
	}
	private void LateUpdate() {
		if (Input.GetMouseButtonDown(0)) {
			// TODO: In refactoring, there is no more check if we are moving some object
			// Now, clicking on another object while moving will both place the current object, and directly try to select a new one
			SelectObject();
		}
		
		// Check if selected object still exists, if not, update state
		if (hasSelectedObject && selectedObject == null) {
			ResetSelectedObject();
		}
	}

	public void SetSelectedObject(Selectable selected) {
		// Select all objects to make sure only one object is selected
		// TODO: We know we can only select one object, and we know the object we have selected
		DeselectAllObjects();

		// Play object select sound
		soundManager.PlaySelectObjectSound();

		// Set the selected object and update listeners
		selectedObject = selected;
		hasSelectedObject = true;
		ObjectSelected?.Invoke(selected);

		// Select the object
		selected.Select();

		// Let camera orbit around selected object
		cameraController.SetTarget(selected.gameObject);
	}
	public void ResetSelectedObject() {
		// Play object deselect sound
		soundManager.PlayDeselectObjectSound();

		// Reset the selected object and update listeners
		ObjectDeselected?.Invoke(selectedObject);
		selectedObject = null;
		hasSelectedObject = false;

		// Deselect all objects
		DeselectAllObjects();

		// Let camera orbit the default target
		cameraController.ResetTarget();
	}

	private void SelectObject() {
		// First check if mouse is currently over UI, do nothing
		if (EventSystem.current.IsPointerOverGameObject()) return;
		if (EventSystem.current.currentSelectedGameObject != null) return;

		// Shoot a ray from main camera through screen point
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		// Check if ray hits something with the mask "Vehicle"
		if (Physics.Raycast(ray, out RaycastHit hit, 100, selectableLayers)) {
			GameObject hitObject = hit.transform.gameObject;
			Selectable selectable = hitObject.GetComponent<Selectable>();

			// Check if the object is a subclass of Selectable
			if (selectable != null) {
				if (!selectable.IsSelected()) {
					// Select object only if it is not currently selected
					SetSelectedObject(selectable);
				} else {
					// If it is currently selected, deselect it and reset camera
					ResetSelectedObject();
				}
			}
		}
	}

	private void DeselectAllObjects() {
		foreach (var obj in gameManager.vehicles) {
			obj.Deselect();
		}
		foreach (var obj in gameManager.various) {
			obj.Deselect();
		}
		foreach (var obj in gameManager.lights) {
			obj.Deselect();
		}
	}
}