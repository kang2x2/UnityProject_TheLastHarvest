using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _curOrder = 1;
    Dictionary<string, UI_PopUp> _popUps = new Dictionary<string, UI_PopUp>();

    RectTransform _joystick;

    public UI_PopUp CurPopUp { get; private set; }

    public void SetJoyStick(RectTransform joystick)
    {
        _joystick = joystick;
        _joystick.localScale = Vector3.one;
    }

    public void ShowPopUpUI_Check(string name, string text, Action action)
    {
        ShowPopUpUI(name);
        PopUpUI_Check popUpCheck = _popUps["PopUpUI_Check"] as PopUpUI_Check;

        if(popUpCheck == null)
        {
            Debug.Log("Check PopUpUI Is Null...");
            return;
        }

        popUpCheck.ValueInit(text, action);
    }

    public void ShowPopUpUI(string name, object param = null)
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
        popUp.Show(param);

        CurPopUp = popUp;
    }

    public void ClosePopUpUI(string name)
    {
        if (_joystick != null)
        {
            _joystick.localScale = Vector3.one;
        }

        UI_PopUp popUp = null;

        if (_popUps.TryGetValue(name, out popUp) == false)
        {
            Debug.Log("Fail Find PopUpUI...");
            return;
        }

        _curOrder -= 1;
        popUp.gameObject.SetActive(false);
    }

    public void Clear()
    {
        _joystick = null;
        _popUps.Clear();
        _curOrder = 1;
    }
}
