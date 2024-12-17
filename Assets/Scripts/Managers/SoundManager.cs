using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    public AudioSource BGMSource { get; set; }
    Dictionary<string, AudioClip> _bgmClips = new Dictionary<string, AudioClip>();

    Dictionary<string, AudioSource> _sfxSources = new Dictionary<string, AudioSource>();

    public float BGMVolum { get; set; }
    public float SFXVolum { get; set; }

    public void Init()
    {
        BGMVolum = Managers.DataManager.Sound.Data.bgmVolum;
        SFXVolum = Managers.DataManager.Sound.Data.sfxVolum;

        GameObject soundRoot = GameObject.Find("@SoundRoot");
        if(soundRoot == null)
        {
            soundRoot = new GameObject { name = "@SoundRoot" };
        }
        GameObject.DontDestroyOnLoad(soundRoot);

        GameObject _bgmObj = new GameObject { name = "BGMObject" };
        _bgmObj.transform.parent = soundRoot.transform;
        BGMSource = _bgmObj.AddComponent<AudioSource>();
        BGMSource.loop = true;

       GameObject _sfxObj = new GameObject { name = "SFXObjects" };
       _sfxObj.transform.parent = soundRoot.transform;

        GetBGMClip("BGMs/TitleBGM");
        GetBGMClip("BGMs/FieldBGM");
        GetBGMClip("BGMs/CaveBGM");
    }

    public void PlayBGM(string name)
    {
        string path = $"Sounds/{name}";
        AudioClip bgmClip = GetBGMClip(name);
        if (bgmClip == null)
        {
            Debug.Log("Fail Fild BGM..");
            return;
        }

        BGMSource.volume = BGMVolum;
        BGMSource.clip = bgmClip;
        BGMSource.Play();
    }

    public void PlaySFX(string name)
    {
        AudioClip sfxClip = GetSFXClip(name);

        if (sfxClip == null)
        {
            Debug.Log("Fail Fild SFX..");
            return;
        }

        if(_sfxSources[name].isPlaying == true)
        {
            _sfxSources[name].Stop();
        }

        _sfxSources[name].volume = SFXVolum;
        _sfxSources[name].PlayOneShot(sfxClip);
    }

    private AudioClip GetBGMClip(string name)
    {
        AudioClip bgmClip;
        string path = $"Sounds/{name}";

        if (_bgmClips.TryGetValue(name, out bgmClip) == false)
        {
            bgmClip = Resources.Load<AudioClip>(path);
            _bgmClips.Add(name, bgmClip);
        }

        if (bgmClip == null)
        {
            Debug.Log("Fail GetBGMClip...");
            return null;
        }

        return bgmClip;
    }

    private AudioClip GetSFXClip(string name)
    {
        AudioSource sfxSource;

        if(_sfxSources.TryGetValue(name, out sfxSource) == false)
        {
            string path = $"Sounds/{name}";

            GameObject _sfxObj = new GameObject { name = "SFXObject" };
            _sfxObj.transform.parent = GameObject.Find("@SoundRoot").transform.Find("SFXObjects");
            if(_sfxObj.transform.parent == null)
            {
                Debug.Log("Fail Find SFXObjects");
                return null;
            }
            sfxSource = _sfxObj.AddComponent<AudioSource>();
            _sfxSources.Add(name, sfxSource);
            _sfxSources[name].clip = Resources.Load<AudioClip>(path);
        }

        return _sfxSources[name].clip;
    }

    public void Clear()
    {
        BGMSource.Stop();
        BGMSource.clip = null;

        foreach(AudioSource sfx in _sfxSources.Values)
        {
            sfx.Stop();
            sfx.clip = null;
        }
        _sfxSources.Clear();
    }
}
