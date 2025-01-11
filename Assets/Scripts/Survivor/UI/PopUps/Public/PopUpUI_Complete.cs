using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PopUpUI_Complete : UI_PopUp
{
    Action _action;

    enum Texts
    {
        CheckText,
    }
    enum Buttons
    {
        OkButton,
    }

    public override void Init()
    {
        base.Init();

        UI_Bind<Text>(typeof(Texts));
        UI_Bind<Button>(typeof(Buttons));

        UI_BindEvent(UI_Get<Button>((int)Buttons.OkButton).gameObject, ClickOkButton);
    }

    public void ValueInit(string text, Action action = null)
    {
        UI_Get<Text>((int)Texts.CheckText).text = text;
        _action = action;
    }

    public void ClickOkButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.CloseCurPopUpUI(() => { _action?.Invoke(); });
    }
}
