using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PopUp : UI_Base
{
    readonly int MaxResolutionX = 2000;
    readonly int MaxResolutionY = 2000;

    readonly int MinResolutionX = 135;
    readonly int MinResolutionY = 270;

    public bool IsShow { get; protected set; }
    protected Image _antiTouchImage;

    // 참조할 해상도. 135에 270이면, x : 135 y : 270 해상도를 참조해 캔버스의 스케일을 조절함.
    CanvasScaler _canvasScaler;

    public override void Init()
    {
        base.Init();

        _canvasScaler = GetComponent<CanvasScaler>();

        _antiTouchImage = new GameObject() { name = "AntiTouchImage" }.AddComponent<Image>();
        _antiTouchImage.transform.SetParent(transform);
        _antiTouchImage.transform.SetSiblingIndex(0);
        _antiTouchImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        RectTransform antiTouchRect = _antiTouchImage.GetComponent<RectTransform>();

        antiTouchRect.anchorMin = Vector2.zero;
        antiTouchRect.anchorMax = Vector2.one;

        antiTouchRect.pivot = new Vector2(0.5f, 0.5f);

        antiTouchRect.offsetMin = Vector2.zero;
        antiTouchRect.offsetMax = Vector2.zero;

        antiTouchRect.localScale = Vector3.one;
    }

    public IEnumerator coShowUI(Action action)
    {
        float eventTime = 0.1f;
        float accTime = 0.0f;
        float resolutionValueX = MaxResolutionX;
        float resolutionValueY = MaxResolutionY;

        while(true)
        {
            float t = accTime / eventTime;
            resolutionValueX = Mathf.Lerp(MaxResolutionX, MinResolutionX, t);
            resolutionValueY = Mathf.Lerp(MaxResolutionY, MinResolutionY, t);

            _canvasScaler.referenceResolution = new Vector2(resolutionValueX, resolutionValueY);

            if (accTime > eventTime)
            {
                _canvasScaler.referenceResolution = new Vector2(MinResolutionX, MinResolutionY);
                action?.Invoke();
                break;
            }

            accTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator coCloseUI(Action action)
    {
        float eventTime = 0.1f;
        float accTime = 0.0f;
        float resolutionValueX = MinResolutionX;
        float resolutionValueY = MinResolutionY;

        while (true)
        {
            float t = accTime / eventTime;
            resolutionValueX = Mathf.Lerp(MinResolutionX, MaxResolutionX, t);
            resolutionValueY = Mathf.Lerp(MinResolutionY, MaxResolutionY, t);

            _canvasScaler.referenceResolution = new Vector2(resolutionValueX, resolutionValueY);

            if (accTime > eventTime)
            {
                _canvasScaler.referenceResolution = new Vector2(MaxResolutionX, MaxResolutionY);
                action?.Invoke();
                break;
            }

            accTime += Time.deltaTime;
            yield return null;
        }
    }
}
