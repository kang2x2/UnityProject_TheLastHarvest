using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using System.IO;
public class UI_StoreItem : UI_Base
{
    private int _itemIndex;

    enum Images
    {
        ItemImage,
    }

    enum Texts
    {
        LevelText,
        DescText,
        PriceText,
    }

    enum Buttons
    {
        BuyButton,
    }

    public void Init(int index)
    {
        _itemIndex = index;

        UI_Bind<Image>(typeof(Images));
        UI_Bind<Text>(typeof(Texts));
        UI_Bind<Button>(typeof(Buttons));

        string spriteSheetPath = Managers.DataManager.Store.Data.passiveItems[_itemIndex].spriteSheetPath;
        Sprite[] sheet = Resources.LoadAll<Sprite>(spriteSheetPath);

        foreach (var sprite in sheet)
        {
            if (sprite.name == Managers.DataManager.Store.Data.passiveItems[_itemIndex].spriteName)
            {
                UI_Get<Image>((int)Images.ItemImage).sprite = sprite;
                break;
            }
        }

        UI_Get<Text>((int)Texts.DescText).text = Managers.DataManager.Store.Data.passiveItems[_itemIndex].desc;

        UI_BindEvent(UI_Get<Button>((int)Buttons.BuyButton).gameObject, ClickBuyButton);
    }

    private void LateUpdate()
    {
        UI_Get<Text>((int)Texts.LevelText).text = "Lv." + Managers.DataManager.Store.Data.passiveItems[_itemIndex].level.ToString();
        UI_Get<Text>((int)Texts.PriceText).text = Managers.DataManager.Store.Data.passiveItems[_itemIndex].price.ToString() + "G";

        int price = Managers.DataManager.Store.Data.passiveItems[_itemIndex].price;

        if(Managers.DataManager.User.Data.gold >= price)
        {
            UI_Get<Button>((int)Buttons.BuyButton).image.color = Color.green;
            UI_Get<Button>((int)Buttons.BuyButton).interactable = true;
        }
        else
        {
            UI_Get<Button>((int)Buttons.BuyButton).image.color = Color.gray;
            UI_Get<Button>((int)Buttons.BuyButton).interactable = false;
        }
    }

    public void ClickBuyButton(PointerEventData data)
    {
        int price = Managers.DataManager.Store.Data.passiveItems[_itemIndex].price;
        if (Managers.DataManager.User.Data.gold >= price)
        {
            Managers.DataManager.Store.Data.passiveItems[_itemIndex].level += 1;
            Managers.DataManager.Store.Data.passiveItems[_itemIndex].price += 10;
            Managers.DataManager.Store.StoreDataOverwrite();
            
            float bonus = Managers.DataManager.Store.Data.passiveItems[_itemIndex].addValue;
            Managers.DataManager.User.Data.bonus[Managers.DataManager.Store.Data.passiveItems[_itemIndex].upgradIndex] += bonus;

            Managers.DataManager.User.Data.gold -= price;
            Managers.DataManager.User.UserDataOverwrite();
        }
    }
}
