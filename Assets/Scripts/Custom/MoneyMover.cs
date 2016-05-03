using UnityEngine;
using System.Collections;

public class MoneyMover : MonoBehaviour {

	public float initialPosition = 100.0f;
	public float finalPosition = -50.0f;
	private Transform trans;
	public int limits =40;	
	public System.Random rand;
	public int randomSeed = 0;
		// Use this for initialization
	void Start () {
		trans = transform;
		rand  = new System.Random(randomSeed); 
	}
	
	
	// Update is called once per frame
	void Update () {

		if(trans.position.z < finalPosition) //Reset obstacle
		{	
			reset();
			return;
		}
		trans.position = new Vector3(trans.position.x , trans.position.y , trans.position.z - GameDifficulty.obstacleSpeed * Time.deltaTime );	
	}
	
	
	public void reset()
	{
		trans.position = new Vector3(rand.Next(limits)-20 , rand.Next(limits)-20 , initialPosition );
			
	}
	
}
