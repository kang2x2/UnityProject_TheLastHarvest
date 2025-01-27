using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopUpUI_Store : UI_PopUp
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
        ReturnButton,
    }

    enum Texts
    {
        UserGoldText,

        AttackStatText,
        SpeedStatText,
        MaxHpStatText,
        RecoveryStatText,
        ExpStatText,
        SelectStatText,
    }

    List<UI_StoreItem> _itemUis = new List<UI_StoreItem>();
    public override void Init()
    {
        base.Init();

        UI_Bind<Scrollbar>(typeof(Scrollbars));
        UI_Bind<GameObject>(typeof(GameObjects));
        UI_Bind<Button>(typeof(Buttons));
        UI_Bind<Text>(typeof(Texts));

        for (int i = 0; i < Managers.DataManager.Store.Data.passiveItems.Length; ++i)
        {
            UI_StoreItem itemUI = Managers.ResourceManager.Instantiate
                ("UI/PopUps/UI_PassiveCard", UI_Get<GameObject>((int)GameObjects.Content).transform).GetComponent< UI_StoreItem>();
            itemUI.Init(i);
            itemUI.gameObject.SetActive(false);
            _itemUis.Add(itemUI);
        }

        UI_Get<Scrollbar>((int)Scrollbars.ScrollbarVertical).value = 1;
        UI_BindEvent(UI_Get<Button>((int)Buttons.ReturnButton).gameObject, ClickReturnButton);
    }

    public override void Show(object param = null)
    {
        UI_Get<Scrollbar>((int)Scrollbars.ScrollbarVertical).value = 1;
        Vector2 downPos = new Vector2(0.0f, -1000.0f);
        foreach (UI_StoreItem ui in _itemUis)
        {
            ui.gameObject.SetActive(true);
        }

        UI_Get<GameObject>((int)GameObjects.Content).GetComponent<RectTransform>().localPosition = downPos;
    }

    public void ClickReturnButton(PointerEventData data)
    {
        foreach(UI_StoreItem ui in _itemUis)
        {
            ui.gameObject.SetActive(false);
        }

        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.CloseCurPopUpUI();
    }

    private void LateUpdate()
    {
        UI_Get<Text>((int)Texts.UserGoldText).text = Managers.DataManager.User.Data.gold.ToString();

        int iStat = Managers.DataManager.User.GetUserStat_Int(Define.UserStatType.Attack);
        UI_Get<Text>((int)Texts.AttackStatText).text = iStat.ToString() + "%";

        iStat = Managers.DataManager.User.GetUserStat_Int(Define.UserStatType.MoveSpeed);
        UI_Get<Text>((int)Texts.SpeedStatText).text = iStat.ToString() + "%";

        iStat = Managers.DataManager.User.GetUserStat_Int(Define.UserStatType.MaxHP, 0);
        UI_Get<Text>((int)Texts.MaxHpStatText).text = iStat.ToString() + "%";

        iStat = Managers.DataManager.User.GetUserStat_Int(Define.UserStatType.Exp);
        UI_Get<Text>((int)Texts.ExpStatText).text = iStat.ToString() + "%";

        iStat = Managers.DataManager.User.Data.selectCount;
        UI_Get<Text>((int)Texts.SelectStatText).text = iStat.ToString();

        float fStat = Managers.DataManager.User.GetUserStat_Float(Define.UserStatType.Recovery, 1);
        UI_Get<Text>((int)Texts.RecoveryStatText).text = fStat.ToString() + "%";
    }
}
