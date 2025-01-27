using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Field
    public float MoveSpeed { get; set; }
    public float AttackRatio { get; set; }
    public float GetExpRatio { get; set; }
    public float RecoveryRatio { get; set; }
    public float CriticalRatio { get; set; }
    public int SelectItemCount { get; set; }
    public int ReRollCount { get; set; }
    public float Hp { get; set; }
    public float MaxHp { get; set; }
    public Transform Margent { get; private set; }
    public bool[] HasItem { get; set; } = new bool[(int)Define.ItemName.End];

    public bool IsLive { get; private set; }

    Vector2 _curDir;

    SpriteRenderer _sprite;
    Rigidbody2D _rigid;
    Collider2D _collider;
    Animator _anim;

    ParticleSystem _hitEffect;
    ParticleSystem _healEffect;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        HasItem[(int)Define.ItemName.Gun] = true;
        HasItem[(int)Define.ItemName.Shoose] = true;
        HasItem[(int)Define.ItemName.Margent] = true;
        HasItem[(int)Define.ItemName.ExpBoost] = true;
        HasItem[(int)Define.ItemName.PowerCore] = true;
        HasItem[(int)Define.ItemName.MaxHp] = true;
        HasItem[(int)Define.ItemName.Recovery] = true;

        IsLive = true;

        _hitEffect = transform.Find("PlayerHitEffect").GetComponent<ParticleSystem>();
        _hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        _healEffect = transform.Find("HealEffect").GetComponent<ParticleSystem>();
        _healEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        //if (Application.platform == RuntimePlatform.Android ||
        //    Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    GetComponent<PlayerInput>().defaultActionMap = "Gamepad";
        //}
        //else
        //{
        //    GetComponent<PlayerInput>().defaultActionMap = "Keyboard&Mouse";
        //}
    }

    public void CharacterSetting(Data_Character data)
    {
        _anim = GetComponent<Animator>();
        _anim.runtimeAnimatorController = data.gameAnimator;

        MoveSpeed = data.speedBonus + Managers.DataManager.User.Data.bonus[(int)Define.UserStatType.MoveSpeed];
        AttackRatio = data.attackBonus + Managers.DataManager.User.Data.bonus[(int)Define.UserStatType.Attack];
        MaxHp = Managers.DataManager.User.Data.bonus[(int)Define.UserStatType.MaxHP];
        RecoveryRatio = Managers.DataManager.User.Data.bonus[(int)Define.UserStatType.Recovery];
        GetExpRatio = data.expBonus + Managers.DataManager.User.Data.bonus[(int)Define.UserStatType.Exp];
        CriticalRatio = Managers.DataManager.User.Data.bonus[(int)Define.UserStatType.Critical];
        Margent = transform.Find("Passive_Margnet");
        SelectItemCount = Managers.DataManager.User.Data.selectCount;
        ReRollCount = 3;

        foreach (Transform initItem in GetComponentsInChildren<Transform>())
        {
            if(initItem.GetComponent<Survivor_Item>() != null)
            {
                initItem.GetComponent<Survivor_Item>().Init();
            }
        }

        Hp = MaxHp;
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

        Vector2 nextVec = _curDir.normalized * MoveSpeed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + nextVec);

        // transform.Translate(_curDir.normalized * MoveSpeed * Time.fixedDeltaTime);
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

        if (_curDir.x != 0)
        {
            _sprite.flipX = _curDir.x < 0;
        }
        if(_curDir.magnitude <= 0)
        {
            _anim.SetBool("move", false);
        }

        if (IsLive == true && Hp < MaxHp && RecoveryRatio > 0.0f)
        {
            Hp += RecoveryRatio * Time.deltaTime;
            if (Hp > MaxHp)
            {
                Hp = MaxHp;
            }
        }    
    }

    public void PlayHealEffect()
    {
        _healEffect.Play();
    }

    IEnumerator Dead()
    {
        _sprite.color = Color.white;
        Weapon[] weapons = transform.GetComponentsInChildren<Weapon>();
        foreach(Weapon weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }

        float accTime = 0.0f;
        float pauseTime = 2.0f;
        
        while(true)
        {
            _hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            _rigid.velocity = Vector2.zero;
            _rigid.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

            accTime += Time.deltaTime;
            if(accTime > pauseTime)
            {
                Managers.GameManagerEx.GameOverType = Define.GameOverType.Dead;
                Managers.UIManager.ShowPopUpUI("PopUpUI_GameOver", null, UIManager.UIAnimationType.None);
                Managers.GameManagerEx.IsPause = true;
                break;
            }
            yield return null;
        }
    }

   //private void OnTriggerStay2D(Collider2D collision)
   //{
   //    if(collision.CompareTag("Enemy") == true)
   //    {
   //        if (Hp > 0)
   //        {
   //            _sprite.color = Color.red;
   //            _hitEffect.Play();
   //
   //            if (collision.gameObject.GetComponent<Monster>() != null)
   //            {
   //                Hp -= collision.gameObject.GetComponent<Monster>().Attack * Time.deltaTime;
   //            }
   //        }
   //        else
   //        {
   //            if (IsLive == true)
   //            {
   //                IsLive = false;
   //                _rigid.velocity = Vector2.zero;
   //                _sprite.sortingOrder = 5;
   //                _anim.SetBool("Dead", true);
   //                Hp = 0.0f;
   //
   //                _hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
   //
   //                StartCoroutine(Dead());
   //            }
   //        }
   //    }
   //}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Enemy") == true)
        {
            if(Managers.GameManagerEx.IsPause == true)
            {
                return;
            }

            if (Hp > 0)
            {
                _sprite.color = Color.red;
                _hitEffect.Play();

                if (collision.gameObject.GetComponent<Monster>() != null)
                {
                    Hp -= collision.gameObject.GetComponent<Monster>().Attack * Time.deltaTime;
                }
            }
            else
            {
                if (IsLive == true)
                {
                    IsLive = false;
                    _rigid.velocity = Vector2.zero;
                    _sprite.sortingOrder = 5;
                    _anim.SetBool("Dead", true);
                    Hp = 0.0f;

                    _hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

                    StartCoroutine(Dead());
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") == true)
        {
            _hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            _sprite.color = Color.white;
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

    private void OnMove(InputValue value)
    {
        _anim.SetBool("move", true);
        _curDir = value.Get<Vector2>();
    }
}
