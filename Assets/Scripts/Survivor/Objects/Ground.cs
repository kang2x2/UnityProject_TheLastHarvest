using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerArea") == false)
        {
            return;
        }
        
        if(Managers.Instance != null)
        {
            GameObject player = Managers.GameManagerEx.Player;

            if(Camera.main != null)
            {
                // Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // Vector2 playerDir = mousePos - new Vector2(player.transform.position.x, player.transform.position.y);

                Vector3 playerPos = Managers.GameManagerEx.Player.transform.position;

                float diffX = playerPos.x - transform.position.x;
                float diffY = playerPos.y - transform.position.y;

                float dirX = diffX > 0 ? 1 : -1;
                float dirY = diffY > 0 ? 1 : -1;

                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 32);
                }
                else if(diffY > diffX)
                {
                    transform.Translate(Vector3.up * dirY * 32);
                }
            }
        }
    }
}
