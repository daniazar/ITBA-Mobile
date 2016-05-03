using UnityEngine;

public class SpecialEffects : MonoBehaviour {
	// There are four possible special effects that can be assigned.
	public GameObject positiveThrustEffect;
	public GameObject negativeThrustEffect;
	public GameObject positiveTurnEffect;
	public GameObject negativeTurnEffect;
	// How loud should collision sounds be? This is used in the OnCollisionEnter () function.
	public float collisionVolume = 0.01f;
}
