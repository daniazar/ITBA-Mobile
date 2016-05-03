using UnityEngine;
using System.Collections;

public class Lose : MonoBehaviour {

	public Texture backgroundTexture;
	public float maxTime = 4;
	private float time =0;
	public GUISkin skin;

	void OnGUI()
	{
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height ), backgroundTexture);	
		GUI.Label(new Rect(Screen.width/2 , 200 ,240,20), "Money Collected: " + Score.money.ToString(), skin.GetStyle("LoseSkin"));
		GUI.Label(new Rect(Screen.width/2 , 180 ,240,20), "Time Elapsed: " + Score.timer.ToString(), skin.GetStyle("LoseSkin"));
			time += Time.deltaTime;

		if(Input.anyKeyDown  && time > maxTime)
		{
			
			Application.LoadLevel("MainMenu");
		}
	}
		
	
}