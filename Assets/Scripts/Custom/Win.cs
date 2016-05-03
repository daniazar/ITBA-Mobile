using UnityEngine;
using System.Collections;

public class Win : MonoBehaviour {

	public float maxTime = 4;
	private float time =0;
	public GUISkin skin;
	
	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width/2 -50, 70 ,240,20), "Money Collected: " + Score.money.ToString(), skin.GetStyle("LoseSkin"));
		GUI.Label(new Rect(Screen.width/2 -50, 150 ,240,20), "Time Elapsed: " + Score.timer.ToString(), skin.GetStyle("LoseSkin"));
		
		time += Time.deltaTime;
		if(Input.anyKeyDown  && time > maxTime)
		{
			
			Application.LoadLevel("MainMenu");
		}
	}
		
	
}