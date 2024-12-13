using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PopUpUI_Complete : UI_PopUp
{
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
        UI_Bind<Text>(typeof(Texts));
        UI_Bind<Button>(typeof(Buttons));

        UI_BindEvent(UI_Get<Button>((int)Buttons.OkButton).gameObject, ClickOkButton);
    }

    public override void Show(object param = null)
    {
        UI_Get<Text>((int)Texts.CheckText).text = (string)param;
    }

    public void ClickOkButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ClosePopUpUI("PopUpUI_Complete");
    }
}
