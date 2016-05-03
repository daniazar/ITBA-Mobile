using UnityEngine;
using System.Collections;

public class OvniObstacle : MonoBehaviour {

	private Transform trans;
	private Vector3 targetA;
	private Vector3 targetB;
	private float weights;
	public float speed = 0.5f;
	public int limits = 40;
	System.Random rand  = new System.Random(randomSeed);
	public static int randomSeed = (new System.Random()).Next();

	void Start(){
		trans = transform;
		rebuild();
	}
	

	void LateUpdate () {
		weights = Mathf.Cos(Time.time * speed * 2f * Mathf.PI) * 0.5f + 0.5f;
		trans.localPosition = targetA * weights + targetB * (1-weights);
	}
	
	public void rebuild()
	{	
			targetA = new Vector3(rand.Next(limits)-20 , rand.Next(limits)-20 ,0 )  ;
			targetB = new Vector3(rand.Next(limits)-20 , rand.Next(limits)-20 ,0 ) ;
	}
	
}