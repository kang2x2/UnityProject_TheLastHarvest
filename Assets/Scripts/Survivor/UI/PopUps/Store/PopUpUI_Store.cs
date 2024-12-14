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
    }
}
