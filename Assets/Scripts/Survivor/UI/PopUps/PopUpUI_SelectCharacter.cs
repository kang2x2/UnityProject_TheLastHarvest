using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopUpUI_SelectCharacter : UI_PopUp
{
    public Data_Character[] _characterDatas;
    int _characterIndex;
    enum GameObjects
    {
        Character,
    }

    enum Texts
    {
        NameText,
        DescText,
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
        _characterIndex = 0;

        UI_Bind<GameObject>(typeof(GameObjects));
        UI_Bind<Text>(typeof(Texts));
        UI_Bind<Button>(typeof(Buttons));

        UI_BindEvent(UI_Get<Button>((int)Buttons.SelectButton).gameObject, ClickSelectButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.ReturnButton).gameObject, ClickReturnButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.NextButton).gameObject, ClickNextButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.PrevButton).gameObject, ClickPrevButton);
    }

    public void ClickSelectButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ShowPopUpUI_Check("PopUpUI_Check", "게임을 시작할까요?", () =>
        {
            Managers.GameManagerEx.PlayerData = _characterDatas[_characterIndex];
            Managers.SceneManagerEx.ChangeScene(Define.SceneType.GameScene);
        });
    }

    public void ClickReturnButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ClosePopUpUI("PopUpUI_SelectCharacter");
    }

    public void ClickNextButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");

        _characterIndex += 1;
        if (_characterIndex >= _characterDatas.Length)
        {
            _characterIndex = 0;
        }
    }

    public void ClickPrevButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");

        _characterIndex -= 1;
        if (_characterIndex < 0)
        {
            _characterIndex = _characterDatas.Length - 1;
        }
    }

    private void LateUpdate()
    {
        Animator anim = UI_Get<GameObject>((int)GameObjects.Character).GetComponent<Animator>();
        anim.runtimeAnimatorController = _characterDatas[_characterIndex].selectAnimator;

        UI_Get<Text>((int)Texts.NameText).text = _characterDatas[_characterIndex].name;
        UI_Get<Text>((int)Texts.DescText).text = _characterDatas[_characterIndex].desc;
    }
}
