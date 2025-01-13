using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PopUpUI_Guid : UI_PopUp
{
    enum Buttons
    {
        NextButton,
        PrevButton,
        CloseButton
    }

    enum GuidImages
    {
        GuidPanel_GameGoal,
        GuidPanel_Item,
        GuidPanel_Store,
    }

    List<Image> _guidImages = new List<Image>();
    int _curGuidIndex;

    public override void Init()
    {
        base.Init();

        UI_Bind<Button>(typeof(Buttons));
        UI_Bind<Image>(typeof(GuidImages));

        UI_BindEvent(UI_Get<Button>((int)Buttons.NextButton).gameObject, ClickNextButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.PrevButton).gameObject, ClickPrevButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.CloseButton).gameObject, ClickCloseButton);

        foreach (Image guidImage in UI_GetAll<Image>())
        {
            _guidImages.Add(guidImage);
            guidImage.gameObject.SetActive(false);
        }
    }

    public override void Show(object param = null)
    {
        _curGuidIndex = 0;
        _guidImages[0].gameObject.SetActive(true);
    }

    public void ClickNextButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");

        _guidImages[_curGuidIndex].gameObject.SetActive(false);
        _curGuidIndex += 1;
        _guidImages[_curGuidIndex].gameObject.SetActive(true);
    }

    public void ClickPrevButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");

        _guidImages[_curGuidIndex].gameObject.SetActive(false);
        _curGuidIndex -= 1;
        _guidImages[_curGuidIndex].gameObject.SetActive(true);
    }

    public void ClickCloseButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.CloseCurPopUpUI(() => {
            foreach (Image guidImage in _guidImages)
            {
                guidImage.gameObject.SetActive(false);
            }
        });
    }

    private void LateUpdate()
    {
        if(_curGuidIndex == _guidImages.Count - 1)
        {
            UI_Get<Button>((int)Buttons.NextButton).gameObject.SetActive(false);
        }
        else
        {
            UI_Get<Button>((int)Buttons.NextButton).gameObject.SetActive(true);
        }

        if (_curGuidIndex == 0)
        {
            UI_Get<Button>((int)Buttons.PrevButton).gameObject.SetActive(false);
        }
        else
        {
            UI_Get<Button>((int)Buttons.PrevButton).gameObject.SetActive(true);
        }
    }
}
