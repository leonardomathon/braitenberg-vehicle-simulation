using System.Collections;
using System.Collections.Generic;
using Objects.Light;
using Objects.Vehicle;
using UnityEngine;

public class GameManager : MonoBehaviour {
	// Max vehicles, lights and other objects inside the scene
	public int maxVehicles = 5;
	public int maxLights = 10;
	public int maxVarious = 10;

	// List that holds the Braitenberg vehicles
	public List<Vehicle> vehicles = new List<Vehicle>();

	// List that holds the lights that affect the vehicles
	public List<Lightbulb> lights = new List<Lightbulb>();

	// List that holds all the other objects spawned in scene
	public List<Objects.Object> various = new List<Objects.Object>();

	// Mask for the vehicles
	public LayerMask vehicleMask;

	// Mask for the lights
	public LayerMask lightMask;

	// Mask for the other objects
	public LayerMask variousMask;

	// Camera controller
	private CameraController cameraController;

	// Singleton pattern for GameManager

	#region singleton

	public static GameManager Instance {get; private set;}

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
		} else {
			Instance = this;
		}
	}
	#endregion

	private void Start() {
		cameraController = CameraController.Instance;
	}

	public bool AllowSpawnVehicle() {
		return vehicles.Count < maxVehicles;
	}

	public bool AllowSpawnLights() {
		return lights.Count < maxLights;
	}

	public bool AllowSpawnVarious() {
		return various.Count < maxVarious;
	}

	public void ClearScene() {
		// Reset camera
		cameraController.ResetTarget();

		// Remove all vehicles from the scene
		foreach (var vehicle in vehicles) {
			Destroy(vehicle.gameObject);
		}

		foreach (var bulb in lights) {
			Destroy(bulb.gameObject);
		}

		foreach (var obj in various) {
			Destroy(obj.gameObject);
		}

		// Remove all objects from list
		vehicles.Clear();
		lights.Clear();
		various.Clear();
	}

	public List<Lightbulb> GetLights() {
		// When we implement cars with lights on them, this function should also return those lights
		return lights;
	}
}