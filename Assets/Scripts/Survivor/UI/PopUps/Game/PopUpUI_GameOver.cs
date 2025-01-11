using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PopUpUI_GameOver : UI_PopUp
{
    enum BonusPanels
    {
        KillBonusPanel,
        TimeBonusPanel,
        LevelBonusPanel,
        ClearPanel,

        ResultPanel,
    }
    enum Images
    {
        BackGroundImage,
        GameOverImage,
    }
    enum Texts
    {
        KillCountText,
        TimeValueText,
        LevelValueText,

        KillBonusText,
        TimeBonusText,
        LevelBonusText,
        ResultBonusText,
    }

    enum Buttons
    {
        ReturnButton,
    }

    public Sprite[] headerImage;

    public override void Init()
    {
        base.Init();

        UI_Bind<GameObject>(typeof(BonusPanels));
        UI_Bind<Image>(typeof(Images));
        UI_Bind<Text>(typeof(Texts));
        UI_Bind<Button>(typeof(Buttons));

        UI_BindEvent(UI_Get<Button>((int)Buttons.ReturnButton).gameObject, ReturnTitle);
    }

    public override void Show(object param = null)
    {
        #region GameOverType

        UI_Get<Image>((int)Images.GameOverImage).sprite = headerImage[(int)Managers.GameManagerEx.GameOverType];
        switch(Managers.GameManagerEx.GameOverType)
        {
            case Define.GameOverType.Clear:
                UI_Get<Image>((int)Images.BackGroundImage).color = new Vector4(0.48f, 0.73f, 0.0f, 0.6f);
                break;
            case Define.GameOverType.Dead:
                UI_Get<Image>((int)Images.BackGroundImage).color = new Vector4(0.0f, 0.0f, 0.0f, 0.9f);
                break;
        }
        #endregion

        #region Disable
        GameObject[] bonusPanels = UI_GetAll<GameObject>();
        foreach(GameObject panel in bonusPanels)
        {   
            panel.SetActive(false);
        }
        UI_Get<Button>((int)Buttons.ReturnButton).gameObject.SetActive(false);
        #endregion

        StartCoroutine(ScoreCalculation());
    }

    public void ReturnTitle(PointerEventData data)
    {
        StartCoroutine(Managers.DataManager.Character.UnLockCharacter(() => {
            Managers.SoundManager.PlaySFX("UISounds/SelectionComplete");
            Managers.GameManagerEx.Continue();
            Managers.SceneManagerEx.ChangeScene(Define.SceneType.TitleScene);
        }));
    }

    private void LateUpdate()
    {
        UI_Get<Text>((int)Texts.KillCountText).text = string.Format("{0:F0}", Managers.GameManagerEx.Kill);

        int min = Mathf.FloorToInt(Managers.GameManagerEx.ProgressTime / 60);
        int sec = Mathf.FloorToInt(Managers.GameManagerEx.ProgressTime % 60);
        UI_Get<Text>((int)Texts.TimeValueText).text = string.Format("{0:D2}:{1:D2}", min, sec);

        UI_Get<Text>((int)Texts.LevelValueText).text = string.Format("{0:F0}", Managers.GameManagerEx.GameLevel);
    }

    IEnumerator ScoreCalculation()
    {
        int result = Managers.GameManagerEx.Kill / 10;
        result += Managers.GameManagerEx.GameLevel / 5;

        int timeBonus = 0;
        if(Managers.GameManagerEx.ProgressTime > 600.0f)
        {
            timeBonus = 600 / 20;
        }
        else
        {
            timeBonus = (int)Managers.GameManagerEx.ProgressTime / 20;
        }
        result += timeBonus;

        if (Managers.GameManagerEx.GameOverType == Define.GameOverType.Clear)
        {
            result *= 2;
        }
        Managers.DataManager.User.Data.gold += result;
        Managers.DataManager.User.UserDataOverwrite();

        yield return new WaitForSeconds(1.0f);

        Managers.SoundManager.PlaySFX("UISounds/BonusGold");
        UI_Get<Text>((int)Texts.KillBonusText).text = (Managers.GameManagerEx.Kill / 10).ToString();
        UI_Get<GameObject>((int)BonusPanels.KillBonusPanel).SetActive(true);
        yield return new WaitForSeconds(1.0f);

        Managers.SoundManager.PlaySFX("UISounds/BonusGold");
        UI_Get<Text>((int)Texts.TimeBonusText).text = timeBonus.ToString();
        UI_Get<GameObject>((int)BonusPanels.TimeBonusPanel).SetActive(true);
        yield return new WaitForSeconds(1.0f);

        Managers.SoundManager.PlaySFX("UISounds/BonusGold");
        UI_Get<Text>((int)Texts.LevelBonusText).text = (Managers.GameManagerEx.GameLevel / 5).ToString();
        UI_Get<GameObject>((int)BonusPanels.LevelBonusPanel).SetActive(true);
        yield return new WaitForSeconds(1.0f);

        if(Managers.GameManagerEx.GameOverType == Define.GameOverType.Clear)
        {
            Managers.SoundManager.PlaySFX("UISounds/BonusGold");
            UI_Get<GameObject>((int)BonusPanels.ClearPanel).SetActive(true);
            yield return new WaitForSeconds(1.0f);
        }

        Managers.SoundManager.PlaySFX("UISounds/ResultGold");
        UI_Get<Text>((int)Texts.ResultBonusText).text = result.ToString();
        UI_Get<GameObject>((int)BonusPanels.ResultPanel).SetActive(true);
        yield return new WaitForSeconds(1.0f);

        // TODO : 데이터 업데이트
        if(Managers.WebManager.IsLogin == true)
        {
            // return button 점수 계산 끝나고 생성
            IEnumerator coUpdate = Managers.WebManager.CoUpdateRequest("ranking", "Put", () =>
            {
                UI_Get<Button>((int)Buttons.ReturnButton).gameObject.SetActive(true);
            });

            StartCoroutine(coUpdate);
        }
        else
        {
            UI_Get<Button>((int)Buttons.ReturnButton).gameObject.SetActive(true);
        }
    }
}
