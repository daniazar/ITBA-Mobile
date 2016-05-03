// Mono Framework
using System;

// Unity Framework
using UnityEngine;

public class Spline
{
	/// <summary>
	/// Steps for debug aproximation
	/// </summary>
	public float splineSteps = 3;
	
	/// <summary>
	/// Total length of the spline
	/// </summary>
    public float lengthTotal = 0;
	
	/// <summary>
	/// Points of the spline
	/// </summary>
	private Transform[] point;
	
	/// <summary>
	/// Lengths
	/// </summary>
	private float[] length;
	
	/// <summary>
	/// Prev count of nodes
	/// </summary>
    private int	prevCount = -1;
    
    /// <summary>
    /// Update of the spline information
    /// </summary>
    public void Update(Transform[] pnts)
    {
        point = pnts;
            
        if (length == null || length.Length != pnts.Length)
            length = new float[pnts.Length];
        
        if (point.Length > 1)
        {
			if (point.Length != prevCount)
            {
				length[0] = 0;
				length[1] = 0;
				length[point.Length - 1] = 0;
				
				for (int i=2; i < point.Length - 1; i++)
					CalculateLengthInSegment(i);
				
				prevCount = point.Length;
			}
			CalculateLength();
		}
    }
    
    /// Function to calculate the length
	public float CalculateLength()
    {
		float lengthStep = 10.0f;
        
		Vector3 prev = GetPoint(2, 0);
		Vector3 q, dif;
		lengthTotal = 0;
        
		for (int i = 2; i < point.Length-1; i++)
        {
			for (int j = 1; j <= lengthStep; j++)
            {
				q = GetPoint(i, j / lengthStep);
				dif = q - prev;
				lengthTotal += dif.magnitude;
				prev = q;
			}
		}
		return lengthTotal;
	}
    
    /// Calculate the length in the specified segment
	float CalculateLengthInSegment(int index)
    {
		if (index < 2) return -1;
		if (index > point.Length - 1) return -2;
		
		float lengthStep = 10.0f;
		Vector3 prev = GetPoint(index, 0);
		Vector3 q, dif;
		length[index] = 0;
		for (int j = 1; j <= lengthStep; j++)
        {
			q = GetPoint(index, j / lengthStep);
			dif = q - prev;
			length[index] += dif.magnitude;
			prev = q;
		}
		return length[index];
	}
    
	/// <summary>
	/// These variables could be locals from GetPoint. They are
	/// here just to optimize.
	/// </summary>
    float calc1, calc2, calc3, calc4, px, py, pz;

	/// <summary>
	/// Evaluate a point on the B spline
    /// Reusable local variables (optimization)
	/// </summary>
	public Vector3 GetPoint(int i, float t)
    {
        if (point == null || point.Length < 4) return Vector3.zero;

		calc1 = (((-t+3)*t-3)*t+1)/6;
		calc2 = (((3*t-6)*t)*t+4)/6;
		calc3 = (((-3*t+3)*t+3)*t+1)/6;
		calc4 = (t*t*t)/6;
		
		px  = calc1 * point[i-2].position.x;
		py  = calc1 * point[i-2].position.y;
		pz  = calc1 * point[i-2].position.z;

		px += calc2 * point[i-1].position.x;
		py += calc2 * point[i-1].position.y;
		pz += calc2 * point[i-1].position.z;
		
		px += calc3 * point[i].position.x;
		py += calc3 * point[i].position.y;
		pz += calc3 * point[i].position.z;
		
		px += calc4 * point[i+1].position.x;
		py += calc4 * point[i+1].position.y;
		pz += calc4 * point[i+1].position.z;
        
		return new Vector3(px, py, pz);
	}
    
    /// <summary>
    /// Returns a point inside the path at a distance from the origin.
    /// </param>
	public Vector3 GetPointAtDistance(float distance, out int val)
	{
		int index = 2;
		int indexCheck = -1;
		bool done = false;
		float acumDist = length[index];
		
		val = -1;
		if (distance < 0)
		{
			done = true;
			val = 0;
			return GetPoint(2, 0);
		}
		
		while(!done)
		{
			if (index >= point.Length-1)
			{
				done = true;
			}
			else
			{
				if (distance>acumDist)
				{
					index++;
					acumDist+=length[index];
				}
				else
				{
					done = true;
					indexCheck = index;
				}
			}
		}
		
		if (indexCheck!=-1)
		{
			float relDist = length[indexCheck] - (acumDist - distance);
			float proportional = relDist/length[indexCheck];
			return GetPoint(indexCheck, proportional);
		}
		else
		{
			val = 1;
			return GetPoint(point.Length-2, 1);
		}
	}
    
    
}