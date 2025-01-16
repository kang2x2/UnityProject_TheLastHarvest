using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    public enum UIAnimationType
    {
        None,
        Scale,
        Page,
        End
    }

    int _curOrder = 1;
    Dictionary<string, UI_PopUp> _popUps = new Dictionary<string, UI_PopUp>();
    Stack<UI_PopUp> _popUpStack = new Stack<UI_PopUp>();

    RectTransform _joystick;

    public UI_PopUp CurPopUp { get; private set; }

    public void SetJoyStick(RectTransform joystick)
    {
        _joystick = joystick;
        _joystick.gameObject.SetActive(true);
        _joystick.localScale = Vector3.one;
    }

    public void ShowPopUpUI_Check(string name, string text, Action action, UIAnimationType type = UIAnimationType.Scale)
    {
        ShowPopUpUI(name, type);
        PopUpUI_Check popUpCheck = _popUps["PopUpUI_Check"] as PopUpUI_Check;

        if(popUpCheck == null)
        {
            Debug.Log("Check PopUpUI Is Null...");
            return;
        }

        popUpCheck.ValueInit(text, action);
    }

    public void ShowPopUpUI_Complete(string name, string text, Action action = null, UIAnimationType type = UIAnimationType.Scale)
    {
        ShowPopUpUI(name, type);
        PopUpUI_Complete popUpComplete = _popUps["PopUpUI_Complete"] as PopUpUI_Complete;

        if (popUpComplete == null)
        {
            Debug.Log("Complete PopUpUI Is Null...");
            return;
        }

        popUpComplete.ValueInit(text, action);
    }

    public void ShowPopUpUI(string name, object param = null, UIAnimationType type = UIAnimationType.Scale)
    {
        if(_joystick != null)
        {
            _joystick.localScale = Vector3.zero;
        }

        UI_PopUp popUp;

        if(_popUps.TryGetValue(name, out popUp) == false)
        {
            popUp = Managers.ResourceManager.Instantiate($"UI/PopUps/{name}").GetComponent<UI_PopUp>();
            _popUps.Add(name, popUp);
            popUp.Init();
        }

        if (popUp == null)
        {
            Debug.Log("Fail Load PopUpUI...");
            return;
        }

        popUp.gameObject.SetActive(true);
        popUp.GetComponent<Canvas>().sortingOrder = _curOrder++;
        _popUpStack.Push(popUp);
        CurPopUp = _popUpStack.Peek();
        CurPopUp.IsShow = true;

        Action showAction = () => { CurPopUp.Show(param); };

        switch(type)
        {
            case UIAnimationType.None:
                CurPopUp.Show(param);
                break;
            case UIAnimationType.Scale:
                IEnumerator coShowScale = CurPopUp.coShowScale(showAction);
                Managers.CoroutineManager.MyStartCoroutine(coShowScale);
                break;
            case UIAnimationType.Page:

                break;
        }
    }

    public void CloseCurPopUpUI(Action afterAction = null, UIAnimationType type = UIAnimationType.Scale, float closeTime = 0.0f)
    {
        if (_joystick != null)
        {
            _joystick.localScale = Vector3.one;
        }

        CurPopUp.IsShow = false;

        Action closeAction = () =>
        {
            _curOrder -= 1;
            CurPopUp.gameObject.SetActive(false);
            _popUpStack.Pop();

            if (_popUpStack.Count != 0)
            {
                CurPopUp = _popUpStack.Peek();
            }
            else
            {
                CurPopUp = null;
            }

            afterAction?.Invoke();
        };

        switch(type)
        {
            case UIAnimationType.None:
                closeAction.Invoke();
                break;
            case UIAnimationType.Scale:
                IEnumerator coCloseScale = CurPopUp.coCloseScale(closeAction);
                Managers.CoroutineManager.MyStartCoroutine(coCloseScale);
                break;
            case UIAnimationType.Page:

                break;
        }
    }

    public void Clear()
    {
        _joystick = null;
        _popUps.Clear();
        _curOrder = 1;
        CurPopUp = null;
    }
}
