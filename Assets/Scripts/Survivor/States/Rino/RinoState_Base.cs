using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RinoState_Base
{
    protected Boss_Rino _rino;
    protected Animator _rinoAnim;
    protected SpriteRenderer _rinoSprite;
    protected Collider2D _rinoCollider;
    protected Rigidbody2D _rinoRigid;

    public virtual void Init(Boss_Rino rino)
    {
        _rino = rino;
        _rinoAnim = rino.GetComponent<Animator>();
        _rinoSprite = rino.GetComponent<SpriteRenderer>();
        _rinoCollider = rino.GetComponent<Collider2D>();
        _rinoRigid = rino.GetComponent<Rigidbody2D>();
    }

    public abstract void Enter();

    public virtual void FixedUpdate()
    {

    }
    public virtual void Update()
    {

    }
    public virtual void LateUpdate()
    {

    }
    public abstract void Exit();
}
