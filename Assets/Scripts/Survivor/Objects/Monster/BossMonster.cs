using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    public Data_Monster data;

    public float Hp { get; protected set; }
    public float MaxHp { get; protected set; }
    public float Attack { get; private set; }
    public bool IsLive { get; protected set; }

    protected SpriteRenderer _sprite;
    protected Animator _anim;
    protected Rigidbody2D _rigid;
    protected Collider2D _collider;

    protected GameObject _target = null;

    protected float speed;
    protected float _rePositionOffSet = 10.0f;

    private void Start()
    {
        Init();
    }

    public virtual void Init() { }

    protected void MonsterSetting(Data_Monster data, int gameLevel)
    {
        _anim = GetComponent<Animator>();
        _anim.runtimeAnimatorController = data.Animator;

        MaxHp = data.maxHp;
        Hp = MaxHp;//
        Hp = 10;
        Attack = data.attack;

        speed = data.speed;

        Vector3 playerPos = Managers.GameManagerEx.Player.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y + 10, playerPos.z);
    }
}
