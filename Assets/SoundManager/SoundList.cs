// Mono Framework
using System;
using System.Collections;
using System.Xml;
using System.IO;

// Unity Engine
using UnityEngine;


public abstract class SoundList : MonoBehaviour
{
	/// <summary>
	/// All the audioClips, filled in the Unity editor by a programmer or level designer
	/// </summary>
    public AudioClip[] soundClips;				
	
	/// <summary>
	/// SoundProps with all the info about the clip
	/// </summary>
    private SoundProp[] _sounds;
	
	/// <summary>
	/// Start
	/// </summary>
	protected void Start()
	{
        _sounds = GetSoundProps();

        if (_sounds == null)
        {
            Debug.Log("[ERROR] You should define a SoundProp vector at SoundList... class.");
            return;
        }

		// Relate the array entries with the specified audioClip
		for (int i=0; i<soundClips.Length; i++)
		{
			// Some sounds could be null (if they are not used in the level
			// but the have to keep the sound index for other level)
			if (soundClips[i] != null)
			{
				SoundProp sp = GetSoundPropByName(soundClips[i].name);
			
				if (sp != null)
				{
					sp.audioClip = soundClips[i];
				}
				else
				{
					Debug.LogWarning(String.Format("Cannot find the sound {0} on the array list of sounds.", soundClips[i].name));
				}
			}
		}
	}
	
	/// <summary>
	/// Returns the SoundProp using its name
	/// </summary>
	public SoundProp GetSoundPropByName(string name)
	{
        for (int i = 0; i < _sounds.Length; i++)
		{
            if (String.Compare(name, _sounds[i].name, true) == 0)
			{
                return _sounds[i];
			}
		}

		return null;
	}
	
	
	/// <summary>
	/// Returns the SoundProp using its Id
	/// </summary>
	public SoundProp GetSoundProp(int sndId)
	{
		if (_sounds == null)
		{
			Debug.Log("ERROR. _sounds is null");
			return null;
		}
		
		if (sndId < _sounds.Length)
        		return _sounds[sndId];
		else
			return null;
	}
	
	/// <summary>
	/// Returns all the props
	/// </summary>
    protected abstract SoundProp[] GetSoundProps();
}



