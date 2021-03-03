namespace Objects.Vehicle {
	public class VehicleMovementAgression : VehicleMovement {
		public float[] MotorActivation(float[] sensorMeasurements) {
			return new[] {1f, 1f};
		}
	}
}
