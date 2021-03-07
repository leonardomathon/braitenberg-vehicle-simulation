namespace Objects.Vehicle {
	public class VehicleMovementFear : VehicleMovement {
		public float[] MotorActivation(float[] sensorMeasurements) {
			// Sensor measurements map directly to motor outs in this case, no manipulation needed
			return sensorMeasurements;
		}
	}
}