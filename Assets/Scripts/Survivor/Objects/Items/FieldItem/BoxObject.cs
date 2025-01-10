using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Managers.GameManagerEx.Pause();
            Managers.GameManagerEx.BoxCount = 0;
            Managers.UIManager.ShowPopUpUI("PopUpUI_BoxOpen");
            Managers.ResourceManager.Destroy(gameObject);
        }
    }
}
