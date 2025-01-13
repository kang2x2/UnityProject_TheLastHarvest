using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PopUp : UI_Base
{
    public bool IsShow { get; set; }
    protected Image _antiTouchImage;

    RectTransform _backGroundRect;
    public override void Init()
    {
        base.Init();

        // _backGroundRect = GameObject.Find("BackGroundImage").GetComponent<RectTransform>();
        _backGroundRect = transform.Find("BackGroundImage").GetComponent<RectTransform>();

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

    public IEnumerator coShowScale(Action action)
    {
        Time.timeScale = 1.0f;

        float eventTime = 0.1f;
        float accTime = 0.0f;
        float curScale = 0.0f;

        while(true)
        {
            float t = accTime / eventTime;
            curScale = Mathf.Lerp(0.0f, 1.0f, t);

            _backGroundRect.localScale = new Vector3(curScale, curScale, curScale);

            if (accTime > eventTime)
            {
                _backGroundRect.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                action?.Invoke();
                break;
            }

            accTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator coCloseScale(Action action)
    {
        Time.timeScale = 1.0f;

        float eventTime = 0.1f;
        float accTime = 0.0f;
        float curScale = 1.0f;

        while (true)
        {
            float t = accTime / eventTime;
            curScale = Mathf.Lerp(1.0f, 0.0f, t);

            _backGroundRect.localScale = new Vector3(curScale, curScale, curScale);

            if (accTime > eventTime)
            {
                _backGroundRect.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                action?.Invoke();
                break;
            }

            accTime += Time.deltaTime;
            yield return null;
        }
    }
}
