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

    Vector2 _curDir;

    SpriteRenderer _sprite;
    Rigidbody2D _rigid;
    Animator _anim;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();

        HasItem[(int)Define.ItemType.Gun] = true;
        HasItem[(int)Define.ItemType.Shoose] = true;
        HasItem[(int)Define.ItemType.Margent] = true;
        HasItem[(int)Define.ItemType.ExpBoost] = true;
        HasItem[(int)Define.ItemType.BoxUpgrade] = true;
        HasItem[(int)Define.ItemType.PowerCore] = true;

        SelectBoxCount = 3;
        MaxHp = 100.0f;
        Hp = MaxHp;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Enemy") == true)
        {
            Hp -= collision.gameObject.GetComponent<Monster>().MonsterData.attack * Time.deltaTime;
        }
    }
}
