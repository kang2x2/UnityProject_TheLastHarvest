using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    Scene_Base _curScene;
    LoadingManager _loadingManager;

    public Scene_Base CurScene { get { return _curScene; } set { _curScene = value; } }
    public bool IsLoading { get { return _loadingManager._isLoading; }} 

    public void Init()
    {
        _loadingManager = new LoadingManager();
        _loadingManager.Init();
        CurScene = GameObject.Find("@Scene").GetComponent<Scene_Base>();
        CurScene.Init();
        ActiveGame();
    }

    public void ActiveGame()
    {
        IEnumerator coFadeIn = Managers.FadeManager.FadeIn(() => {
            Managers.SoundManager.PlayBGM("BGM", 0.3f);
        }, 2.0f);

        Managers.CoroutineManager.MyStartCoroutine(coFadeIn);
    }

    public void ChangeScene(Define.SceneType type)
    {
        _loadingManager.EnterLoading(type);
    }

    public void Clear()
    {
        CurScene = null;
    }
}
