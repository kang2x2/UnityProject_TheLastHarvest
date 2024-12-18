using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Rino : BossMonster
{
    public override void Init()
    {
        base.Init();
    }

    void FixedUpdate()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }

        if (Managers.Instance != null)
        {
            if (IsLive == false || _anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            {
                return;
            }

            Vector2 dir = _target.GetComponent<Rigidbody2D>().position - _rigid.position;
            Vector2 destPos = dir.normalized * speed * Time.fixedDeltaTime;
            _rigid.MovePosition(_rigid.position + destPos);
            _rigid.velocity = Vector2.zero;
        }
    }
}

