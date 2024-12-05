using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Rino : MonoBehaviour
{
    Animator _anim;
    SpriteRenderer _sprite;
    Rigidbody2D _rigid;
    Collider2D _collider;

    StateManager_Rino _stateManager;
    void Start()
    {
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        StartCoroutine(Opening());
    }

    IEnumerator Opening()
    {
        Vector2 destPos = Camera.main.transform.position;
        transform.position = new Vector3(destPos.x, destPos.y + 10);

        _anim.Play("Rino_Run");
        _anim.speed = 2.5f;

        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, destPos, 5.0f * Time.deltaTime);
            float dist = Vector2.Distance(transform.position, destPos);

            if (dist <= 1.0f)
            {
                _anim.Play("Rino_Break");
                _anim.speed = 1.0f;
                break;
            }

            yield return null;
        }

        while(true)
        {
            transform.position = Vector2.MoveTowards(transform.position, destPos, 1.0f * Time.deltaTime);
            float dist = Vector2.Distance(transform.position, destPos);

            if (dist <= 0.02f)
            {
                _stateManager = new StateManager_Rino();
                _stateManager.Init(this);
                _stateManager.ChangeState((int)StateManager_Rino.RinoState.Idle);
                break;
            }

            yield return null;
        }

        yield return true;
    }

    private void FixedUpdate()
    {
        if (_stateManager != null)
        {
            _stateManager.FixedUpdate();
        }
    }
    void Update()
    {
        if (_stateManager != null)
        {
            _stateManager.Update();
        }
    }
    private void LateUpdate()
    {
        if (_stateManager != null)
        {
            _stateManager.LateUpdate();
        }
    }
}
