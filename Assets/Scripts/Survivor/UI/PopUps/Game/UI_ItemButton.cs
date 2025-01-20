using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemButton : UI_Base
{
    enum Images
    {
        ItemImage,
    }

    enum Texts
    {
        LevelText,
        NameText,
        ItemDescText,
        AbilityDescText,
    }

    enum textType
    {
        Level,
        Name,
        ItemDesc,
        AbilityDesc,
        End
    }

    enum Buttons
    {
        GetButton,
    }

    public Data_Item ItemData { get; set; }

    public Survivor_Item Item { get; set; } = null;

    public bool IsLive { get; private set; } = true;

    int _level = 0;

    public override void Init()
    {
        UI_Bind<Image>(typeof(Images));
        UI_Bind<Text>(typeof(Texts));
        UI_Bind<Button>(typeof(Buttons));

        UI_Get<Image>((int)Images.ItemImage).sprite = ItemData.icon;

        UI_Get<Text>((int)Texts.NameText).text = ItemData.name;
        UI_Get<Text>((int)Texts.ItemDescText).text = ItemData.itemDesc;
        UI_Get<Text>((int)Texts.AbilityDescText).text = ItemData.abilityDesc;

        UI_BindEvent(UI_Get<Button>((int)Buttons.GetButton).gameObject, ClickGetButton);
    }

    private void LateUpdate()
    {
        if(ItemData.abilityType == Define.AbilityType.Init)
        {
            UI_Get<Text>((int)Texts.LevelText).text = "½Å±Ô È¹µæ";
        }
        else if (ItemData.abilityType == Define.AbilityType.Consumption)
        {
            UI_Get<Text>((int)Texts.LevelText).text = "¼Ò¸ðÇ°";
        }
        else
        {
            string nextLevel = _level + 1 >= ItemData.maxLevel ? "Max" : "Lv." + (_level + 2).ToString();
            UI_Get<Text>((int)Texts.LevelText).text = $"Lv.{_level + 1} -> {nextLevel}";
        }
    }

    public void ClickGetButton(PointerEventData data)
    {
        Player player = Managers.GameManagerEx.Player.GetComponent<Player>();

        if (ItemData.abilityType == Define.AbilityType.Init)
        {
            Item.Init();
            player.HasItem[(int)ItemData.itemName] = true;
            IsLive = false;
        }
        else if(ItemData.abilityType == Define.AbilityType.Consumption)
        {
            switch(ItemData.itemName)
            {
                case Define.ItemName.HealthPack:
                    player.Hp = player.MaxHp;
                    break;
            }
        }
        else
        {
            _level += 1;
            Item.LevelUp(ItemData.abilityType);

            if (_level >= ItemData.maxLevel)
            {
                IsLive = false;
                UI_Get<Button>((int)Buttons.GetButton).interactable = false;
            }
        }

        Managers.SoundManager.PlaySFX("UISounds/CardSelect");
        Managers.UIManager.CloseCurPopUpUI(() => {
            if (Managers.GameManagerEx.LevelUpEffect.isPlaying == true)
            {
                Managers.GameManagerEx.LevelUpEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            Managers.ItemCardManager.CompletedSelect();
            Managers.GameManagerEx.Continue(); 
        });
    }
}
