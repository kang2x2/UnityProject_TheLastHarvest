using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public float Hp { get; protected set; }
    public float MaxHp { get; protected set; }
    public float Attack { get; protected set; }
    public float Speed { get; protected set; }
    public bool IsLive { get; protected set; }

    protected SpriteRenderer _sprite;
    protected Animator _anim;
    protected Rigidbody2D _rigid;
    protected Collider2D _collider;

    protected GameObject _target = null;

    protected void FieldInit()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        _target = Managers.GameManagerEx.Player;
        IsLive = true;
    }

    protected bool FixedStopCheck()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return false;
        }

        if (IsLive == false)
        {
            _rigid.velocity = Vector2.zero;
            return false;
        }

        // _rigid.velocity = Vector2.zero;
        return true;
    }

    protected bool LateStopCheck()
    {
        if (Managers.GameManagerEx.IsPause == true)
        {
            _anim.speed = 0.0f;
            if (Managers.GameManagerEx.Player.GetComponent<Player>().IsLive == false)
            {
                _rigid.velocity = Vector2.zero;
            }
            return false;
        }

        if (Managers.SceneManagerEx.IsLoading == true)
        {
            _anim.speed = 0.0f;
            _rigid.velocity = Vector2.zero;
        }

        return true;
    }

    protected void MonsterHit(Projectile projectile)
    {
        WorldUI_DamageText damageText = Managers.ResourceManager.
            Instantiate("UI/Worlds/WorldUI_DamageText").GetComponent<WorldUI_DamageText>();
        damageText.Init(transform.position, projectile.Attack);

        Hp -= projectile.Attack;

        if (projectile.Effect == Projectile.EffectType.Bullet)
        {
            GameObject effect = Managers.ResourceManager.Instantiate("Objects/BulletHitEffect");
            effect.transform.position = transform.position;
        }
        else if (projectile.Effect == Projectile.EffectType.BigBullet)
        {
            GameObject effect = Managers.ResourceManager.Instantiate("Objects/BigBulletHitEffect");
            effect.transform.position = transform.position;
        }
        else if (projectile.Effect == Projectile.EffectType.Slash)
        {
            GameObject effect = Managers.ResourceManager.Instantiate("Objects/SlashHitEffect");
            effect.transform.position = transform.position;
        }
        else if (projectile.Effect == Projectile.EffectType.Blow)
        {
            GameObject effect = Managers.ResourceManager.Instantiate("Objects/SlashHitEffect");
            effect.transform.position = transform.position;
        }
    }
}
