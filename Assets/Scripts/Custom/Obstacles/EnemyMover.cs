using UnityEngine;
using System.Collections;

public class EnemyMover : MonoBehaviour {

	public float initialPosition = 100.0f;
	public float finalPosition = -50.0f;
	private Transform trans;
	public CircularObstacle obstacle;
	public RectangularObstacle obstacleRectangular;
	public OvniObstacle obstacleOvni;
	
		// Use this for initialization
	void Start () {
		trans = transform; 
	}
	
	
	// Update is called once per frame
	void Update () {

		if(trans.position.z < finalPosition) //Reset obstacle
		{	
			reset();
			return;
		}
		trans.position = new Vector3(0 , 0 , trans.position.z - GameDifficulty.obstacleSpeed * Time.deltaTime );	
	}
	
	
	public void reset()
	{
		if(obstacle)
			obstacle.rebuild();
		if(obstacleRectangular)
			obstacleRectangular.rebuild();
		if(obstacleOvni)
			obstacleOvni.rebuild();	
		trans.position = new Vector3(0 , 0 , initialPosition );	
	}
	
}
