using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Rino : BossMonster
{
    public override void Init()
    {
        _target = Managers.GameManagerEx.Player;
        IsLive = true;

        _sprite = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        MonsterSetting(data, Managers.GameManagerEx.GameLevel);
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

    void LateUpdate()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            _anim.speed = 0.0f;
            if (Managers.SceneManagerEx.IsLoading == true)
            {
                _rigid.velocity = Vector2.zero;
            }
            return;
        }
        else
        {
            _anim.speed = 1.0f;
        }

        if (Managers.Instance != null && IsLive == true)
        {
            _sprite.flipX = transform.position.x < _target.transform.position.x;
        }
    }

    #region Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerProjectile"))
        {
            Hp -= collision.transform.GetComponent<Projectile>().Attack;

            if (collision.GetComponent<Projectile>().Effect == Projectile.EffectType.Bullet)
            {
                Managers.SoundManager.PlaySFX("Battles/BlowHit");
                GameObject effect = Managers.ResourceManager.Instantiate("Objects/BulletHitEffect");
                effect.transform.position = transform.position;
            }
            else if (collision.GetComponent<Projectile>().Effect == Projectile.EffectType.BigBullet)
            {
                Managers.SoundManager.PlaySFX("Battles/BlowHit");
                GameObject effect = Managers.ResourceManager.Instantiate("Objects/BigBulletHitEffect");
                effect.transform.position = transform.position;
            }
            else if (collision.GetComponent<Projectile>().Effect == Projectile.EffectType.Slash)
            {
                GameObject effect = Managers.ResourceManager.Instantiate("Objects/SlashHitEffect");
                effect.transform.position = transform.position;
            }
            else if (collision.GetComponent<Projectile>().Effect == Projectile.EffectType.Blow)
            {
                Managers.SoundManager.PlaySFX("Battles/BlowHit");
                GameObject effect = Managers.ResourceManager.Instantiate("Objects/BlowHitEffect");
                effect.transform.position = transform.position;
            }

            if (Hp > 0)
            {
                _anim.SetTrigger("Hit");
            }
            else
            {
                IsLive = false;
                _collider.enabled = false;
                _rigid.simulated = false;
                _sprite.sortingOrder = 1;
                _anim.SetBool("Dead", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerArea") && IsLive == true)
        {
            if (Managers.GameManagerEx.Player.GetComponent<Player>().IsLive == true)
            {
                float randomAngle = Random.Range(0.0f, 360.0f);
                float randomRadian = randomAngle * Mathf.Deg2Rad;

                Vector3 ranDir = new Vector2(Mathf.Cos(randomRadian), Mathf.Sin(randomRadian));
                float monsterPosX = (_target.transform.position + (ranDir.normalized * (_rePositionOffSet / 2))).x;
                float monsterPosY = (_target.transform.position + (ranDir.normalized * _rePositionOffSet)).y;

                transform.position = new Vector2(monsterPosX, monsterPosY);
            }
        }
    }
    #endregion
}
