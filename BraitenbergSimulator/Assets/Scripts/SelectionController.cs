using Objects;
using Objects.Light;
using Objects.Vehicle;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = Objects.Object;

public class SelectionController : MonoBehaviour {
	[SerializeField] private Selectable selectedObject;

	[SerializeField] private bool selectedObjectMovable;

	// The layer mask on which we can select objects
	[SerializeField] private LayerMask selectableAreaMasks;

	// The layer mask on wich we can move object
	[SerializeField] private LayerMask moveableAreaMask;

	private GameManager gameManager;

	private SoundManager soundManager;

	private CameraController cameraController;

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

	private void Update() {
		if (!selectedObjectMovable && Input.GetMouseButtonDown(0)) {
			SelectObject();
		}

		if (selectedObject && Input.GetKeyDown(KeyCode.M)) {
			MoveSelectedObject();
		}

		if (selectedObject && Input.GetKeyDown(KeyCode.R)) {
			RotateSelectedObject();
		}

		if (selectedObjectMovable) {
			Move();
		}

		if (selectedObjectMovable && Input.GetMouseButtonDown(0)) {
			PlaceSelectedObject();
		}

		// Check if selected object still exists
		if (selectedObject == null) {
			selectedObject = null;
		}
	}

	public Selectable GetSelectedObject() {
		return selectedObject;
	}

	private void ResetSelection() {
		cameraController.ResetTarget();
	}

	public void DeleteSelectedObject() {
		// Play object delete sound
		soundManager.PlayDeleteObjectSound();

		// Deselect before destroying
		DeselectAllObjects();

		// Cleanup object list, brute force
		foreach (var vehicle in gameManager.vehicles) {
			if (vehicle == selectedObject) {
				gameManager.vehicles.Remove((Vehicle) selectedObject);
				Destroy(selectedObject);
				return;
			}
		}
		foreach (var various in gameManager.various) {
			if (various == selectedObject) {
				gameManager.various.Remove((Object) selectedObject);
				Destroy(selectedObject);
				return;
			}
		}
		foreach (var bulb in gameManager.lights) {
			if (bulb == selectedObject) {
				gameManager.lights.Remove((Lightbulb) selectedObject);
				Destroy(selectedObject);
				return;
			}
		}
	}

	public void SetSelectedObject(Selectable selectable) {
		// Select all objects to make sure only one object is selected
		DeselectAllObjects();

		// Play object select sound
		soundManager.PlaySelectObjectSound();

		// Set the selected object
		selectedObject = selectable;

		// Select the object
		selectable.Select();

		// Let camera orbit around selected object
		cameraController.SetTarget(selectable.gameObject);
	}

	public void ResetSelectedObject() {
		// Play object deselect sound
		soundManager.PlayDeselectObjectSound();

		// Reset the selected object
		selectedObject = null;

		// Deselect all objects
		DeselectAllObjects();
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
			obj.Deselect();
		}
		foreach (var obj in gameManager.various) {
			obj.Deselect();
		}
		foreach (var obj in gameManager.lights) {
			obj.Deselect();
		}

		// Let camera orbit the default target
		cameraController.ResetTarget();
	}

	public void MoveSelectedObject() {
		cameraController.EnableOverviewCamera();
		selectedObjectMovable = true;
		selectedObject.GetComponent<Object>().Move();
		cameraController.UnfollowTarget();
	}

	private void Move() {
		Debug.Log(cameraController.CameraIsMoving());
		if (!cameraController.CameraIsMoving()) {
			// Shoot ray
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out var hit, 100, moveableAreaMask)) {
				Debug.Log("Ray cast");
				var destinationPos = new Vector3(
					Mathf.Ceil(hit.point.x),
					selectedObject.transform.position.y,
					Mathf.Ceil(hit.point.z)
				);

				selectedObject.transform.position = destinationPos;
			}
		}
	}

	private void PlaceSelectedObject() {
		// Play audio file
		soundManager.PlayPlaceObjectSound();

		// Disable camera overview mode
		cameraController.DisableOverviewCamera(selectedObject.gameObject);

		// Set object to unmovable
		selectedObject.GetComponent<Object>().Place();

		// Set movable boolean to false
		selectedObjectMovable = false;

		// Follow the target again
		cameraController.FollowTarget();
	}

	public void RotateSelectedObject() {
		// Play object rotate sound
		soundManager.PlayRotateObjectSound();

		// Get current rotation
		Quaternion currentRotation = selectedObject.transform.rotation;

		// Rotate 45 degrees around the y axis
		selectedObject.transform.rotation = currentRotation * Quaternion.Euler(0, 45.0f, 0);
	}
}