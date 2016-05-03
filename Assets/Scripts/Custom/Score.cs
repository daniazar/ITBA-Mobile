using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	public static int money = 0;
	public static float timer = 0;
	public bool reset = true;
	
	// Use this for initialization
	void Start () {
		if(reset)
		{
			money = 0;
			timer = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(reset)
		{
			timer = Time.timeSinceLevelLoad;
			if(timer >= 300)
				Application.LoadLevel("Win");
	
		}
	}
	
	public static void AddMoney(int mon){
		money += mon;
	}
	
	void OnGUI ()
	{
		GUI.Label (new Rect (Screen.width / 2 - 20, 10, 150, 20), "Money collected: " + money.ToString());
	}
}
