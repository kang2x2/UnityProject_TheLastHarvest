using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    ParticleSystem[] _effects;

    private void Start()
    {
        _effects = GetComponentsInChildren<ParticleSystem>();
    }

    private void LateUpdate()
    {
        if(Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            foreach (ParticleSystem effect in _effects)
            {
                if (effect != null)
                {
                    effect.Pause();
                }
            }
            return;
        }
        else
        {
            foreach (ParticleSystem effect in _effects)
            {
                if (effect != null && effect.isPaused == true)
                {
                    effect.Play();
                }
            }
        }

        bool isDestroy = true;
        foreach(ParticleSystem effect in _effects)
        {
            if(effect != null && effect.isPlaying == true)
            {
                isDestroy = false;
                break;
            }
        }

        if(isDestroy == true)
        {
            Managers.ResourceManager.Destroy(gameObject);
        }
    }
}
