using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PopUpUI_Clear : UI_PopUp
{
    enum Texts
    {
        KillCountText,
        TimeValueText,
        LevelValueText,
    }

    enum Buttons
    {
        ReturnButton,
    }

    public override void Init()
    {
        UI_Bind<Text>(typeof(Texts));
        UI_Bind<Button>(typeof(Buttons));

        UI_BindEvent(UI_Get<Button>((int)Buttons.ReturnButton).gameObject, ReturnTitle);
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
}
