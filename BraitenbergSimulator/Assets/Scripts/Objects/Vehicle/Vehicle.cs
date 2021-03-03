using Objects.Vehicle;
using Objects.Vehicle.Motors;
using UnityEngine;

[System.Serializable] public class Vehicle : Object {
	[SerializeField] private VehicleType type;

	private VehicleType _type;

	[SerializeField] private Vector3 position;

	public GameObject body;

	public Wheel leftWheel;

	public Sensor leftSensor;
	public Sensor rightSensor;

	public int leftMotorSpeed;
	public int rightMotorSpeed;

	private GameManager gameManager;
	private VehicleMovement movement;
	private float time;

	private void Start() {
		AttachMovementScript();
	}

	protected override void Update() {
		// base.Update();
		// UpdateMovementScript();
		UpdateBodyRotation();

		time += Time.deltaTime;
		if (time > 5f) {
			Debug.Log(leftSensor.Measure(gameManager.GetLights()));
			time = 0f;
		}


		float motor = leftMotorSpeed * Input.GetAxis("Vertical");

		// if (axleInfo.steering) {
		// 	axleInfo.leftWheel.steerAngle = steering;
		// 	axleInfo.rightWheel.steerAngle = steering;
		// }
		// if (axleInfo.motor) {
		leftWheel.SetForce(motor);
		// rightWheel.motorTorque = motor;
		// }
		// ApplyLocalPositionToVisuals(axleInfo.leftWheel);
		// ApplyLocalPositionToVisuals(axleInfo.rightWheel);
	}

	// Attach the rigt movementscript to the vehicle object based on VehicleType
	private void AttachMovementScript() {
		_type = type;
		SetMovementType(type);
	}
	private void SetMovementType(VehicleType type) {
		switch (type) {
			case VehicleType.Default:
				gameObject.AddComponent<VehicleDefaultMovement>();
				break;
			case VehicleType.Agression:
				movement = new VehicleMovementAgression();
				break;
			case VehicleType.Exploration:
				gameObject.AddComponent<VehicleMovementExploration>();
				break;
			case VehicleType.Fear:
				gameObject.AddComponent<VehicleMovementFear>();
				break;
			case VehicleType.Love:
				gameObject.AddComponent<VehicleMovementLove>();
				break;
		}
	}

	// Update movementscript if VehicleType has changed
	private void UpdateMovementScript() {
		// TODO: This could and probably should be replaced with some event-like call when the type is actually changed
		if (type != _type) {
			_type = type;
			SetMovementType(type);
		}
	}

	private void UpdateBodyRotation() {
		var transformLocalRotation = body.transform.rotation;
		transformLocalRotation.x = 0;
		transformLocalRotation.z = 0;
		body.transform.rotation = transformLocalRotation;
	}

	public void SetGameManager(GameManager gameManager) {
		this.gameManager = gameManager;
	}
}