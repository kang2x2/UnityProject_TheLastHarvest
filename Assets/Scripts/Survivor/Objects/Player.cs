using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Field
    public float MoveSpeed { get; set; } = 1.5f;
    public float AttackRatio { get; set; } = 1.0f;
    public float GetExpRatio { get; set; } = 1.0f;
    public int SelectBoxCount { get; set; }
    public float Hp { get; set; }
    public float MaxHp { get; set; }
    public bool[] HasItem { get; set; } = new bool[(int)Define.ItemType.End];

    public bool IsLive { get; private set; }

    Vector2 _curDir;

    SpriteRenderer _sprite;
    Rigidbody2D _rigid;
    Collider2D _collider;
    Animator _anim;

    ParticleSystem _hitEffect;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        HasItem[(int)Define.ItemType.Gun] = true;
        HasItem[(int)Define.ItemType.Shoose] = true;
        HasItem[(int)Define.ItemType.Margent] = true;
        HasItem[(int)Define.ItemType.ExpBoost] = true;
        HasItem[(int)Define.ItemType.BoxUpgrade] = true;
        HasItem[(int)Define.ItemType.PowerCore] = true;

        SelectBoxCount = 3;
        MaxHp = 20.0f;
        Hp = MaxHp;
        IsLive = true;

        _hitEffect = transform.Find("PlayerHitEffect").GetComponent<ParticleSystem>();
        _hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    public void CharacterSetting(Data_Character data)
    {
        _anim = GetComponent<Animator>();
        _anim.runtimeAnimatorController = data.gameAnimator;

        MoveSpeed += data.speedBonus;
        AttackRatio += data.attackBonus;
        GetExpRatio += data.expBonus;
    }

    private void FixedUpdate()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }

        if(IsLive == false)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            _anim.SetBool("move", true);

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _curDir = mousePos - new Vector2(transform.position.x, transform.position.y);

            transform.Translate(_curDir.normalized * MoveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            _anim.SetBool("move", false);
        }

        _rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            _anim.speed = 0.0f;
            return;
        }
        else
        {
            _anim.speed = 1.0f;
        }
        

        if (_curDir.normalized.x < 0)
        {
            _sprite.flipX = true;
        }
        else
        {
            _sprite.flipX = false;
        }
    }

    IEnumerator Dead()
    {
        Weapon[] weapons = transform.GetComponentsInChildren<Weapon>();
        foreach(Weapon weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }

        float accTime = 0.0f;
        float pauseTime = 2.0f;
        
        while(true)
        {
            _rigid.velocity = Vector2.zero;
            _rigid.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

            accTime += Time.deltaTime;
            if(accTime > pauseTime)
            {
                Managers.UIManager.ShowPopUpUI("PopUpUI_Dead");
                Managers.GameManagerEx.Pause();
                break;
            }
            yield return null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Enemy") == true)
        {
            if(Hp > 0)
            {
                _sprite.color = Color.red;
                _hitEffect.Play();

                if (collision.gameObject.GetComponent<Monster>() != null)
                {
                    Hp -= collision.gameObject.GetComponent<Monster>().Attack * Time.deltaTime;
                }

                else if (collision.gameObject.GetComponent<BossMonster>() != null)
                {
                    Hp -= collision.gameObject.GetComponent<BossMonster>().Attack * Time.deltaTime;
                }
            }
            else
            {
                if(IsLive == true)
                {
                    _hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

                    IsLive = false;
                    _rigid.velocity = Vector2.zero;
                    _sprite.sortingOrder = 5;
                    _anim.SetBool("Dead", true);

                    StartCoroutine(Dead());
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") == true)
        {
            _hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            _sprite.color = Color.white;
        }
    }
}
