using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneUI_GameHUD : UI_Scene
{
    enum GameObjects
    {
        Stick,
        BossPanel,
    }
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
        GameSpeedButton,
        PauseButton,
    }

    enum Texts
    {
        LevelText,
        KillText,
        TimerText,
        HpText,

        GameSpeedText,

        AttackStatText,
        SpeedStatText,
        RecoveryStatText,
        ExpStatText,
        MargnetStatText,
    }

    private void Awake()
    {
        UI_Bind<GameObject>(typeof(GameObjects));
        UI_Bind<Slider>(typeof(Sliders));
        UI_Bind<Image>(typeof(Images));
        UI_Bind<Button>(typeof(Buttons));
        UI_Bind<Text>(typeof(Texts));

        UI_BindEvent(UI_Get<Button>((int)Buttons.PauseButton).gameObject, ClickPauseButton);
        UI_BindEvent(UI_Get<Button>((int)Buttons.GameSpeedButton).gameObject, ClickGameSpeedButton);

        foreach (Text text in UI_GetAll<Text>())
        {
            if (Managers.GameManagerEx.MapType == Define.MapType.Cave)
            {
                text.color = Color.white;
            }
            else
            {
                text.color = Color.black;
            }
        }

        UI_Get<GameObject>((int)GameObjects.BossPanel).gameObject.SetActive(false);
        // Managers.UIManager.SetJoyStick(UI_Get<GameObject>((int)GameObjects.Stick).GetComponent<RectTransform>());
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

        #region Hp
        if (player.Hp < player.MaxHp && player.Hp > 0.0f)
        {
            UI_Get<Text>((int)Texts.HpText).text = string.Format("{0:N1} / {1:F0}", player.Hp, player.MaxHp);
        }
        else
        {
            if(player.Hp <= 0.0f)
            {
                UI_Get<Text>((int)Texts.HpText).text = string.Format("{0:F0} / {1:F0}", 0, player.MaxHp);
            }
            else
            {
                UI_Get<Text>((int)Texts.HpText).text = string.Format("{0:F0} / {1:F0}", player.Hp, player.MaxHp);
            }
        }

        if (float.IsNaN(player.Hp / player.MaxHp) == false)
        {
            UI_Get<Slider>((int)Sliders.PlayerHpBar).value = player.Hp / player.MaxHp;
        }
        #endregion

        UI_Get<Slider>((int)Sliders.ExpBar).value = (float)Managers.GameManagerEx.CurExp / Managers.GameManagerEx.DestExp;
        UI_Get<Text>((int)Texts.KillText).text = string.Format("{0:F0}", Managers.GameManagerEx.Kill);
        UI_Get<Text>((int)Texts.LevelText).text = string.Format("Lv.{0:F0}", Managers.GameManagerEx.GameLevel);

        int min = Mathf.FloorToInt(Managers.GameManagerEx.ProgressTime / 60);
        int sec = Mathf.FloorToInt(Managers.GameManagerEx.ProgressTime % 60);
        UI_Get<Text>((int)Texts.TimerText).text = string.Format("{0:D2}:{1:D2}", min, sec);

        if(Managers.GameManagerEx.IsBossBattle == true)
        {
            if(UI_Get<GameObject>((int)GameObjects.BossPanel).gameObject.activeSelf == false)
            {
                UI_Get<GameObject>((int)GameObjects.BossPanel).gameObject.SetActive(true);
            }
            UI_Get<Slider>((int)Sliders.BossHpBar).value = Managers.GameManagerEx.Boss.Hp / Managers.GameManagerEx.Boss.MaxHp;
        }

        #region UserStat
        UI_Get<Text>((int)Texts.AttackStatText).text = string.Format("{0:N2}", player.AttackRatio);
        UI_Get<Text>((int)Texts.SpeedStatText).text = string.Format("{0:N2}", player.MoveSpeed);
        UI_Get<Text>((int)Texts.RecoveryStatText).text = string.Format("{0:N2}", player.RecoveryRatio);
        UI_Get<Text>((int)Texts.ExpStatText).text = string.Format("{0:N2}", player.GetExpRatio);
        UI_Get<Text>((int)Texts.MargnetStatText).text = string.Format("{0:N2}", player.Margent.GetComponent<CircleCollider2D>().radius);
        #endregion
    }

    public void ClickGameSpeedButton(PointerEventData data)
    {
        Managers.GameManagerEx.SetGameSpeed();

        if (Managers.GameManagerEx.GameSpeedIndex == 1)
        {
            UI_Get<Text>((int)Texts.GameSpeedText).text = string.Format("{0:N1}", Time.timeScale);
        }
        else
        {
            UI_Get<Text>((int)Texts.GameSpeedText).text = string.Format("{0:F0}", Time.timeScale);
        }
    }

    public void ClickPauseButton(PointerEventData data)
    {
        Managers.SoundManager.PlaySFX("UISounds/ButtonSelect");
        Managers.UIManager.ShowPopUpUI("PopUpUI_Pause");
        Managers.GameManagerEx.Pause();
    }
}
