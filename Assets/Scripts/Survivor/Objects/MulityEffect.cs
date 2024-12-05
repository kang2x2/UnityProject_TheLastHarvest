using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulityEffect : MonoBehaviour
{
    ParticleSystem[] _effects;

    private void Start()
    {
        _effects = GetComponentsInChildren<ParticleSystem>();
    }

    private void LateUpdate()
    {
        bool isDestroy = true;
        foreach(ParticleSystem effect in _effects)
        {
            if(effect != null)
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
