using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUI_DamageText : UI_Base
{
    enum Texts
    {
        DamageText,
    }

    RectTransform _rect;
    Rigidbody2D _rigid;
    Animator _anim;
    float _risingPower;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        UI_Bind<Text>(typeof(Texts));
    }

    public void Init(Vector3 pos, float damage, Define.DamageType type)
    {    
        switch(type)
        {
            case Define.DamageType.Normal:
                _risingPower = 4.0f;
                _rigid.gravityScale = 1.0f;
                _anim.SetTrigger("Normal");
                break;
            case Define.DamageType.Critical:
                _risingPower = 5.5f;
                _rigid.gravityScale = 2.0f;
                _anim.SetTrigger("Critical");
                break;
        }

        UI_Get<Text>((int)Texts.DamageText).text = string.Format("{0:F0}", damage);
        _rect.position = pos;

        float ranX = Random.Range(-0.25f, 0.25f);
        Vector2 ranDir = new Vector2(ranX, 1.0f);
        _rigid.AddForce(ranDir.normalized * _risingPower, ForceMode2D.Impulse);
    }

    private void LateUpdate()
    {
        if(Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            _rigid.velocity = Vector2.zero;
            _anim.speed = 0.0f;
            return;
        }

        _anim.speed = 1.0f;
    }

    public void Delete_DamageText()
    {
        Managers.ResourceManager.Destroy(gameObject);
    }
}
