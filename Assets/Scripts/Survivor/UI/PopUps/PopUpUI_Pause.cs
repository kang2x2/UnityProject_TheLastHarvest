using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIPopUp_Pause : UI_PopUp
{
    enum Sliders
    {
        BGMVolum,
        SFXVolum,
    }

    enum Buttons
    {
        SettingButton,
        TitleButton,
        CloseButton,
    }

    enum Texts
    {
        BGMRatioText,
        SFXRatioText,
    }

    public override void Init()
    {
        UI_Bind<Button>(typeof(Buttons));
        UI_Bind<Slider>(typeof(Sliders));
        UI_Bind<Text>(typeof(Texts));

        UI_BindEvent(UI_Get<Button>((int)Buttons.TitleButton).gameObject, ShowCheckUI);
        UI_BindEvent(UI_Get<Button>((int)Buttons.CloseButton).gameObject, ClosePauseUI);

        UI_BindEvent(UI_Get<Slider>((int)Sliders.BGMVolum).gameObject, VolumSet_BGM);
        UI_BindEvent(UI_Get<Slider>((int)Sliders.SFXVolum).gameObject, VolumSet_SFX);
        UI_BindEvent(UI_Get<Slider>((int)Sliders.BGMVolum).gameObject, VolumSet_BGM, Define.UIEvent.Drag);
        UI_BindEvent(UI_Get<Slider>((int)Sliders.SFXVolum).gameObject, VolumSet_SFX, Define.UIEvent.Drag);
    }

    public override void Show(object param = null)
    {
        UI_Get<Slider>((int)Sliders.BGMVolum).value = Managers.SoundManager.BGMVolum;
        UI_Get<Slider>((int)Sliders.SFXVolum).value = Managers.SoundManager.SFXVolum;
    }

    public void ShowCheckUI(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ShowPopUpUI_Check("PopUpUI_Check", "타이틀 화면으로 돌아갑니까?", () =>
        {
            Managers.SceneManagerEx.ChangeScene(Define.SceneType.TitleScene);
        });
    }

    public void ClosePauseUI(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ClosePopUpUI("PopUpUI_Pause");
        Managers.GameManagerEx.Continue();
    }

    public void VolumSet_BGM(PointerEventData data)
    {
        Managers.SoundManager.BGMVolum = UI_Get<Slider>((int)Sliders.BGMVolum).value;
        Managers.SoundManager.BGMSource.volume = Managers.SoundManager.BGMVolum;
    }
    public void VolumSet_SFX(PointerEventData data)
    {
        Managers.SoundManager.SFXVolum = UI_Get<Slider>((int)Sliders.SFXVolum).value;
    }

    private void LateUpdate()
    {
        UI_Get<Text>((int)Texts.BGMRatioText).text = Mathf.RoundToInt(Managers.SoundManager.BGMVolum * 100) + "%";
        UI_Get<Text>((int)Texts.SFXRatioText).text = Mathf.RoundToInt(Managers.SoundManager.SFXVolum * 100) + "%";
    }
}
