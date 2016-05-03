// Mono Framework
using System;
using System.Collections;

// Unity Framework
using UnityEngine;

public enum SndIdGame : int
{
    SND_CLICK 			= 0,
    SND_CLICK_ERROR 	= 1,
	SND_CANCEL 			= 2,
	SND_CONFIRM 		= 3,
}

/// <summary>
/// SoundList of the Game
/// </summary>
public class SoundListGame : SoundList
{
    SoundProp[] sounds = {
		new SoundProp((int) SndIdGame.SND_CLICK,       "ButtonClick",         1, 100),
		new SoundProp((int) SndIdGame.SND_CLICK_ERROR, "Crush",    1, 100),
		new SoundProp((int) SndIdGame.SND_CANCEL,      "Cancel-Back",         1, 100),
		new SoundProp((int) SndIdGame.SND_CONFIRM,     "Confirm",             1, 100),
	};

    new void Start()
    {
        base.Start();
    }

    protected override SoundProp[] GetSoundProps()
    {
        return sounds;
    }
}

