// Mono Framework
using System;
using System.Collections;

// Unity Framework
using UnityEngine;

public delegate void SoundManagerCallback();

public class FadeAudioSource
{
	/// <summary>
	/// Target volume to go
	/// </summary>
    public float targetVolume;
	
	/// <summary>
	/// The initial volume.
	/// </summary>
    public float initialVolume;
	
	/// <summary>
	/// Complete the fade in the specified seconds
	/// </summary>
    public float fadeInSecs;
	
	/// <summary>
	/// The initial time.
	/// </summary>
    public float initialTime;
	
	/// <summary>
	/// The accum time.
	/// </summary>
    public float accumTime;
	
	/// <summary>
	/// The affected audio source.
	/// </summary>
    public AudioSource audioSrc;
	
	/// <summary>
	/// Callback function
	/// </summary>
    public SoundManagerCallback fnCb;
}
