using UnityEngine;

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
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true || 
            IsLive == false)
        {
            _rigid.velocity = Vector2.zero;

            return false;
        }

        return true;
    }

    protected bool LateStopCheck()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            _anim.speed = 0.0f;
            _rigid.velocity = Vector2.zero;
            return false;
        }

        _anim.speed = 1.0f;
        return true;
    }

    protected void MonsterHit(Projectile projectile)
    {
        float playerAtkRatio = Managers.GameManagerEx.Player.GetComponent<Player>().AttackRatio;
        float weaponAtkRatio = Random.Range(0.9f, 1.1f);

        float weaponDamage = weaponAtkRatio * projectile.Attack;
        float damage = playerAtkRatio * weaponDamage;

        float criticalRatio = Random.Range(0.0f, 101.0f);
        if(criticalRatio > Managers.GameManagerEx.Player.GetComponent<Player>().CriticalRatio)
        {
            WorldUI_DamageText damageText = Managers.ResourceManager.
                Instantiate("UI/Worlds/WorldUI_DamageText").GetComponent<WorldUI_DamageText>();
            damageText.Init(transform.position, damage, Define.DamageType.Normal);
        }
        else
        {
            damage *= 1.3f;

            WorldUI_DamageText damageText = Managers.ResourceManager.
                Instantiate("UI/Worlds/WorldUI_DamageText").GetComponent<WorldUI_DamageText>();
            damageText.Init(transform.position, damage, Define.DamageType.Critical);
        }

        Hp -= damage;

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
