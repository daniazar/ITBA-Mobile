using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public GUISkin skin;
	private string instructionText = "Instructions:\nPress Left, Right and Up to move.\nTo win you need to survive for 300 seconds.";
	private int buttonWidth = 200;
	private int buttonHeight = 50;
	private int groupWidth = 300;
	private int groupHeight = 450;
	
	
	void OnGUI()
	{
			GUI.Label(new Rect(Screen.width / 2 - groupWidth / 2 + 10, 10, 300, 200 ), instructionText);
			GUI.Box (new Rect (Screen.width / 2 - groupWidth / 2, Screen.height / 2 - groupHeight / 2 , groupWidth, groupHeight), "", GUI.skin.GetStyle("box"));
	        GUI.Label (new Rect (groupWidth/2 - 30, Screen.height/4 + 20, groupWidth, groupHeight), "StarDom", skin.GetStyle("LoseSkin"));
           	if(GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, Screen.height / 2 , buttonWidth, buttonHeight), "Start Normal Game "))
			{
				SoundManager.PlaySound((int) SndIdGame.SND_CLICK);
				Application.LoadLevel("2D Platformer");
			}
			
    	
	}	
}
