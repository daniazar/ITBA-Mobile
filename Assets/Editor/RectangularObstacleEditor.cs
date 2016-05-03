using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(RectangularObstacle))]

public class RectangularObstacleEditor : Editor 
{
	
	public override void OnInspectorGUI() 
	{
	RectangularObstacle pathScript = (RectangularObstacle) target as RectangularObstacle;
		
	
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			
			Rect startButton = EditorGUILayout.BeginHorizontal();
			startButton.x = startButton.width / 2 - 100;
			startButton.width = 200;
			startButton.height = 18;

			if (GUI.Button(startButton, "Add Random path")) 
			{
				RectangularObstacle.randomSeed	= (new System.Random()).Next();		
				pathScript.GenerateEasyElipicPath();
				
					GUIUtility.ExitGUI();
			}			
			
			EditorGUILayout.EndHorizontal();
	
			EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		
	}
	
}
