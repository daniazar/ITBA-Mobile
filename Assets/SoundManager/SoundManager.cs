// Mono Framework
using System;
using System.Collections;

// Unity Framework
using UnityEngine;

/// <summary>
/// A SoundManager.
/// 
/// Features:
/// . Background, Music and sounds channels.
/// . A priority system for the sound triggering.
/// . Fade In and Fade Out of music and sounds (event oriented)
/// . Fade out of all sounds (event oriented).
/// . 
/// 
/// Requires:
/// 
/// A GameObject with this MonoBehaviour, SoundList object and a Dont Destroy on Unload if necessary.
/// </summary>
public class SoundManager : MonoBehaviour
{
	/// <summary>
	/// Number of Audio Sources to create
	/// </summary>
    public const int CHANNELS = 7;

    /// <summary>
    /// Reference to the single instance
    /// </summary>
    private static SoundManager instance;
	
	/// <summary>
	/// Array of audio sources reserved for fx
	/// </summary>
    private static AudioSource[] fx;
	
	/// <summary>
	/// Audio source used for music
	/// </summary>
    private static AudioSource music;
	
	/// <summary>
	/// Audio source used for background sound
	/// </summary>
    private static AudioSource background;
	
	/// <summary>
	/// 
	/// </summary>
    private static SoundList soundList;
	
	/// <summary>
	/// Callback
	/// </summary>
    private static SoundManagerCallback fadeOutAllSoundsCallback;
	
	/// <summary>
	/// Fade all sounds?
	/// </summary>
    private static bool fadingAllSounds;
	
	/// <summary>
	/// Array of fades
	/// </summary>
    private static ArrayList asFades = new ArrayList();
	
	/// <summary>
	/// The _current background snd property.
	/// </summary>
    private static SoundProp currentBackgroundSndProp = null;
	
	/// <summary>
	/// The _current music snd property.
	/// </summary>
    private static SoundProp currentMusicSndProp = null;
    
    void Awake()
    {
        instance = this;
        soundList = GetComponent(typeof(SoundList)) as SoundList;
    }

    void Start()
    {
        // Add an specific audiosource for the music
        music = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        music.playOnAwake = false;

        // Add an specific audiosource for the background main sound
        background = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        background.playOnAwake = false;

        // Add many audiosources for multiple sounds
        fx = new AudioSource[CHANNELS];
        for (int i = 0; i < CHANNELS; i++)
        {
            fx[i] = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            fx[i].playOnAwake = false;
        }

        MasterVolume = SoundConfig.masterVolume;
        BackgroundVolume = SoundConfig.backgroundVolume;
        MusicVolume = SoundConfig.musicVolume;
        FxVolume = SoundConfig.fxVolume;

        music.volume = SoundConfig.musicVolume * MasterVolume;
        background.volume = SoundConfig.backgroundVolume * MasterVolume;

        fadingAllSounds = false;
    }

    void OnEnable()
    {
        instance = this;
    }

    void OnDisable()
    {
        // Stop all sounds now
        StopAll();
    }

    void Update()
    {
        if (soundList == null)
            soundList = gameObject.GetComponent(typeof(SoundList)) as SoundList;

        // Update the position of the music and the background main sound
        if (Camera.mainCamera != null)
        {
            music.transform.position = Camera.mainCamera.transform.position;
            background.transform.position = Camera.mainCamera.transform.position;
        }

        // Change the volume of the faded sounds
        foreach (FadeAudioSource fas in asFades)
        {
            fas.accumTime = Time.realtimeSinceStartup - fas.initialTime;

            // Increase the volume
            float totalDelta = fas.targetVolume - fas.initialVolume;
            float deltaVolume = totalDelta * (fas.accumTime / fas.fadeInSecs);

            fas.audioSrc.volume = fas.initialVolume + deltaVolume;

            //Debug.Log(String.Format("totalDelta: {0} deltaVol: {1} vol: {2}", totalDelta, deltaVolume, fas.audioSrc.volume));

            if (fas.accumTime >= fas.fadeInSecs)
            {
                fas.audioSrc.volume = fas.targetVolume;

                // Call the event
                if (fas.fnCb != null)
                    fas.fnCb();

                // Remove the fading
                asFades.Remove(fas);

                // Return on every remove
                return;
            }
        }

        if (fadingAllSounds)
        {
            if (asFades.Count == 0)
            {
                if (fadeOutAllSoundsCallback != null)
                {
                    fadeOutAllSoundsCallback();
                    fadeOutAllSoundsCallback = null;
                }
            }
        }
    }

