using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopUpUI_Store : UI_PopUp
{
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

    public override void Init()
    {
        UI_Bind<GameObject>(typeof(GameObjects));
        UI_Bind<Button>(typeof(Buttons));
        UI_Bind<Text>(typeof(Texts));

        for (int i = 0; i < Managers.DataManager.Store.Data.passiveItems.Length; ++i)
        {
            UI_StoreItem itemUI = Managers.ResourceManager.Instantiate
                ("UI/PopUps/UI_PassiveCard", UI_Get<GameObject>((int)GameObjects.Content).transform).GetComponent< UI_StoreItem>();
            itemUI.Init(i);
        }

        UI_BindEvent(UI_Get<Button>((int)Buttons.ReturnButton).gameObject, ClickReturnButton);
    }

    public void ClickReturnButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ClosePopUpUI("PopUpUI_Store");
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
