// Mono Framework
using System;

// Unity Framework
using UnityEngine;

public class DebugEx
{
	public static void DrawRect(Vector3 center, Vector3 normal, float width, float height, Color c)
	{
		Vector3 p0 = Vector3Util.RotateX(normal, 90);
		Vector3 p1 = Vector3Util.RotateZ(normal, 90);
		Vector3 p2 = Vector3Util.RotateX(normal, -90);
		Vector3 p3 = Vector3Util.RotateZ(normal, -90);
		
		Debug.DrawLine(p0, p1, c);
		Debug.DrawLine(p1, p2, c);
		Debug.DrawLine(p2, p3, c);
		Debug.DrawLine(p3, p0, c);
	}
	
	public static void DrawRectXZ(Vector3 center, float width, float height, Color c)
	{
		Vector3 p0 = new Vector3(center.x - (width / 2.0f), center.y, center.z - (height / 2.0f));
		Vector3 p1 = new Vector3(center.x - (width / 2.0f), center.y, center.z + (height / 2.0f));
		Vector3 p2 = new Vector3(center.x + (width / 2.0f), center.y, center.z + (height / 2.0f));
		Vector3 p3 = new Vector3(center.x + (width / 2.0f), center.y, center.z - (height / 2.0f));
		
		Debug.DrawLine(p0, p1, c);
		Debug.DrawLine(p1, p2, c);
		Debug.DrawLine(p2, p3, c);
		Debug.DrawLine(p3, p0, c);
	}
	
	public static void DrawRectCrossXZ(Vector3 center, float width, float height, Color c)
	{
		Vector3 p0 = new Vector3(center.x - (width / 2.0f), center.y, center.z - (height / 2.0f));
		Vector3 p1 = new Vector3(center.x - (width / 2.0f), center.y, center.z + (height / 2.0f));
		Vector3 p2 = new Vector3(center.x + (width / 2.0f), center.y, center.z + (height / 2.0f));
		Vector3 p3 = new Vector3(center.x + (width / 2.0f), center.y, center.z - (height / 2.0f));
		
		Debug.DrawLine(p0, p1, c);
		Debug.DrawLine(p1, p2, c);
		Debug.DrawLine(p2, p3, c);
		Debug.DrawLine(p3, p0, c);
		Debug.DrawLine(p0, p2, c);
		Debug.DrawLine(p1, p3, c);
	}
	
	
}