    public static float MasterVolume
    {
        set
        {
            SoundConfig.masterVolume = value;
        }
        get
        {
            return SoundConfig.masterVolume;
        }
    }

    public static float BackgroundVolume
    {
        set
        {
            SoundConfig.backgroundVolume = value;
            background.volume = value * MasterVolume * (currentBackgroundSndProp != null ? (currentBackgroundSndProp.volume / 100.0f) : 1);
        }
        get
        {
            return SoundConfig.backgroundVolume;
        }
    }

    public static float MusicVolume
    {
        set
        {
            SoundConfig.musicVolume = value;
            music.volume = value * MasterVolume * (currentMusicSndProp != null ? (currentMusicSndProp.volume / 100.0f) : 1);
        }
        get
        {
            return SoundConfig.musicVolume;
        }
    }

    public static float FxVolume
    {
        set
        {
            SoundConfig.fxVolume = value;

            for (int i = 0; i < CHANNELS; i++)
            {
                fx[i].volume = value * MasterVolume;
            }
        }
        get
        {
            return SoundConfig.fxVolume;
        }
    }

    public static void PauseAll(bool pauseThem)
    {
        if (pauseThem)
        {
            // Pause the background, music and all sounds
            background.Pause();
            music.Pause();
            for (int i = 0; i < CHANNELS; i++)
                fx[i].Pause();

            // Finish all the fades
            foreach (FadeAudioSource fas in asFades)
                fas.audioSrc.volume = fas.targetVolume;

            asFades.Clear();
        }
        else
        {
            // Resume the background, music and all sounds if there were playing in the pause
            
            if (background.time > 0)
                background.Play();
            
            if (music.time > 0)
                music.Play();
            
            for (int i = 0; i < CHANNELS; i++)
            {
                if (fx[i].time > 0)
                    fx[i].Play();
            }
           

        }
    }

    /// <summary>
    /// Play the background sound
    /// </summary>
    /// <param name="int"></param>
    public static void PlayBackground(int sndId)
    {		
        SoundProp sp = soundList.GetSoundProp(sndId);

        if (sp != null)
        {
            currentBackgroundSndProp = sp;

            // Set the position of the current camera in order to play the sound balanced
            if (Camera.mainCamera != null)
                background.transform.position = Camera.mainCamera.transform.position;

            background.clip = sp.audioClip;
            background.loop = sp.loop;
            background.volume = BackgroundVolume * MasterVolume * (sp.volume / 100.0f);
            background.Play();
        }
    }

    /// <summary>
    /// Stop the background sound
    /// </summary>
    public static void StopBackground()
    {
        background.Stop();
    }

    /// <summary>
    /// Play the music
    /// </summary>
    /// <param name="int"></param>
    public static void PlayMusic(int sndId)
    {
        PlayMusic(sndId, false);
    }

    public static void PlayMusic(int sndId, bool fadeOutCurrent)
    {
        if (instance == null) return;

        SoundProp sp = soundList.GetSoundProp(sndId);

        if (sp != null)
        {

            currentMusicSndProp = sp;

            if (fadeOutCurrent && music.isPlaying)
            {
                FadeOutMusic(1, playMusicAfterFadeOut);
            }
            else
            {
                playMusicAfterFadeOut();
            }
        }
    }

    public static bool IsPlayingMusic()
    {
        return music.isPlaying;
    }

    private static void playMusicAfterFadeOut()
    {
        // Set the position of the current camera in order to play the sound balanced
        if (Camera.mainCamera != null)
            music.transform.position = Camera.mainCamera.transform.position;

        music.clip = currentMusicSndProp.audioClip;
        music.loop = currentMusicSndProp.loop;

        music.volume = MusicVolume * MasterVolume * (currentMusicSndProp.volume / 100.0f);
        music.Play();
    }

    /// <summary>
    /// Stop the music
    /// </summary>
    public static void StopMusic()
    {
        music.Stop();
    }

