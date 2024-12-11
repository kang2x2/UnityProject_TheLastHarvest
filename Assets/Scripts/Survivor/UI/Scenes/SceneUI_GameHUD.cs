using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneUI_GameHUD : UI_Scene
{
    enum Sliders
    {
        ExpBar,
        PlayerHpBar,
        BossHpBar,
    }

    enum Images
    {
        KillIcon,
    }

    enum Buttons
    {
        PauseButton,
    }
    
    enum Texts
    {
        LevelText,
        KillText,
        TimerText,
    }

    private void Awake()
    {
        UI_Bind<Slider>(typeof(Sliders));
        UI_Bind<Image>(typeof(Images));
        UI_Bind<Button>(typeof(Buttons));
        UI_Bind<Text>(typeof(Texts));

        UI_BindEvent(UI_Get<Button>((int)Buttons.PauseButton).gameObject, ClickPauseButton);

        if(Managers.GameManagerEx.MapType == Define.MapType.Cave)
        {
            UI_Get<Text>((int)Texts.KillText).color = Color.white;
            UI_Get<Text>((int)Texts.LevelText).color = Color.white;
        }
        else
        {
            UI_Get<Text>((int)Texts.KillText).color = Color.black;
            UI_Get<Text>((int)Texts.LevelText).color = Color.black;
        }

        UI_Get<Slider>((int)Sliders.BossHpBar).gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Managers.GameManagerEx.IsPause == true)
        {
            return;
        }
    }

    private void LateUpdate()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.GameManagerEx.Player == null)
        {
            return;
        }

        Player player = Managers.GameManagerEx.Player.GetComponent<Player>();
        if(float.IsNaN(player.Hp / player.MaxHp) == false)
        {
            UI_Get<Slider>((int)Sliders.PlayerHpBar).value = player.Hp / player.MaxHp;
        }

        UI_Get<Slider>((int)Sliders.ExpBar).value = (float)Managers.GameManagerEx.CurExp / Managers.GameManagerEx.DestExp;
        UI_Get<Text>((int)Texts.KillText).text = string.Format("{0:F0}", Managers.GameManagerEx.Kill);
        UI_Get<Text>((int)Texts.LevelText).text = string.Format("Lv.{0:F0}", Managers.GameManagerEx.GameLevel);

        int min = Mathf.FloorToInt(Managers.GameManagerEx.ProgressTime / 60);
        int sec = Mathf.FloorToInt(Managers.GameManagerEx.ProgressTime % 60);
        UI_Get<Text>((int)Texts.TimerText).text = string.Format("{0:D2}:{1:D2}", min, sec);

        if(Managers.GameManagerEx.IsBossBattle == true)
        {
            if(UI_Get<Slider>((int)Sliders.BossHpBar).gameObject.activeSelf == false)
            {
                UI_Get<Slider>((int)Sliders.BossHpBar).gameObject.SetActive(true);
            }
            UI_Get<Slider>((int)Sliders.BossHpBar).value = Managers.GameManagerEx.Boss.Hp / Managers.GameManagerEx.Boss.MaxHp;
        }
    }

    public void ClickPauseButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ShowPopUpUI("PopUpUI_Pause");
        Managers.GameManagerEx.Pause();
    }
}
