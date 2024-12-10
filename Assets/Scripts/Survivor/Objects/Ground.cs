using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    float _rePositionOffSet;

    private void Start()
    {
        _rePositionOffSet = 20.0f;
    }

    private void LateUpdate()
    {
        if (Managers.GameManagerEx.Player != null)
        {
            Vector3 playerPos = Managers.GameManagerEx.Player.transform.position;

            float diffX = playerPos.x - transform.position.x;
            float diffY = playerPos.y - transform.position.y;

            if(Mathf.Abs(diffX) > _rePositionOffSet)
            {
                float dirX = diffX > 0 ? 1 : -1;
                diffX = Mathf.Abs(diffX);
                transform.Translate(Vector3.right * dirX * (_rePositionOffSet * 2.0f));
            }

            if (Mathf.Abs(diffY) > _rePositionOffSet)
            {
                float dirY = diffY > 0 ? 1 : -1;
                diffY = Mathf.Abs(diffY);
                transform.Translate(Vector3.up * dirY * (_rePositionOffSet * 2.0f));
            }
        }
    }
}