    public static void FadeOutMusic(float inSecs, SoundManagerCallback cbfn)
    {
	
        FadeAudioSource fas = new FadeAudioSource();

        fas.initialTime = Time.realtimeSinceStartup;
        fas.accumTime = 0;
        fas.initialVolume = music.volume;
        fas.targetVolume = 0;
        fas.audioSrc = music;
        fas.fadeInSecs = inSecs;
        fas.fnCb += cbfn;

        asFades.Add(fas);
    }

    public static void FadeInMusic(int sndId, float inSecs, SoundManagerCallback cbfn)
    {
        SoundProp sp = soundList.GetSoundProp(sndId);

        if (sp != null)
        {
            currentMusicSndProp = sp;

            // Set the position of the current camera in order to play the sound balanced
            if (Camera.mainCamera != null)
                music.transform.position = Camera.mainCamera.transform.position;

            music.clip = sp.audioClip;
            music.loop = sp.loop;
            music.volume = 0;
            music.Play();

            FadeAudioSource fas = new FadeAudioSource();

            fas.initialTime = Time.realtimeSinceStartup;
            fas.accumTime = 0;
            fas.initialVolume = 0;
            fas.targetVolume = MusicVolume * MasterVolume * (sp.volume / 100.0f);
            fas.audioSrc = music;
            fas.fadeInSecs = inSecs;

            

            if (cbfn != null)
                fas.fnCb += cbfn;
            else
                fas.fnCb = null;

            asFades.Add(fas);

        }
    }

    public static void FadeOutBackground(float inSecs, SoundManagerCallback cbfn)
    {
        FadeAudioSource fas = new FadeAudioSource();

        fas.initialTime = Time.realtimeSinceStartup;
        fas.accumTime = 0;
        fas.initialVolume = background.volume;
        fas.targetVolume = 0;
        fas.audioSrc = background;
        fas.fadeInSecs = inSecs;
        fas.fnCb += cbfn;

        asFades.Add(fas);
    }
	
	/// <summary>
	/// Play a sound without a specific position
	/// </summary>
    public static int PlaySound(int sndId)
    {
        if (Camera.mainCamera != null)
        {
            return PlaySound(Camera.mainCamera.transform.position, sndId);
        }
        else
		{
            return -1;
		}
    }

    /// <summary>
    /// Play an specific sound
    /// </summary>
    public static int PlaySound(Vector3 pos, int sndId)
    {
        if (fx == null) return -1;
        SoundProp sp = soundList.GetSoundProp(sndId);
		
        if (sp != null)
        {
            // The specified sound should be marked as FX (the default value)
            if (sp.type == SndType.SND_FX)
            {
                int channeldIdx = getChannelIdx(sp);

                if (channeldIdx != -1)
                {
                    playThisSoundOnSource(channeldIdx, sp, pos);
                    return channeldIdx;
                }
                else
                    Debug.Log("All audiosource are busy. Cannot play sound: " + sp.name);
            }
            else
                Debug.Log(String.Format("Trying to play a sound that is not a FX ({0})", sp.name));
        }
        return -1;
    }

    private static void playThisSoundOnSource(int idx, SoundProp sp, Vector3 pos)
    {
        fx[idx].Stop();
        fx[idx].clip = sp.audioClip;
        fx[idx].loop = sp.loop;
        fx[idx].volume = FxVolume * MasterVolume * (sp.volume / 100.0f);

        fx[idx].transform.position = pos;
        fx[idx].Play();
    }

    /// <summary>
    /// Returns a channeld idx to play a sound.
    /// Could be:
    /// 1. An empty channel (not yet used)
    /// 2. An IDLE channel
    /// 3. A busy channel but with less priority
    /// 4. A busy channel with the same priority
    /// 
    /// If there isn't a channel that satisfy these conditions, returns -1
    /// 
    /// </summary>
    /// <returns></returns>
    private static int getChannelIdx(SoundProp sp)
    {
        for (int i = 0; i < CHANNELS; i++)
        {
            if (fx[i].clip != null)
            {
                if (!fx[i].isPlaying)
                {
                    // Found a audiosource that is not currently being played

                    if (fx[i].clip != sp.audioClip)
                        return i;
                }
            }
            else
            {
                return i;
            }
        }

        // No audiosource idle. Find a busy audiosource with less priority than the new one
        for (int i = 0; i < CHANNELS; i++)
        {
            SoundProp prop = soundList.GetSoundPropByName(fx[i].clip.name);
            if (sp.priority > prop.priority)
                return i;
        }

        // Try something with the same priority
        for (int i = 0; i < CHANNELS; i++)
        {
            SoundProp prop = soundList.GetSoundPropByName(fx[i].clip.name);
            if (sp.priority == prop.priority)
                return i;
        }

        // Cannot find a suitable channel
        return -1;
    }

