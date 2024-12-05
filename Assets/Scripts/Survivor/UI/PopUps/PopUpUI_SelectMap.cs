using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopUpUI_SelectMap : UI_PopUp
{
    public Data_Map[] _mapDatas;
    int _mapIndex;
    enum Images
    {
        MapImage,
    }

    enum Texts
    {
        NameText,
    }

    enum Buttons
    {
        SelectButton,
        ReturnButton,
        NextButton,
        PrevButton,
    }

    public override void Init()
    {
        _mapIndex = 0;

        UI_Bind<Image>(typeof(Images));
        UI_Bind<Text>(typeof(Texts));
        UI_Bind<Button>(typeof(Buttons));

        UI_BindEvent(UI_Get<Button>((int)Buttons.SelectButton).gameObject, ClickSelectButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.ReturnButton).gameObject, ClickReturnButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.NextButton).gameObject, ClickNextButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.PrevButton).gameObject, ClickPrevButton);
    }

    public void ClickSelectButton(PointerEventData data)
    {
        Managers.GameManagerEx.MapType = _mapDatas[_mapIndex].mapType;
        Managers.UIManager.ShowPopUpUI("PopUpUI_SelectCharacter");
    }

    public void ClickReturnButton(PointerEventData data)
    {
        Managers.UIManager.ClosePopUpUI("PopUpUI_SelectMap");
    }

    public void ClickNextButton(PointerEventData data)
    {
        _mapIndex += 1;
        if (_mapIndex >= _mapDatas.Length)
        {
            _mapIndex = 0;
        }
    }

    public void ClickPrevButton(PointerEventData data)
    {
        _mapIndex -= 1;
        if (_mapIndex < 0)
        {
            _mapIndex = _mapDatas.Length - 1;
        }
    }

    private void LateUpdate()
    {
        UI_Get<Image>((int)Images.MapImage).sprite = _mapDatas[_mapIndex].icon;
        UI_Get<Text>((int)Texts.NameText).text = _mapDatas[_mapIndex].name;
    }
}
