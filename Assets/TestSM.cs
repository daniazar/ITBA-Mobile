using UnityEngine;
using System.Collections;

public class TestSM : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
	
	}
	
	void OnGUI()
	{
		if (GUILayout.Button("Trigger Click"))
		{
			SoundManager.PlaySound((int) SndIdGame.SND_CLICK);
		}
		
		if (GUILayout.Button("Trigger Click Error"))
		{
			SoundManager.PlaySound((int) SndIdGame.SND_CLICK_ERROR);
		}
		
		if (GUILayout.Button("Trigger Cancel"))
		{
			SoundManager.PlaySound((int) SndIdGame.SND_CANCEL);
		}
		
		if (GUILayout.Button("Trigger Confirm"))
		{
			SoundManager.PlaySound((int) SndIdGame.SND_CONFIRM);
		}
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}
}
