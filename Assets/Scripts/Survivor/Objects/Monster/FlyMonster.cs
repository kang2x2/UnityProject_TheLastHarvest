using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMonster : Monster
{
    bool _isClearEvent;

    bool _isKnockBack;
    float _knockBackAccTime;
    float _knockBackTime;


    WorldUI_HpBar _hpBar;

    [SerializeField]
    Data_Monster[] datas;

    Vector3 _dir;

    void OnEnable()
    {
        if (_sprite != null)
        {
            _sprite.sortingOrder = 4;
        }
        if (_collider != null)
        {
            _collider.enabled = true;
            _collider.isTrigger = false;
        }
        if (_rigid != null)
        {
            _rigid.simulated = true;
        }
    }

    public void Init(int index, Vector3 dir)
    {
        FieldInit();

        _dir = dir;
        _knockBackAccTime = 0.0f;
        _knockBackTime = 0.1f;

        _sprite.sortingOrder = 7;
        _isClearEvent = false;
        _isKnockBack = false;

        MonsterSetting(datas[index], Managers.GameManagerEx.GameLevel);

        _hpBar = Managers.ResourceManager.Instantiate("UI/Worlds/WorldUI_HpBar").GetComponent<WorldUI_HpBar>();
        _hpBar.Init(transform, MaxHp);
        _hpBar.gameObject.SetActive(false);
        if (_hpBar == null)
        {
            Debug.Log("HpBar Instantiate Fail");
        }

        _sprite.flipX = dir.x >= 0.0f;
    }

    void MonsterSetting(Data_Monster data, int gameLevel)
    {
        _anim = GetComponent<Animator>();
        _anim.runtimeAnimatorController = data.Animator;

        if (gameLevel < 10)
        {
            MaxHp = data.maxHp + (gameLevel * 1.1f);
            Hp = MaxHp;
            Attack = data.attack + (gameLevel * 1.1f);
        }
        else
        {
            MaxHp = data.maxHp + (gameLevel * 1.15f);
            Hp = MaxHp;
            Attack = data.attack + (gameLevel * 1.15f);
        }

        Speed = data.speed;
    }
    void FixedUpdate()
    {
        if (FixedStopCheck() == false)
        {
            return;
        }
        if (_isKnockBack == true)
        {
            _knockBackAccTime += Time.fixedDeltaTime;

            if (_knockBackAccTime > _knockBackTime)
            {
                _rigid.velocity = Vector2.zero;
                _knockBackAccTime = 0.0f;
                _isKnockBack = false;
            }
        }

        if (_isKnockBack == false)
        {
            Vector2 destPos = _dir.normalized * Speed * Time.fixedDeltaTime;
            _rigid.MovePosition(_rigid.position + destPos);
        }
    }

    void LateUpdate()
    {
        if (LateStopCheck() == false)
        {
            return;
        }

        _anim.speed = 1.0f;

        if (Managers.GameManagerEx.IsClear == true)
        {
            if (_isClearEvent == false)
            {
                Managers.SoundManager.PlaySFX("Battles/Dead");
                Managers.ResourceManager.Instantiate("Objects/ExpObject").transform.position = transform.position;
                Managers.ResourceManager.Destroy(_hpBar.gameObject);
                _isClearEvent = true;
            }

            IsLive = false;
            _collider.enabled = false;
            _rigid.simulated = false;
            _sprite.sortingOrder = 1;
            _anim.SetBool("Dead", true);
        }
    }

    void Dead()
    {
        _collider.enabled = false;
        Managers.ResourceManager.Destroy(_hpBar.gameObject);
        Managers.ResourceManager.Destroy(gameObject);
    }

    IEnumerator HitEvent(Collider2D collision)
    {
        Projectile projectile = collision.transform.GetComponent<Projectile>();
        for (int i = 0; i < projectile.AttackCount; ++i)
        {
            MonsterHit(projectile);

            _rigid.velocity = Vector2.zero;
            _isKnockBack = true;
            Vector3 knockBackDir = transform.position - _target.transform.position;
            _rigid.AddForce(knockBackDir.normalized * projectile.KnockBackPower, ForceMode2D.Impulse);

            if (Hp > 0)
            {
                Managers.SoundManager.PlaySFX("Battles/BlowHit");
                _anim.SetTrigger("Hit");
                _hpBar.gameObject.SetActive(true);
                _hpBar.ValueInit(Hp);
            }
            else
            {
                Managers.SoundManager.PlaySFX("Battles/Dead");

                GameObject exp = Managers.ResourceManager.Instantiate("Objects/ExpObject");
                exp.GetComponent<ExpObject>().Init();
                exp.transform.position = transform.position;

                _hpBar.gameObject.SetActive(false);
                Managers.GameManagerEx.Kill += 1;

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
            StartCoroutine(HitEvent(collision));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerArea"))
        {
            IsLive = false;
        }
    }

    #endregion
}
