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
    float _risingPower;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _rigid = GetComponent<Rigidbody2D>();
        _risingPower = 4.0f;

        UI_Bind<Text>(typeof(Texts));
    }

    public void Init(Vector3 pos, float damage)
    {    
        UI_Get<Text>((int)Texts.DamageText).text = string.Format("{0:F0}", damage);
        _rect.position = pos;

        float ranX = Random.Range(-0.25f, 0.25f);
        Vector2 ranDir = new Vector2(ranX, 1.0f);
        _rigid.AddForce(ranDir.normalized * _risingPower, ForceMode2D.Impulse);
    }

    public void Delete_DamageText()
    {
        Managers.ResourceManager.Destroy(gameObject);
    }
}
