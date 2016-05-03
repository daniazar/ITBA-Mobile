using UnityEngine;
using System.Collections;

public class MovementSettings : MonoBehaviour {
	
	// What is the maximum speed of this movement?
	public float maxSpeed;
	// What's the acceleration in the positive and negative directions associated with this movement?
	public float positiveAcceleration;
	public float negativeAcceleration;
	// How much drag should we apply when there isn't input for this movement?
	public float dragWhileCoasting;
	// How much drag should we apply to slow down the movement for speeds above maxSpeed?
	public float dragWhileBeyondMaxSpeed ;
	// When neither of the above drag factors are in play, how much drag should there normally be?  (Usually very small.)
	public float dragWhileAcceleratingNormally;
	// This function determines which drag variable to use and returns one.
	public float ComputeDrag (float input , Vector3 velocity) {
		//Is the input not zero (the 0.01 allows for some error since we're working with floats and they aren't completely precise)
		if (Mathf.Abs (input) > 0.01) {
			// Are we greater or less than our max speed? Then return the appropriate drag.
			if (velocity.magnitude > maxSpeed)
				return dragWhileBeyondMaxSpeed;
			else
				return dragWhileAcceleratingNormally;
		} else
			//If the input is zero, use dragWhileCoasting
			return dragWhileCoasting;
	}
}

