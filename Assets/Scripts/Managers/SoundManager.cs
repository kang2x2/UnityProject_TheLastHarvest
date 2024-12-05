using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource _bgmSource;
    AudioSource _sfxSource;
    Dictionary<string, AudioClip> _bgmClips = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> _sfxClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject soundRoot = GameObject.Find("@SoundRoot");
        if(soundRoot == null)
        {
            soundRoot = new GameObject { name = "@SoundRoot" };
        }
        GameObject.DontDestroyOnLoad(soundRoot);

        GameObject _bgmObj = new GameObject { name = "BGMObject" };
        _bgmObj.transform.parent = soundRoot.transform;
        _bgmSource = _bgmObj.AddComponent<AudioSource>();
        _bgmSource.loop = true;

        GameObject _sfxObj = new GameObject { name = "SFXObject" };
        _sfxObj.transform.parent = soundRoot.transform;
        _sfxSource = _sfxObj.AddComponent<AudioSource>();

        GetBGMClip("BGM");
    }

    public void PlayBGM(string name, float volum = 1.0f)
    {
        string path = $"Sounds/{name}";
        AudioClip bgmClip = GetBGMClip(name);
        if (bgmClip == null)
        {
            Debug.Log("Fail Fild BGM..");
            return;
        }

        _bgmSource.volume = volum;
        _bgmSource.clip = bgmClip;
        _bgmSource.Play();
    }

    public void PlaySFX(string name, float volum = 1.0f)
    {
        AudioClip sfxClip = GetSFXClip(name);

        if (sfxClip == null)
        {
            Debug.Log("Fail Fild SFX..");
            return;
        }

        _sfxSource.volume = volum;
        _sfxSource.PlayOneShot(sfxClip);
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
        AudioClip sfxClip;
        string path = $"Sounds/{name}";

        if(_sfxClips.TryGetValue(name, out sfxClip) == false)
        {
            sfxClip = Resources.Load<AudioClip>(path);
            _sfxClips.Add(name, sfxClip);
        }

        if(sfxClip == null)
        {
            Debug.Log("Fail GetSFXClip...");
            return null;
        }

        return sfxClip;
    }

    public void Clear()
    {
        _bgmSource.Stop();
        _bgmSource.clip = null;

        _sfxSource.clip = null;
        _sfxClips.Clear();
    }
}
