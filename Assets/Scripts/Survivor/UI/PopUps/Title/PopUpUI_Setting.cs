using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PopUpUI_Setting : UI_PopUp
{
    enum Sliders
    {
        BGMVolum,
        SFXVolum,
    }

    enum Buttons
    {
        VolumCheckButton,
        DataResetButton,
        ReturnButton,
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

        UI_BindEvent(UI_Get<Button>((int)Buttons.VolumCheckButton).gameObject, ClickVolumCheckButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.DataResetButton).gameObject, ClickDataResetButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.ReturnButton).gameObject, ClickReturnButton);

        UI_BindEvent(UI_Get<Slider>((int)Sliders.BGMVolum).gameObject, VolumSet_BGM);
        UI_BindEvent(UI_Get<Slider>((int)Sliders.SFXVolum).gameObject, VolumSet_SFX);
        UI_BindEvent(UI_Get<Slider>((int)Sliders.BGMVolum).gameObject, VolumSet_BGM, Define.UIEvent.Drag);
        UI_BindEvent(UI_Get<Slider>((int)Sliders.SFXVolum).gameObject, VolumSet_SFX, Define.UIEvent.Drag);
    }

    public override void Show(object param = null)
    {
        UI_Get<Slider>((int)Sliders.BGMVolum).value = Managers.SoundManager.BGMVolum;
        UI_Get<Slider>((int)Sliders.SFXVolum).value = Managers.SoundManager.SFXVolum;

        UI_Get<Text>((int)Texts.BGMRatioText).text = Mathf.RoundToInt(Managers.SoundManager.BGMVolum * 100) + "%";
        UI_Get<Text>((int)Texts.SFXRatioText).text = Mathf.RoundToInt(Managers.SoundManager.SFXVolum * 100) + "%";
    }

    public void ClickVolumCheckButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/SelectionComplete");
    }

    public void ClickDataResetButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ShowPopUpUI_Check("PopUpUI_Check", "ȹ���� ĳ���� �� ��ȭ�� �ɷ�ġ�� ���� ������ϴ�. ���� ��� �����͸� �ʱ�ȭ �մϱ�?", () =>
        {
            Managers.DataManager.DataAllReset();
            Managers.UIManager.ClosePopUpUI("PopUpUI_Check");
            Managers.UIManager.ShowPopUpUI_Complete("PopUpUI_Complete", "��� �������� �ʱ�ȭ�� �Ϸ�ƽ��ϴ�.");
        });
    }

    public void ClickReturnButton(PointerEventData data)
    {
        Managers.DataManager.Sound.SoundDataOverwrite(
            UI_Get<Slider>((int)Sliders.BGMVolum).value, 
            UI_Get<Slider>((int)Sliders.SFXVolum).value);

        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ClosePopUpUI("PopUpUI_Setting");
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
