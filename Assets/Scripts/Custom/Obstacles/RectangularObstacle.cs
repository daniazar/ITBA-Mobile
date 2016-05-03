using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]


public class RectangularObstacle : MonoBehaviour 
{


	System.Random rand  = new System.Random(randomSeed);
	int randomAngle;
	public static int randomSeed = (new System.Random()).Next();

	private Transform trans;
	
	void Start(){
		trans = gameObject.transform;	
	}
	
	public void GenerateEasyElipicPath()
	{
		rand = new System.Random(randomSeed);
		trans.Rotate( - Vector3.forward * randomAngle);
		int randomStart = rand.Next(40);
		int randomHeight = rand.Next(30);
		int randomWidth = rand.Next(80);
		randomAngle = rand.Next(360);
		trans.position = new Vector3(randomStart ,0 ,0);
		trans.localScale = new Vector3(randomWidth , randomHeight, 1f);
		trans.Rotate(Vector3.forward * randomAngle);
		trans.position = new Vector3(trans.position.x ,trans.position.y ,0);
		 
	}
	
	public void rebuild()
	{
			randomSeed = rand.Next();		
			GenerateEasyElipicPath();
	
	}
	
}


