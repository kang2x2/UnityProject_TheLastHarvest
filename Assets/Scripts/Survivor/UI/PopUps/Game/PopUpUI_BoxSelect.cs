using UnityEngine;
using UnityEngine.UI;

public class PopUpUI_BoxSelect : UI_PopUp
{
    enum Scrollbars
    {
        ScrollbarVertical,
    }
    enum GameObjects
    {
        Content,
    }

    public override void Init()
    {
        base.Init();

        UI_Bind<Scrollbar>(typeof(Scrollbars));
        UI_Bind<GameObject>(typeof(GameObjects));
    }

    public override void Show(object param = null)
    {
        UI_Get<Scrollbar>((int)Scrollbars.ScrollbarVertical).value = 1;

        Vector2 downPos = new Vector2(0.0f, -1000.0f);
        UI_Get<GameObject>((int)GameObjects.Content).GetComponent<RectTransform>().localPosition = downPos;

        Managers.SoundManager.PlaySFX("UISounds/LevelUp");
        Managers.GameManagerEx.Pause();

        Managers.ItemCardManager.ItemCardSuffle(2, UI_Get<GameObject>((int)GameObjects.Content).transform, ItemCardManager.SelectType.Box);

        Time.timeScale = 1.0f;
        Managers.GameManagerEx.LevelUpEffect.Play();
    }
}
