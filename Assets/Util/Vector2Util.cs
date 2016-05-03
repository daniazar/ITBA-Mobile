// Mono Framework
using System;

// Unity Framework
using UnityEngine;

public class Vector2Util
{
    public static Vector2 Normalize(Vector2 v)
    {
        float mod = Mathf.Sqrt((v.x * v.x) + (v.y * v.y));
        return new Vector2(v.x / mod, v.y / mod);
    }
	
	/// <summary>
	/// Returns the angle (in DEG) between the two specified vectors.
	/// </summary>
	public static float GetAngleBetweenVectors(Vector2 v1, Vector2 v2)
	{
		return Mathf.Acos(Vector2.Dot(v1, v2)) * Mathf.Rad2Deg;
	}
}