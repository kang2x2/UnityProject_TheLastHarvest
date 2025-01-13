using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopUpUI_Get : UI_PopUp
{
    public Data_GetCharacter[] _characterDatas;
    enum Images
    {
        ObjectImage,
    }

    enum Texts
    {
        NameText,
        DescText,
    }

    enum Buttons
    {
        OkButton,
    }

    public override void Init()
    {
        base.Init();

        UI_Bind<Image>(typeof(Images));
        UI_Bind<Text>(typeof(Texts));
        UI_Bind<Button>(typeof(Buttons));

        UI_BindEvent(UI_Get<Button>((int)Buttons.OkButton).gameObject, ClickOkButton);
    }

    public override void Show(object param = null)
    {
        int index = (int)param;

        Managers.SoundManager.PlaySFX("UISounds/CharacterGet"); 

        UI_Get<Image>((int)Images.ObjectImage).sprite = _characterDatas[index].image;
        UI_Get<Text>((int)Texts.NameText).text = _characterDatas[index].name;
        UI_Get<Text>((int)Texts.DescText).text = _characterDatas[index].desc;
    }

    public void ClickOkButton(PointerEventData data)
    {
        IsShow = false;
        Managers.UIManager.CloseCurPopUpUI();
    }
}
