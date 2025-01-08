using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUpUI_Login : UI_PopUp
{
    enum InputFields
    {
        IdInputField,
        PasswordInputField,
    }

    enum Buttons
    {
        SignUpButton,
        LoginButton,
        ReturnButton,
    }
    enum Texts
    {
        ErrorText,
    }

    public override void Init()
    {
        UI_Bind<InputField>(typeof(InputFields));
        UI_Bind<Button>(typeof(Buttons));
        UI_Bind<Text>(typeof(Texts));

        UI_BindEvent(UI_Get<Button>((int)Buttons.SignUpButton).gameObject, ClickSingUpButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.LoginButton).gameObject, ClickLoginButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.ReturnButton).gameObject, ClickReturnButton);
    }

    public override void Show(object param = null)
    {
        UI_Get<Text>((int)Texts.ErrorText).text = "";
    }

    public void ClickSingUpButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ShowPopUpUI("PopUpUI_SignUp");
    }

    public void ClickLoginButton(PointerEventData data)
    {
        string id = UI_Get<InputField>((int)InputFields.IdInputField).text;
        string pw = UI_Get<InputField>((int)InputFields.PasswordInputField).text;

        StartCoroutine(Managers.WebManager.CoLoginRequest("ranking/getuserdata", "Get",
            id, pw, (str) =>
        {
            if (str == "로그인 완료!")
            {
                Managers.SoundManager.PlaySFX("UISounds/SelectionComplete");
                Managers.UIManager.ShowPopUpUI_Complete("PopUpUI_Complete", "로그인 완료!", ()=> { 
                    Managers.UIManager.ClosePopUpUI("PopUpUI_Login");
                });
            }
            else
            {
                Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
                UI_Get<Text>((int)Texts.ErrorText).color = Color.red;
            }

            UI_Get<Text>((int)Texts.ErrorText).text = str;
        }));
    }

    public void ClickReturnButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ClosePopUpUI("PopUpUI_Login");
    }
}
