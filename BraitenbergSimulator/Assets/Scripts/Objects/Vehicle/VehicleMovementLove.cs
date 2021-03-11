namespace Objects.Vehicle {
	public class VehicleMovementLove : VehicleMovement {
		public float[] MotorActivation(float[] sensorMeasurements) {
			// Sensor measurements only need to be inverted.
			// Characteristic behaviour then emerges from a base motor speed, which is expected but not required.
			var result = new float[sensorMeasurements.Length];
			
			for (var i = 0; i < result.Length; i++) {
				result[i] = -sensorMeasurements[i];
			}
			
			return result;
		}
	}
}