using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager
{
    GameObject _uiFade;

    public void Init()
    {
        _uiFade = Managers.ResourceManager.Instantiate("UI/UI_Fade");
        _uiFade.GetComponentInChildren<Image>().color = new Vector4();
        GameObject.DontDestroyOnLoad(_uiFade);
    }

    public IEnumerator FadeOut(Action action = null, float fadeTime = 1.0f)
    {
        _uiFade.gameObject.SetActive(true);

        float accTime = 0.0f;
        float curAlpha = 0;

        Image fadeImage = _uiFade.GetComponentInChildren<Image>();

        while (accTime < fadeTime)
        {
            curAlpha = Mathf.Lerp(0, 1, accTime / fadeTime);
            Color color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, curAlpha);
            fadeImage.color = color;

            accTime += Time.deltaTime;

            yield return null;
        }

        action?.Invoke();
        yield return true;
    }

    public IEnumerator FadeIn(Action action = null, float fadeTime = 1.0f)
    {
        float accTime = 0.0f;
        float curAlpha = 1;

        Image fadeImage = _uiFade.GetComponentInChildren<Image>();

        while (accTime < fadeTime)
        {
            curAlpha = Mathf.Lerp(1, 0, accTime / fadeTime);
            Color color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, curAlpha);
            fadeImage.color = color;

            accTime += Time.deltaTime;

            yield return null;
        }

        _uiFade.gameObject.SetActive(false);
        action?.Invoke();

        yield return true;
    }
}
