using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PopUpUI_Check : UI_PopUp
{
    Action _action;

    enum Texts
    {
        CheckText,
    }
    enum Buttons
    {
        YesButton,
        NoButton
    }

    public override void Init()
    {
        base.Init();

        UI_Bind<Text>(typeof(Texts));
        UI_Bind<Button>(typeof(Buttons));

        UI_BindEvent(UI_Get<Button>((int)Buttons.YesButton).gameObject, ClickYes);
        UI_BindEvent(UI_Get<Button>((int)Buttons.NoButton).gameObject, ClickNo);
    }

    public void ValueInit(string text, Action action)
    {
        UI_Get<Text>((int)Texts.CheckText).text = text;
        _action = action;
    }

    public void ClickYes(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/SelectionComplete");
        Managers.UIManager.CloseCurPopUpUI(() => 
        {
            Managers.GameManagerEx.Continue();
            _action.Invoke();
        });
    }

    public void ClickNo(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.CloseCurPopUpUI();
    }
}
