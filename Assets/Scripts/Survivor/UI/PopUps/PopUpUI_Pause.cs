using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIPopUp_Pause : UI_PopUp
{
    enum Buttons
    {
        SettingButton,
        TitleButton,
        CloseButton,
    }

    public override void Init()
    {
        UI_Bind<Button>(typeof(Buttons));

        UI_BindEvent(UI_Get<Button>((int)Buttons.TitleButton).gameObject, ShowCheckUI);
        UI_BindEvent(UI_Get<Button>((int)Buttons.CloseButton).gameObject, ClosePauseUI);
    }

    public void ShowCheckUI(PointerEventData data)
    {
        Managers.UIManager.ShowPopUpUI_Check("PopUpUI_Check", "타이틀 화면으로 돌아갑니까?", () =>
        {
            Managers.SceneManagerEx.ChangeScene(Define.SceneType.TitleScene);
        });
    }

    public void ClosePauseUI(PointerEventData data)
    {
        Managers.UIManager.ClosePopUpUI("PopUpUI_Pause");
        Managers.GameManagerEx.Continue();
    }
}
