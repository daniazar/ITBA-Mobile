using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(CircularObstacle))]

public class CircularObstacleEditor : Editor 
{
	public void OnSceneGUI()
	{
		
		CircularObstacle pathScript = (CircularObstacle) target as CircularObstacle;
				
		if (pathScript.nodeObjects != null && pathScript.nodeObjects.Length != 0) 
		{
			int n = pathScript.nodeObjects.Length;
			for (int i = 0; i < n; i++) 
			{
				PathNodeObjects node = pathScript.nodeObjects[i];
				node.position = Handles.PositionHandle(node.position, Quaternion.identity);
			}
		}
		if (GUI.changed) 
		{
				pathScript.CreatePath(pathScript.pathSmooth, false);
						}
	}
	
	
	
	public override void OnInspectorGUI() 
	{
		EditorGUIUtility.LookLikeControls();
		CircularObstacle pathScript = (CircularObstacle) target as CircularObstacle;

			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			pathScript.pathWidth = (int) EditorGUILayout.IntSlider("Path Width", pathScript.pathWidth, 3, 20);
			EditorGUILayout.EndHorizontal();
					
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			pathScript.pathSmooth = (int) EditorGUILayout.IntSlider("Mesh Smoothing", pathScript.pathSmooth, 5, 60);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			
			Rect clearButton = EditorGUILayout.BeginHorizontal();
			clearButton.x = clearButton.width / 2 - 100;
			clearButton.width = 200;
			clearButton.height = 18;
			if (GUI.Button(clearButton, "Reset Mesh")) 
			{
				pathScript.NewPath();
				GUIUtility.ExitGUI();
				
			}
			
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			clearButton = EditorGUILayout.BeginHorizontal();
			clearButton.x = clearButton.width / 2 - 100;
			clearButton.width = 200;
			clearButton.height = 18;
			if (GUI.Button(clearButton, "Clear Mesh")) 
			{
				pathScript.ResetPath();
				pathScript.CreatePath(pathScript.pathSmooth, false);
				GUIUtility.ExitGUI();
				
			}
			
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			
			Rect startButton;			
	
			CircularObstacle.randomSeed = EditorGUILayout.IntField("Random seed", CircularObstacle.randomSeed);
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			pathScript.elipticNodes = EditorGUILayout.IntSlider("Number of nodes", pathScript.elipticNodes, 5, 150);
			EditorGUILayout.EndHorizontal();


			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			startButton = EditorGUILayout.BeginHorizontal();
			startButton.x = startButton.width / 2 - 100;
			startButton.width = 200;
			startButton.height = 18;
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
		
			if (GUI.Button(startButton, "Add path")) 
			{
				// agregar un nodo al path.
//http://stackoverflow.com/questions/2280142/calculate-points-to-create-a-curve-or-spline-to-draw-an-elipse
				//x = a cos theta
				//y = b sin theta			
				pathScript.GenerateEasyElipicPath();
				
					GUIUtility.ExitGUI();
			}
			
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			
			startButton = EditorGUILayout.BeginHorizontal();
			startButton.x = startButton.width / 2 - 100;
			startButton.width = 200;
			startButton.height = 18;

			if (GUI.Button(startButton, "Add Random path")) 
			{
				CircularObstacle.randomSeed	= (new System.Random()).Next();		
				pathScript.GenerateEasyElipicPath();
				
					GUIUtility.ExitGUI();
			}			
			
			EditorGUILayout.EndHorizontal();
	
			EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		
	}
	
}
