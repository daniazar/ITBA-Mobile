// Mono Framework
using System;

// Unity Framework
using UnityEngine;

public class Vector3Util
{
    /// <summary>
    /// Receive two points and return a normalized vector that look from the
    /// origin point to the destination point
    /// </summary>
    /// <param name="ori">The origin point</param>
    /// <param name="dst">The destination point</param>
    /// <returns>A normalized vector that look to the destination point</returns>
	public static Vector3 GetLookAt(Vector3 ori, Vector3 dst)
	{
        Vector3 v3 = dst - ori;
        v3.Normalize();
        return v3;
	}

    /// <summary>
    /// Receive a point, a direction vector and a distance. Returns a point in
    /// the direction of ori with the specified distance.
    /// </summary>
    /// <param name="ori">Origin Point</param>
    /// <param name="dir">Direction (Normalized Vector)</param>
    /// /// <param name="distance">Distance from origin point</param>
    /// <returns>A point over the specified vector with the specified distance</returns>
    public static Vector3 GetPoint(Vector3 ori, Vector3 dir, float distance)
    {
        Ray r = new Ray(ori, dir);
        return r.GetPoint(distance);
    }

	public static float DistanceXZ(Vector3 v1, Vector3 v2)
	{
		return Mathf.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.z - v2.z) * (v1.z - v2.z));
	}

    public static Vector3 Normalize(Vector3 v)
    {
        v.Normalize();
        return v;
    }
    
    public static Vector3 RotateX(Vector3 v, float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector3(v.x,
                           v.y * Mathf.Cos(angleRad) + v.z * Mathf.Sin(angleRad),
                           v.y * Mathf.Sin(angleRad) * -1 + v.z * Mathf.Cos(angleRad));
    }
    
    public static Vector3 RotateY(Vector3 v, float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector3(v.x * Mathf.Cos(angleRad) + v.z * Mathf.Sin(angleRad) * -1,
                           v.y,
                           v.x * Mathf.Sin(angleRad) + v.z * Mathf.Cos(angleRad));
    }
    
    public static Vector3 RotateZ(Vector3 v, float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector3(v.x * Mathf.Cos(angleRad) + v.y * Mathf.Sin(angleRad),
                           v.x * Mathf.Sin(angleRad) * -1 + v.y * Mathf.Cos(angleRad),
                           v.z);
    }
    
    public static Vector3 RotateXRad(Vector3 v, float angle)
    {
        return new Vector3(v.x,
                           v.y * Mathf.Cos(angle) + v.z * Mathf.Sin(angle),
                           v.y * Mathf.Sin(angle) * -1 + v.z * Mathf.Cos(angle));
    }
    
    public static Vector3 RotateYRad(Vector3 v, float angle)
    {
        return new Vector3(v.x * Mathf.Cos(angle) + v.z * Mathf.Sin(angle) * -1,
                           v.y,
                           v.x * Mathf.Sin(angle) + v.z * Mathf.Cos(angle));
    }
    
    public static Vector3 RotateZRad(Vector3 v, float angle)
    {
        return new Vector3(v.x * Mathf.Cos(angle) + v.y * Mathf.Sin(angle),
                           v.x * Mathf.Sin(angle) * -1 + v.y * Mathf.Cos(angle),
                           v.z);
    }
	
	/// <summary>
	/// Applies a random of "err" to each component
	/// err should be between 0.0f and 1.0f
	/// </summary>
	public static Vector3 ApplyRandonOnAngleY(Vector3 pos, int maxErrorDegrees, float err)
	{
		float maxAngle = maxErrorDegrees * err;
		float errAngle = PseudoRandom.GetNextSingle() * maxAngle;
		return Vector3Util.RotateY(pos, errAngle - (maxAngle / 2.0f));
	}
	
}
