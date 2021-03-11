namespace Objects.Vehicle {
	public class VehicleMovementExploration : VehicleMovement {
		public float[] MotorActivation(float[] sensorMeasurements) {
			// Do this slightly more complicated, in order to account for possible future vehicles with more than 1 set of motors
			// Inverted power combined with motor base speed then produces the characteristic behaviour

			var result = new float[sensorMeasurements.Length];

			// Per pair of two sensor inputs, switch them around
			for (var i = 1; i < result.Length; i += 2) {
				result[i - 1] = -sensorMeasurements[i];
				result[i] = -sensorMeasurements[i - 1];
			}
			
			// If there are somehow an odd number of sensors and motors, just copy the last measurement
			if (result.Length % 2 == 1) {
				result[result.Length - 1] = -sensorMeasurements[sensorMeasurements.Length - 1];
			}
			
			return result;
		}
	}
}