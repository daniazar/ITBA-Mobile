// Mono Framework
using System;

// Unity Framework
using UnityEngine;

public class PseudoRandom
{
    //private static System.Random _ran;
	
	public static float GetNextSingle()
	{
		//if (_ran == null)
			//_ran = new Random();
		
		//return (float) _ran.NextDouble();

        return UnityEngine.Random.value;
	}

    public static int GetNextInt(int maxBound)
    {
        //if (_ran == null)
        //    _ran = new Random();

        //return _ran.Next(maxBound);

        return (int) Mathf.Floor(UnityEngine.Random.value * maxBound);
    }

    public static int GetNextInt(int minBound, int maxBound)
    {
        //if (_ran == null)
        //    _ran = new Random();

        //return _ran.Next(minBound, maxBound);

        return (int) UnityEngine.Random.Range(minBound, maxBound);
    }

}