    /// <summary>
    /// Stop an specific sound immediatelly
    /// </summary>
    /// <param name="channelIdx"></param>
    public static void StopSound(int channelIdx)
    {
        if (channelIdx != -1 && channelIdx < CHANNELS)
        {
            if (fx[channelIdx] != null && fx[channelIdx].clip != null)
            {
                fx[channelIdx].Stop();
            }
        }
    }

    /// <summary>
    /// Fade out an specific sound.
    /// Do not call other function of the sound manager that requires a callback function before the first
    /// one finishes and call the callback.
    /// </summary>
    /// <param name="channelIdx"></param>
    /// <param name="fncb"></param>
    public static void FadeOutSound(int channelIdx, float inSecs, SoundManagerCallback cbfn)
    {
        FadeAudioSource fas = new FadeAudioSource();

        fas.initialTime = Time.realtimeSinceStartup;
        fas.accumTime = 0;
        fas.initialVolume = fx[channelIdx].volume;
        fas.targetVolume = 0;
        fas.audioSrc = fx[channelIdx];
        fas.fadeInSecs = inSecs;
        fas.fnCb += cbfn;

        asFades.Add(fas);
    }

    public static void FadeInSound(int sndId, float inSecs, SoundManagerCallback cbfn)
    {
        SoundProp sp = soundList.GetSoundProp(sndId);

        if (sp != null)
        {

            int channeldIdx = getChannelIdx(sp);

            if (channeldIdx != -1)
            {
                // Set the position of the current camera in order to play the sound balanced
                if (Camera.mainCamera != null)
                    fx[channeldIdx].transform.position = Camera.mainCamera.transform.position;

                fx[channeldIdx].clip = sp.audioClip;
                fx[channeldIdx].loop = sp.loop;
                fx[channeldIdx].volume = 0;
                fx[channeldIdx].Play();

                FadeAudioSource fas = new FadeAudioSource();

                fas.initialTime = Time.realtimeSinceStartup;
                fas.accumTime = 0;
                fas.initialVolume = 0;
                fas.targetVolume = FxVolume * MasterVolume * (sp.volume / 100.0f);
                fas.audioSrc = fx[channeldIdx];
                fas.fadeInSecs = inSecs;

                if (cbfn != null)
                    fas.fnCb += cbfn;
                else
                    fas.fnCb = null;

                asFades.Add(fas);
            }

        }
    }

    /// <summary>
    /// Stops all the sounds immediatelly
    /// </summary>
    public static void StopAll()
    {
        if (background)
            background.Stop();

        if (music)
            music.Stop();

        for (int i = 0; i < CHANNELS; i++)
        {
            if (fx != null)
                if (fx[i] != null)
                    fx[i].Stop();
        }
    }

    /// <summary>
    /// Fade out all the sounds that are currently being played.
    /// </summary>
    /// <param name="fncb"></param>
    public static void FadeOutAll(float inSecs, SoundManagerCallback cbfn)
    {

        // Finish all the fades
        foreach (FadeAudioSource fas in asFades)
            fas.audioSrc.volume = fas.targetVolume;

        asFades.Clear();

        FadeOutMusic(inSecs, null);

        FadeOutBackground(inSecs, null);

        for (int i=0; i<CHANNELS; i++)
        {
            if (fx[i].isPlaying)
                FadeOutSound(i, inSecs, null);
        }

        fadingAllSounds = true;
        fadeOutAllSoundsCallback += cbfn;
    }

    /// <summary>
    /// Return the channel
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static AudioSource GetChannelById(int id)
    {
        return fx[id];
    }
}

