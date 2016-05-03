using UnityEngine;
using System.Collections;

public class DamagerCircular : MonoBehaviour {

	public EnemyMover mover;
	public int Damage = 10;
	public int range = 4; 
	void OnTriggerEnter(Collider otherObject){
		
		Vector3 direction = otherObject.gameObject.transform.forward;
		RaycastHit hit;
		Vector3 pos = otherObject.gameObject.transform.position;
		
		// Did we hit anything?
		if (Physics.Raycast (pos, direction, out hit, range)) {
			// Apply a force to the rigidbody we hit
			if (hit.collider)
			{
//				Debug.Log(hit.collider.gameObject.name);
				
				if(hit.collider.gameObject.tag == "Enemy")
				{
					//Debug.Log("Choque con" + otherObject.gameObject.name);	
					LifeManager.getHit(10);
					mover.reset();
					
				}	
			}
			
			
			
			Debug.DrawLine(pos, hit.point, Color.red);
            //Debug.Log(hit.collider.name); // this will tell you what you are hitting
			// Send a damage message to the hit object			
			//hit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
		}
		
	}
	
	

	


}
