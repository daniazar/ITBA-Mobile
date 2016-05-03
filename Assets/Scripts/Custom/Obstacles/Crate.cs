using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {

		
	public float forwardThrust = 150;
    public float turnThrust = 100F;
	float x;
	float z;  
	// Update is called once per frame
	void Start () {
		
		 x = Random.Range(-20, 20);
		 z = Random.Range(-20, 20);
		
		
		rigidbody.velocity = new Vector3(x , z, 0);
		rigidbody.angularVelocity = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
		 
		StartCoroutine (Move());
	}

	
	
	// Update is called once per frame
	void Update () {

		
	}
	
	IEnumerator Move(){
		while(true){
			x = Random.Range(-20, 20);
			z = Random.Range(-20, 20);
			rigidbody.velocity = new Vector3(x , z, 0);
			
			yield return new WaitForSeconds(3);
		}
	}
}
