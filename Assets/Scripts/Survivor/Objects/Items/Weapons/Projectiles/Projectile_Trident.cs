using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Trident : Projectile
{
    Rigidbody2D _rigid;
    Transform _parent;
    Vector3 _dir;

    int _bounce;
    float _speed;

    float _curLifeTime = 0.0f;
    float _maxLifeTime = 45.0f;

    public void Init(int bounce, float attack, float knockBackPower, Vector3 dir, Transform parent)
    {
        _rigid = GetComponent<Rigidbody2D>();
        Attack = attack;
        KnockBackPower = knockBackPower;
        AttackCount = 1;
        Effect = EffectType.Slash;

        _bounce = bounce;
        _dir = dir;
        _parent = parent;
        _speed = 10.0f;

        _rigid.velocity = dir * _speed;

        _curLifeTime = 0.0f;
        _maxLifeTime = 15.0f;
    }

    private void Update()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            _rigid.velocity = Vector2.zero;
            return;
        }

        if (_rigid.velocity.magnitude < _speed)
        {
            _rigid.velocity = _dir * _speed;
        }

        _curLifeTime += Time.deltaTime;
        if(_curLifeTime > _maxLifeTime)
        {
            _curLifeTime = 0.0f;
            _parent.GetComponent<Weapon_Trident>().IsSpawn = true;
            Managers.ResourceManager.Destroy(gameObject, 1.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall") == true)
        {
            if(_bounce > 0)
            {
                Managers.SoundManager.PlaySFX("weaponSounds/TridentWall");
                Managers.SoundManager.PlaySFX("weaponSounds/Trident");

                Vector3 normal = Vector2.zero;
                switch (collision.gameObject.name)
                {
                    case "WallLeft":
                        normal = Vector2.right;
                        break;
                    case "WallRight":
                        normal = Vector2.left;
                        break;
                    case "WallTop":
                        normal = Vector2.down;
                        break;
                    case "WallBottom":
                        normal = Vector2.up;
                        break;
                }

                float randomAngle = Random.Range(-45.0f, 45.0f);
                Vector3 reflectDir = Quaternion.Euler(0, 0, randomAngle) * normal;

                transform.rotation = Quaternion.FromToRotation(Vector3.up, reflectDir);

                _dir = reflectDir;
                _rigid.velocity = _dir * _speed;

                if (_rigid.velocity.magnitude > _speed)
                {
                    _rigid.velocity = _dir * _speed;
                }

                _bounce -= 1;
            }
            else
            {
                _parent.GetComponent<Weapon_Trident>().IsSpawn = true;
                Managers.ResourceManager.Destroy(gameObject, 1.0f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.name == "WallSet")
        {
            if(_bounce > 0)
            {
                Vector3 returnDir = Managers.GameManagerEx.Player.transform.position - transform.position;

                transform.rotation = Quaternion.FromToRotation(Vector3.up, returnDir.normalized);

                _dir = returnDir;
                _rigid.velocity = _dir * _speed;

                if (_rigid.velocity.magnitude > _speed)
                {
                    _rigid.velocity = _dir * _speed;
                }
            }
        }
    }
}
