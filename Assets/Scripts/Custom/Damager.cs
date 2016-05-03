using UnityEngine;
using System.Collections;

public class Damager : MonoBehaviour {

	public EnemyMover mover;
	public int Damage = 10;
	 
	void OnTriggerEnter(Collider otherObject){
		
		//Debug.Log("Choque con" + otherObject.gameObject.name);	
		if(otherObject.gameObject.tag == "Player")
		{
			LifeManager.getHit(10);
			mover.reset();
		}
	}

	


}
