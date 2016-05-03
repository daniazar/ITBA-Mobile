using UnityEngine;
using System.Collections;

public class GameDifficulty : MonoBehaviour {

	public float maxSpeed = 200.0f;
	public float initialSpeed = 1.0f;
	public float speedIntervalIncrease = 10.0f;
	public float speedIncreasePerSecond = 0.1f;
	
	private float counter = 0;
	private float lastSpeedIncreaseTime = 0;
	private float lastSpeedIncrease = 0;
	public static float obstacleSpeed;
	// Use this for initialization
	void Start () {
	 obstacleSpeed = initialSpeed; 
	}
	
	// Update is called once per frame
	void Update () {
		counter += Time.deltaTime;
		if(counter > speedIntervalIncrease)  // Increase Speed
		{
			counter = 0;
			if(obstacleSpeed >= maxSpeed){ // Max Speed Achived...
				return;
			}
			
			//Calculate Speed
			lastSpeedIncrease = lastSpeedIncreaseTime * speedIncreasePerSecond;
			lastSpeedIncreaseTime = Time.timeSinceLevelLoad;
			obstacleSpeed += lastSpeedIncreaseTime * speedIncreasePerSecond - lastSpeedIncrease;
		}
	}
}
