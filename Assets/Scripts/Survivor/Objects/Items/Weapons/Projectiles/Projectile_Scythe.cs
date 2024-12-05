using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Scythe : Projectile
{
    float _offset;
    float _speed;
    float _angle;

    public void Init(float offset, float speed, float attack)
    {
        Attack = attack;
        Effect = EffectType.Slash;

        _offset = offset;
        _speed = speed;
    }

    private void Update()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }

        transform.Rotate(Vector3.back * _speed * 10.0f * Time.deltaTime);

        _angle += _speed / 40;

        if (_angle >= 360.0f)
        {
            transform.parent.GetComponent<Weapon_Scythe>().IsSpawn = true;
            Managers.ResourceManager.Destroy(gameObject);
        }

        Vector3 center = transform.parent.position;
        
        float destX = center.x + (_offset * Mathf.Cos(_angle * (Mathf.PI / 180.0f)));
        float destY = center.y - (_offset * Mathf.Sin(_angle * (Mathf.PI / 180.0f)));

        transform.position = new Vector2(destX, destY);
    }
}
