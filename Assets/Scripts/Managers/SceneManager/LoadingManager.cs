using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager
{
    GameObject _uiLoading;
    Text _LoadingText;
    Text _TipText;

    public bool _isLoading;

    public void Init()
    {
        _uiLoading = Managers.ResourceManager.Instantiate("UI/UI_Loading");
        _LoadingText = _uiLoading.GetComponentsInChildren<Text>()[0];
        _TipText = _uiLoading.GetComponentsInChildren<Text>()[1];

        GameObject.DontDestroyOnLoad(_uiLoading);
        _uiLoading.gameObject.SetActive(false);
        _isLoading = false;
    }
    public void EnterLoading(Define.SceneType type)
    {
        _isLoading = true;
        _LoadingText.text = "Loading...";

        IEnumerator coFadeIn = Managers.FadeManager.FadeIn();
        IEnumerator coLoadScene = LoadSceneProgress();
        IEnumerator coFadeOut = Managers.FadeManager.FadeOut(() =>
        {
            Managers.Clear();
            SceneManager.LoadScene(Enum.GetName(typeof(Define.SceneType), type));
            Managers.CoroutineManager.MyStartCoroutine(coFadeIn);

            _uiLoading.gameObject.SetActive(true);
            Managers.CoroutineManager.MyStartCoroutine(coLoadScene);
        });

        Managers.CoroutineManager.MyStartCoroutine(coFadeOut);
    }

    IEnumerator LoadSceneProgress()
    {
        while (Managers.SceneManagerEx.CurScene == null)
        {
            Managers.SceneManagerEx.CurScene = GameObject.Find("@Scene").GetComponent<Scene_Base>();
            yield return null;
        }

        Managers.SceneManagerEx.CurScene.Init();

        // 코루틴의 인수로 들어있는 progress와 외부에서 선언한 progress는 별개.
        // float progress = 0.0f;
        IEnumerator coLoading = Managers.SceneManagerEx.CurScene.Loading(progress =>
        {
            _uiLoading.GetComponentInChildren<Slider>().value = progress;
        });

        Managers.CoroutineManager.MyStartCoroutine(coLoading);

        while (_uiLoading.GetComponentInChildren<Slider>().value < 1.0f)
        {
            yield return null;
        }

        float accTime = 0.0f;
        IEnumerator coFadeIn = Managers.FadeManager.FadeIn(() => { GameSet_BGM(); });
        IEnumerator coFadeOut = Managers.FadeManager.FadeOut(() =>
        {
            _uiLoading.gameObject.SetActive(false);
            Managers.CoroutineManager.MyStartCoroutine(coFadeIn);
            _uiLoading.GetComponentInChildren<Slider>().value = 0;
            _isLoading = false;
        });

        _LoadingText.text = "Success!";

        while (accTime < 2.0f)
        {
            accTime += Time.deltaTime;

            if (accTime >= 2.0f)
            {
                Managers.CoroutineManager.MyStartCoroutine(coFadeOut);
                break;
            }
            yield return null;
        }

        yield return true;
    }

    private void GameSet_BGM()
    {
        if (Managers.SceneManagerEx.CurScene.SceneType == Define.SceneType.TitleScene)
        {
            Managers.SoundManager.PlayBGM("BGMs/TitleBGM");
        }
        else if (Managers.SceneManagerEx.CurScene.SceneType == Define.SceneType.GameScene)
        {
            switch (Managers.GameManagerEx.MapType)
            {
                case Define.MapType.Field:
                    Managers.SoundManager.PlayBGM("BGMs/FieldBGM");
                    break;
                case Define.MapType.Cave:
                    Managers.SoundManager.PlayBGM("BGMs/CaveBGM");
                    break;

            }
        }
    }
}
