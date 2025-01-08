using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUpUI_SignUp : UI_PopUp
{
    enum InputFields
    {
        IdInputField,
        PasswordInputField,
        NickNameInputField,
    }
    enum Buttons
    {
        SignUpButton,
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

        UI_BindEvent(UI_Get<Button>((int)Buttons.SignUpButton).gameObject, ClickSignUpButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.ReturnButton).gameObject, ClickReturnButton);
    }

    public override void Show(object param = null)
    {
        UI_Get<Text>((int)Texts.ErrorText).text = "";
    }

    public void ClickSignUpButton(PointerEventData data)
    {
        GameResult res = new GameResult()
        {
            userId = UI_Get<InputField>((int)InputFields.IdInputField).text,
            userPassword = UI_Get<InputField>((int)InputFields.PasswordInputField).text,
            userName = UI_Get<InputField>((int)InputFields.NickNameInputField).text,
            killScore = 0,
            clearScore = 0,
            playTime = 0.0f,
            date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
        };

        IEnumerator coSignUp = Managers.WebManager.CoSignUpRequest("ranking/adduserdata", "POST", res, (str) =>
        {
            if (str == "ȸ�������� �Ϸ�Ǿ����ϴ�.")
            {
                Managers.SoundManager.PlaySFX("UISounds/SelectionComplete");
                Managers.UIManager.ShowPopUpUI_Complete("PopUpUI_Complete", "ȸ�������� �Ϸ�Ǿ����ϴ�.", () =>
                {
                    Managers.UIManager.ClosePopUpUI("PopUpUI_SignUp");
                });
            }
            else
            {
                Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
                UI_Get<Text>((int)Texts.ErrorText).color = Color.red;
            }

            UI_Get<Text>((int)Texts.ErrorText).text = str;
        });

        StartCoroutine(coSignUp);
    }

    public void ClickReturnButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ClosePopUpUI("PopUpUI_SignUp");
    }
}
