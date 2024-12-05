using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Base : MonoBehaviour
{
    public Define.SceneType SceneType { get; set; }
    public virtual void Init() { }

    public virtual IEnumerator Loading(Action<float> onProgress) 
    {
        onProgress?.Invoke(1.0f);
        yield return true;
    }

    void Update()
    {
        
    }
}
