using UnityEngine;
using System.Collections;

public class CollisionSoundEffect : MonoBehaviour {

// Small script to hold a reference to an audioclip to play when the player hits me.

// This script is attached to game object making up your level. 
// The "Foot" script (which is attached to the player) looks for this script on whatever it touches.
// If it finds it, then it will play the sound when the foot comes in contact

public float volumeModifier = 1.0f;
	
	void OnTriggerEnter(Collider otherObject){
		if (otherObject.gameObject.tag == "Enemy") {
			Debug.Log("sonido");
			SoundManager.PlaySound((int) SndIdGame.SND_CLICK_ERROR);
		}
	}
	
}
