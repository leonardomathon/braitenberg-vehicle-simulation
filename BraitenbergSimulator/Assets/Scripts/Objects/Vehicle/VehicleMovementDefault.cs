namespace Objects.Vehicle {
	public class VehicleMovementDefault : VehicleMovement {
		public float[] MotorActivation(float[] sensorMeasurements) {
			// Make the default behaviour simply be all motors at equal force
			var result = new float[sensorMeasurements.Length];
			for (var i = 0; i < result.Length; i++) {
				result[i] = 1f;
			}
			return result;
		}
	}
}