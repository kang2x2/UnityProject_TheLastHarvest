using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPackObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Managers.SoundManager.PlaySFX("Battles/Heal");

            Player player = Managers.GameManagerEx.Player.GetComponent<Player>();
            player.Hp = player.MaxHp;
            player.PlayHealEffect();
            Managers.ResourceManager.Destroy(gameObject);
        }
    }
}
