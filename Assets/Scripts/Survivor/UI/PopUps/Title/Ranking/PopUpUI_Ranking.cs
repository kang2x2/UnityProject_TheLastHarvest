using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUpUI_Ranking : UI_PopUp
{
    enum Scrollbars
    {
        ScrollbarVertical,
    }
    enum GameObjects
    {
        Content,
    }
    enum Texts
    {
        RankingTypeText,
    }

    enum Buttons
    {
        NextButton,
        PrevButton,
        ReturnButton,
    }

    int _rankingType = (int)Define.RankingType.Kill;

    public override void Init()
    {
        UI_Bind<Scrollbar>(typeof(Scrollbars));
        UI_Bind<GameObject>(typeof(GameObjects));
        UI_Bind<Button>(typeof(Buttons));
        UI_Bind<Text>(typeof(Texts));

        UI_Get<Scrollbar>((int)Scrollbars.ScrollbarVertical).value = 1;
        UI_BindEvent(UI_Get<Button>((int)Buttons.ReturnButton).gameObject, ClickReturnButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.NextButton).gameObject, ClickNextButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.PrevButton).gameObject, ClickPrevButton);
    }

    public override void Show(object param = null)
    {
        RankingUpdate();
    }

    void RankingUpdate()
    {
        switch(_rankingType)
        {
            case (int)Define.RankingType.Kill:
                UI_Get<Text>((int)Texts.RankingTypeText).text = "적 처치 수";
                break;
            case (int)Define.RankingType.Clear:
                UI_Get<Text>((int)Texts.RankingTypeText).text = "클리어 횟수";
                break;
        }

        foreach (Transform child in UI_Get<GameObject>((int)GameObjects.Content).transform)
        {
            Managers.ResourceManager.Destroy(child.gameObject);
        }

        IEnumerator coGetAll = Managers.WebManager.CoGetAllRequest("ranking", "Get",
            (Define.RankingType)_rankingType, () => {

                for (int i = 0; i < Managers.WebManager.GameResults.Count; ++i)
                {
                    UI_UserRankingPanel panel = Managers.ResourceManager.Instantiate
                        ("UI/PopUps/UI_UserRankingPanel", UI_Get<GameObject>((int)GameObjects.Content).transform).GetComponent<UI_UserRankingPanel>();

                    panel.Init(i + 1, Managers.WebManager.GameResults[i], (Define.RankingType)_rankingType);
                }

                UI_Get<Scrollbar>((int)Scrollbars.ScrollbarVertical).value = 1;
                Vector2 downPos = new Vector2(0.0f, -1000.0f);
                UI_Get<GameObject>((int)GameObjects.Content).GetComponent<RectTransform>().localPosition = downPos;
            });
        StartCoroutine(coGetAll);
    }

    public void ClickNextButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");

        _rankingType += 1;
        if (_rankingType >= (int)Define.RankingType.End)
        {
            _rankingType = 0;
        }

        RankingUpdate();
    }

    public void ClickPrevButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");

        _rankingType -= 1;
        if (_rankingType < 0)
        {
            _rankingType = (int)Define.RankingType.End - 1;
        }

        RankingUpdate();
    }


    public void ClickReturnButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ClosePopUpUI("PopUpUI_Ranking");
    }
}
