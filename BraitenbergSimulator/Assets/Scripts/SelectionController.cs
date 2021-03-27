using Objects;
using Objects.Light;
using Objects.Vehicle;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = Objects.Object;

public class SelectionController : MonoBehaviour {
	[SerializeField] private Selectable selectedObject;

	// TODO: This is set to true while the object is moving. It should be named as such, and also not be serializable
	[SerializeField] private bool selectedObjectMovable;

	// The layer mask on which we can select objects
	[SerializeField] private LayerMask selectableAreaMasks;

	// The layer mask on which we can move object
	[SerializeField] private LayerMask moveableAreaMask;

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

	public void DeleteSelectedObject() {
		// Play object delete sound
		soundManager.PlayDeleteObjectSound();

		// Deselect before destroying
		DeselectAllObjects();

		// Cleanup object list, brute force
		foreach (Objects.Vehicle.Vehicle vehicle in gameManager.vehicles) {
			if (vehicle == selectedObject) {
				gameManager.vehicles.Remove(vehicle);
				Destroy(selectedObject.gameObject);
				return;
			}
		}
		foreach (Objects.Object various in gameManager.various) {
			if (various == selectedObject) {
				gameManager.various.Remove(various);
				Destroy(selectedObject.gameObject);
				return;
			}
		}
		foreach (Objects.Light.Lightbulb bulb in gameManager.lights) {
			if (bulb == selectedObject) {
				gameManager.lights.Remove(bulb);
				Destroy(selectedObject.gameObject);
				return;
			}
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
		if (Physics.Raycast(ray, out RaycastHit hit, 100, selectableAreaMasks)) {
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

	public void MoveSelectedObject() {
		cameraController.EnableOverviewCamera();
		selectedObjectMovable = true;
		selectedObject.GetComponent<Object>().Move(); // TODO: This will break when selecting a wheel, perform some sort of check on instance of Object
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