using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopUpUI_LevelUp : UI_PopUp
{
    enum Scrollbars
    {
        ScrollbarVertical,
    }
    enum GameObjects
    {
        Content,
    }
    enum Buttons
    {
        ReRollItemButton,
        ReRollGoldButton,
    }
    enum Texts
    {
        HasReRollText,
        HasGoldText,
        NeedGoldText,
    }

    int _reRollPrice;
    public override void Init()
    {
        base.Init();

        UI_Bind<Scrollbar>(typeof(Scrollbars));
        UI_Bind<GameObject>(typeof(GameObjects));
        UI_Bind<Button>(typeof(Buttons));
        UI_Bind<Text>(typeof(Texts));

        UI_BindEvent(UI_Get<Button>((int)Buttons.ReRollItemButton).gameObject, ClickReRollButton_Item);
        UI_BindEvent(UI_Get<Button>((int)Buttons.ReRollGoldButton).gameObject, ClickReRollButton_Gold);

        _reRollPrice = 2;
    }

    public override void Show(object param = null)
    {
        UI_Get<Scrollbar>((int)Scrollbars.ScrollbarVertical).value = 1;

        Vector2 downPos = new Vector2(0.0f, -1000.0f);
        UI_Get<GameObject>((int)GameObjects.Content).GetComponent<RectTransform>().localPosition = downPos;

        Managers.SoundManager.PlaySFX("UISounds/LevelUp");
        Managers.GameManagerEx.Pause();

        Player player = Managers.GameManagerEx.Player.GetComponent<Player>();
        Managers.ItemCardManager.ItemCardSuffle(player.SelectItemCount, UI_Get<GameObject>((int)GameObjects.Content).transform);
        
        Time.timeScale = 1.0f;
        Managers.GameManagerEx.LevelUpEffect.Play();
    }

    public void ClickReRollButton_Item(PointerEventData data)
    {
        Player player = Managers.GameManagerEx.Player.GetComponent<Player>();

        if (player.ReRollCount > 0)
        {
            UI_Get<Scrollbar>((int)Scrollbars.ScrollbarVertical).value = 1;

            Vector2 downPos = new Vector2(0.0f, -1000.0f);
            UI_Get<GameObject>((int)GameObjects.Content).GetComponent<RectTransform>().localPosition = downPos;

            Managers.SoundManager.PlaySFX("UISounds/SelectionComplete");

            player.ReRollCount -= 1;
            Managers.ItemCardManager.ItemCardSuffle(player.SelectItemCount, UI_Get<GameObject>((int)GameObjects.Content).transform);
        }
    }

    public void ClickReRollButton_Gold(PointerEventData data)
    {
        int gold = Managers.DataManager.User.Data.gold;
        if(gold >= _reRollPrice)
        {
            UI_Get<Scrollbar>((int)Scrollbars.ScrollbarVertical).value = 1;

            Vector2 downPos = new Vector2(0.0f, -1000.0f);
            UI_Get<GameObject>((int)GameObjects.Content).GetComponent<RectTransform>().localPosition = downPos;

            Managers.SoundManager.PlaySFX("UISounds/SelectionComplete");

            Managers.DataManager.User.Data.gold -= _reRollPrice;
            Managers.DataManager.User.UserDataOverwrite();
            _reRollPrice *= 2;

            Player player = Managers.GameManagerEx.Player.GetComponent<Player>();
            Managers.ItemCardManager.ItemCardSuffle(player.SelectItemCount, UI_Get<GameObject>((int)GameObjects.Content).transform);
        }
    }

    private void LateUpdate()
    {
        Player player = Managers.GameManagerEx.Player.GetComponent<Player>();
        UI_Get<Text>((int)Texts.HasReRollText).text = player.ReRollCount.ToString();

        if (player.ReRollCount <= 0)
        {
            UI_Get<Button>((int)Buttons.ReRollItemButton).interactable = false;
            UI_Get<Button>((int)Buttons.ReRollItemButton).image.color = Color.gray;
        }
        else
        {
            UI_Get<Button>((int)Buttons.ReRollItemButton).interactable = true;
            UI_Get<Button>((int)Buttons.ReRollItemButton).image.color = Color.white;
        }


        int gold = Managers.DataManager.User.Data.gold;
        UI_Get<Text>((int)Texts.HasGoldText).text = gold.ToString();
        UI_Get<Text>((int)Texts.NeedGoldText).text = _reRollPrice.ToString();

        if(gold < _reRollPrice)
        {
            UI_Get<Button>((int)Buttons.ReRollGoldButton).interactable = false;
            UI_Get<Button>((int)Buttons.ReRollGoldButton).image.color = Color.gray;
        }
        else
        {
            UI_Get<Button>((int)Buttons.ReRollGoldButton).interactable = true;
            UI_Get<Button>((int)Buttons.ReRollGoldButton).image.color = Color.white;
        }
    }
}
