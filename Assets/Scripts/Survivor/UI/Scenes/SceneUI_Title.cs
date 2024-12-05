using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneUI_Title : UI_Scene
{
    enum Buttons
    {
        StartButton,
        SettingButton,
        ExitButton,
    }

    private void Start()
    {
        UI_Bind<Button>(typeof(Buttons));
        UI_BindEvent(UI_Get<Button>((int)Buttons.StartButton).gameObject, ClickStartButton);
    }

    public void ClickStartButton(PointerEventData data)
    {
        Managers.UIManager.ShowPopUpUI("PopUpUI_SelectMap");
        // Managers.SceneManagerEx.ChangeScene(Define.SceneType.GameScene);
    }
}

