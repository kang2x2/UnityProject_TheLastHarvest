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

    public override void Init()
    {
        base.Init();

        UI_Bind<InputField>(typeof(InputFields));
        UI_Bind<Button>(typeof(Buttons));

        UI_BindEvent(UI_Get<Button>((int)Buttons.SignUpButton).gameObject, ClickSignUpButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.ReturnButton).gameObject, ClickReturnButton);
    }

    public override void Show(object param = null)
    {
        UI_Get<InputField>((int)InputFields.IdInputField).text = "";
        UI_Get<InputField>((int)InputFields.PasswordInputField).text = "";
        UI_Get<InputField>((int)InputFields.NickNameInputField).text = "";
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

        IEnumerator coServerCheck = Managers.WebManager.CheckServer(() =>
        {
            IEnumerator coSignUp = Managers.WebManager.CoSignUpRequest("ranking/adduserdata", "POST", res, (str) =>
            {
                if (str == "회원가입이 완료되었습니다.")
                {
                    Managers.SoundManager.PlaySFX("UISounds/SelectionComplete");
                    Managers.UIManager.ShowPopUpUI_Complete("PopUpUI_Complete", "회원가입이 완료되었습니다.", () =>
                    {
                        Managers.UIManager.CloseCurPopUpUI();
                    });
                }
                else
                {
                    Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
                    Managers.UIManager.ShowPopUpUI_Complete("PopUpUI_Complete", str);
                }
            });

            StartCoroutine(coSignUp);
        });

        StartCoroutine(coServerCheck);
    }

    public void ClickReturnButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.CloseCurPopUpUI();
    }
}
