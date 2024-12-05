using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _curOrder = 1;

    Dictionary<string, UI_PopUp> _popUps = new Dictionary<string, UI_PopUp>();

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

    public void ShowPopUpUI(string name)
    {
        UI_PopUp popUp = null;

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
        popUp.Show();
    }

    public void ClosePopUpUI(string name)
    {
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
        _popUps.Clear();
        _curOrder = 1;
    }
}
