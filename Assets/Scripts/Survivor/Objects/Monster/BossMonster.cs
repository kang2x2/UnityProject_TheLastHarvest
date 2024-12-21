using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : Monster
{
    public Data_Monster data;
    protected float _rePositionOffSet = 10.0f;

    protected Material _defaultMaterial;
    [SerializeField]
    protected Material _whiteMaterial;

    float _flashTime;
    bool _isFlashing;
    private void Start()
    {
        Init();
    }

    public virtual void Init() 
    {
        FieldInit();

        _defaultMaterial = _sprite.material;
        _flashTime = 0.1f;
        _isFlashing = false;

        MonsterSetting(data);
    }

    protected void MonsterSetting(Data_Monster data)
    {
        _anim = GetComponent<Animator>();
        _anim.runtimeAnimatorController = data.Animator;

        MaxHp = data.maxHp;
        Hp = MaxHp;
        Attack = data.attack;

        Speed = data.speed;

        Vector3 playerPos = Managers.GameManagerEx.Player.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y + 10, playerPos.z);
    }
    void FixedUpdate()
    {
        if (FixedStopCheck() == false)
        {
            return;
        }
        Vector2 dir = _target.GetComponent<Rigidbody2D>().position - _rigid.position;
        Vector2 destPos = dir.normalized * Speed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + destPos);
    }
    void LateUpdate()
    {
        if (LateStopCheck() == false)
        {
            return;
        }

        if (Managers.Instance != null && IsLive == true)
        {
            _sprite.flipX = transform.position.x < _target.transform.position.x;
        }

        if(_isFlashing == true)
        {
            _sprite.material = _whiteMaterial;
            _flashTime -= Time.deltaTime;
            if(_flashTime < 0)
            {
                _sprite.material = _defaultMaterial;
                _isFlashing = false;
                _flashTime = 0.1f;
            }
        }
    }

    IEnumerator HitEvent(Collider2D collision)
    {
        Projectile projectile = collision.transform.GetComponent<Projectile>();
        for (int i = 0; i < projectile.AttackCount; ++i)
        {
            MonsterHit(projectile);

            if (Hp > 0)
            {
                _isFlashing = true;
                _flashTime = 0.1f;
                Managers.SoundManager.PlaySFX("Battles/BlowHit");
            }
            else
            {
                Managers.SoundManager.PlaySFX("Battles/Dead");
                IsLive = false;
                _collider.isTrigger = true;
                _rigid.simulated = false;
                _sprite.sortingOrder = 1;
                _anim.SetBool("Dead", true);
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    #region Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerProjectile"))
        {
            _isFlashing = true;
            _flashTime = 0.1f;
            StartCoroutine(HitEvent(collision));
        }
    }
    #endregion
}

