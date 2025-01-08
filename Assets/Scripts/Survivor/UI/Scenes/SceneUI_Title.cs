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
        RankingButton,
        LoginButton,
        LogoutButton,
        ExitButton,
    }

    private void Start()
    {
        UI_Bind<Button>(typeof(Buttons));
        UI_BindEvent(UI_Get<Button>((int)Buttons.StartButton).gameObject, ClickStartButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.StoreButton).gameObject, ClickStoreButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.SettingButton).gameObject, ClickSettingButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.RankingButton).gameObject, ClickRankingButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.LoginButton).gameObject, ClickLoginButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.LogoutButton).gameObject, ClickLogoutButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.ExitButton).gameObject, ClickExitButton);
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
        Managers.UIManager.ShowPopUpUI("PopUpUI_Setting");
    }
    public void ClickRankingButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ShowPopUpUI("PopUpUI_Ranking");
    }

    public void ClickLoginButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ShowPopUpUI("PopUpUI_Login");
    }

    public void ClickLogoutButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ShowPopUpUI_Check("PopUpUI_Check", "�α׾ƿ� �ұ��?", () =>
        {
            Managers.WebManager.IsLogin = false;
            Managers.UIManager.ShowPopUpUI_Complete("PopUpUI_Complete", "�α׾ƿ��� �Ϸ�ƽ��ϴ�.");
        });
    }

    public void ClickExitButton(PointerEventData data)
    {
        Managers.DataManager.DataAllOverwrite();
        Application.Quit();
    }

    private void LateUpdate()
    {
        if(Managers.WebManager.IsLogin == true)
        {
            UI_Get<Button>((int)Buttons.LoginButton).gameObject.SetActive(false);
            UI_Get<Button>((int)Buttons.LogoutButton).gameObject.SetActive(true);
        }
        else
        {
            UI_Get<Button>((int)Buttons.LoginButton).gameObject.SetActive(true);
            UI_Get<Button>((int)Buttons.LogoutButton).gameObject.SetActive(false);
        }
    }
}

