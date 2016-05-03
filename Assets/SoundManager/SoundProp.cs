using UnityEngine;
using System.Collections;

public class SoundProp
{
	private const int DEF_VOLUME = 100;
	private const SndType DEF_SND_TYPE = SndType.SND_FX;
	private const bool DEF_LOOP_VALUE = false;
	
	/// <summary>
	/// Id of the prop
	/// </summary>
    public int id;
	
	/// <summary>
	/// Sound or music
	/// </summary>
    public SndType type;
	
	/// <summary>
	/// Name of the clip
	/// </summary>
    public string name;
	
	/// <summary>
	/// Priority.
	/// </summary>
    public int priority;
	
	/// <summary>
	/// Related audio clip
	/// </summary>
    public AudioClip audioClip;
	
	/// <summary>
	/// Looped?
	/// </summary>
    public bool loop;
	
	/// <summary>
	/// The volume of the clip.
	/// Possible values: 0 to 100 -> Converted to 0.0 to 1.0f when it's used
	/// </summary>
    public int volume;      


    public SoundProp(int pId, string pName, int pPriority)
    {
        id = pId;
        name = pName;
        priority = pPriority;
        type = DEF_SND_TYPE;
        loop = DEF_LOOP_VALUE;
        volume = DEF_VOLUME;
    }

    public SoundProp(int pId, string pName, int pPriority, int vol)
    {
        id = pId;
        name = pName;
        priority = pPriority;
        type = DEF_SND_TYPE;
        loop = DEF_LOOP_VALUE;
        volume = vol;
    }

    public SoundProp(int pId, string pName, int pPriority, bool playInLoop, SndType pType)
    {
        id = pId;
        name = pName;
        priority = pPriority;
        type = pType;
        loop = playInLoop;
        volume = DEF_VOLUME;
    }

    public SoundProp(int pId, string pName, int pPriority, bool playInLoop, SndType pType, int vol)
    {
        id = pId;
        name = pName;
        priority = pPriority;
        type = pType;
        loop = playInLoop;
        volume = vol;
    }

}
