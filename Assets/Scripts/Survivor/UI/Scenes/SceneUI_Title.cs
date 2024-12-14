using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneUI_Title : UI_Scene
{
    enum Buttons
    {
        StartButton,
        StoreButton,
        SettingButton,
        ExitButton,
    }

    private void Start()
    {
        UI_Bind<Button>(typeof(Buttons));
        UI_BindEvent(UI_Get<Button>((int)Buttons.StartButton).gameObject, ClickStartButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.StoreButton).gameObject, ClickStoreButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.SettingButton).gameObject, ClickSettingButton);
    }

    public void ClickStartButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ShowPopUpUI("PopUpUI_SelectMap");
    }
    public void ClickStoreButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ShowPopUpUI("PopUpUI_Store");
    }

    public void ClickSettingButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ShowPopUpUI("PopUpUI_TitleSetting");
    }
}

