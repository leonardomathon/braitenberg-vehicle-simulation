using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Objects {
	public class Object : Selectable {
		// Random object id
		private int objectId;

		// Boolean that stores if object is currently movable
		public bool isMovable;
		private bool moving;


		// The layer mask on which we can move object
		public LayerMask moveableAreaMask;

		protected GameManager gameManager;
		private SoundManager soundManager;
		private CameraController cameraController;


		public override List<Tuple<Action, SelectableButton>> Actions() {
			return new List<Tuple<Action, SelectableButton>> {
				new Tuple<Action, SelectableButton>(EnableMovement, SelectableButton.Move),
				new Tuple<Action, SelectableButton>(Rotate, SelectableButton.Rotate),
				new Tuple<Action, SelectableButton>(Delete, SelectableButton.Delete),
			};
		}

		protected new void Start() {
			base.Start();
			objectId = Random.Range(1, int.MaxValue);
			gameManager = GameManager.Instance;
			soundManager = SoundManager.Instance;
			cameraController = CameraController.Instance;
		}
		protected void Update() {
			if (moving) {
				Move();
			}
			if (moving && Input.GetMouseButtonDown(0)) {
				PlaceObject();
			}
		}
		
		// TODO: Clean up this mess
		// TODO: There is some issue with re-activating vehicles after moving

		// Getter for object id
		public string GetObjectId() {
			return objectId.ToString();
		}

		// Getter and setters for isMovable
		public bool IsMovable() {
			return isMovable;
		}

		private void EnableMovement() {
			moving = true;
			PickUp();

			cameraController.EnableOverviewCamera();
			cameraController.UnfollowTarget();
		}

		public void PickUp() {
			isMovable = true;

			// Temporarily disable gravity for the object
			foreach (Rigidbody r in GetComponentsInChildren<Rigidbody>()) {
				r.useGravity = false;
			}

			// Disable collisions
			foreach (Collider c in GetComponentsInChildren<Collider>()) {
				c.enabled = false;
			}
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
						transform.position.y,
						Mathf.Ceil(hit.point.z)
					);

					transform.position = destinationPos;
				}
			}
		}

		public void PlaceObject() {
			// Play audio file
			soundManager.PlayPlaceObjectSound();

			// Disable camera overview mode & target this object
			cameraController.DisableOverviewCamera(gameObject);
			cameraController.FollowTarget();

			// Set object to unmovable
			moving = false;
			Place();
		}
		public void Place() {
			isMovable = false;

			// Enable gravity for the object
			foreach (Rigidbody r in GetComponentsInChildren<Rigidbody>()) {
				r.useGravity = false;
			}

			// Enable collisions
			foreach (Collider c in GetComponentsInChildren<Collider>()) {
				c.enabled = true;
			}
		}

		private void Rotate() {
			// Play object rotate sound
			soundManager.PlayRotateObjectSound();

			// Rotate 45 degrees around the y axis
			Quaternion currentRotation = transform.rotation;
			transform.rotation = currentRotation * Quaternion.Euler(0, 45.0f, 0);
		}

		private void Delete() {
			// Play object delete sound
			soundManager.PlayDeleteObjectSound();

			// Deselect before destroying
			Deselect();
			Destroy(gameObject);
			gameManager.DeletedObject(this);
		}
	}
}