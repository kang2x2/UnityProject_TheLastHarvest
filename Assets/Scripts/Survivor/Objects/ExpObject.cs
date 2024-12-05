using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpObject : MonoBehaviour
{
    enum ExpType { Bronze, Silver, Gold, End }

    public Sprite[] sprites;
    SpriteRenderer _sprite;
    bool _isSucked;
    float _speed;
    float _expValue;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        int ranIndex = Random.Range(0, 100);
        
        if(ranIndex <= 5)
        {
            _expValue = 15;
            _sprite.sprite = sprites[(int)ExpType.Gold];
        }
        else if(ranIndex <= 10)
        {
            _expValue = 8;
            _sprite.sprite = sprites[(int)ExpType.Silver];
        }
        else
        {
            _expValue = 3;
            _sprite.sprite = sprites[(int)ExpType.Bronze];
        }

        _isSucked = false;
        _speed = 0.0f;
    }

    private void Update()
    {
        if (Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }

        if (_isSucked == true)
        {
            _speed += 10.0f * Time.deltaTime;

            Vector3 dir = Managers.GameManagerEx.Player.transform.position - transform.position;
            transform.Translate(dir.normalized * _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Passive_Margnet")
        {
            _isSucked = true;
        }

        if (collision.gameObject.name == "Player")
        {
            Managers.GameManagerEx.GetExp(collision.GetComponent<Player>().GetExpRatio * _expValue);
            Managers.ResourceManager.Destroy(gameObject);
        }
    }
}
