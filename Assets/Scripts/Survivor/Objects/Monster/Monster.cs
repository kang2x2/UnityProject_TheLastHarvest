using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    float hp;
    float maxHp;
    float attack;
    float speed;

    GameObject _target = null;
    float _rePositionOffSet = 10.0f;
    float _knockBackPower = 3.0f;

    bool _isLive = true;

    SpriteRenderer _sprite;
    Animator _anim;
    Rigidbody2D _rigid;
    Collider2D _collider;

    WorldUI_HpBar _hpBar;
    
    [SerializeField]
    Data_Monster[] datas;
    public Data_Monster MonsterData { get; private set; } 

    void OnEnable()
    {
        _target = Managers.GameManagerEx.Player;
        _isLive = true;

        if(_sprite != null)
        {
            _sprite.sortingOrder = 2;
        }
        if (_collider != null)
        {
            _collider.enabled = true;
        }
        if(_rigid != null)
        {
            _rigid.simulated = true;
        }
    }

    public void Init(int index)
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        MonsterSetting(datas[index], Managers.GameManagerEx.GameLevel);

        _hpBar = Managers.ResourceManager.Instantiate("UI/Worlds/WorldUI_HpBar").GetComponent<WorldUI_HpBar>();
        _hpBar.Init(transform, new Vector3(0.0f, 0.75f, 0.0f), maxHp);
        _hpBar.gameObject.SetActive(false);
    }

    void MonsterSetting(Data_Monster data, int gameLevel)
    {
        MonsterData = data;

        _anim = GetComponent<Animator>();
        _anim.runtimeAnimatorController = data.Animator;

        if (gameLevel < 10)
        {
            maxHp = data.maxHp + (gameLevel * 1.1f);
            hp = maxHp;
            attack = data.attack + (gameLevel * 1.1f);
        }
        else
        {
            maxHp = data.maxHp + (gameLevel * 1.2f);
            hp = maxHp;
            attack = data.attack + (gameLevel * 1.2f);
        }

        speed = data.speed;
    }

    void FixedUpdate()
    {
        if(Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }

        if (Managers.Instance != null)
        {
            if(_isLive == false || _anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            {
                return;
            }    

            // 老馆 捞悼 规过
            // transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _moveSpeed * Time.fixedDeltaTime);

            // rigid 荤侩 规过
            Vector2 dir = _target.GetComponent<Rigidbody2D>().position - _rigid.position;
            Vector2 destPos = dir.normalized * speed * Time.fixedDeltaTime;
            _rigid.MovePosition(_rigid.position + destPos);
            _rigid.velocity = Vector2.zero;
        }
    }

    void LateUpdate()
    {
        if(Managers.GameManagerEx.IsPause == true || Managers.SceneManagerEx.IsLoading == true)
        {
            _anim.speed = 0.0f;
            if(Managers.SceneManagerEx.IsLoading == true)
            {
                _rigid.velocity = Vector2.zero;
            }
            return;
        }
        else
        {
            _anim.speed = 1.0f;
        }

        if (Managers.Instance != null && _isLive == true)
        {
            _sprite.flipX = transform.position.x > _target.transform.position.x;
        }
    }

    void Dead()
    {
        Managers.ResourceManager.Destroy(gameObject);
    }

    IEnumerator HitEvent()
    {
        yield return null; 
        Vector3 knockBackDir = transform.position - _target.transform.position;
        _rigid.AddForce(knockBackDir.normalized * _knockBackPower, ForceMode2D.Impulse);
    }

 #region Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerProjectile"))
        {
            hp -= collision.transform.GetComponent<Projectile>().Attack;
            StartCoroutine(HitEvent());

            if(collision.GetComponent<Projectile>().Effect == Projectile.EffectType.Bullet)
            {
                Managers.SoundManager.PlaySFX("Battles/BlowHit");
                GameObject effect = Managers.ResourceManager.Instantiate("Objects/BulletHitEffect");
                effect.transform.position = transform.position;
            }
            else if(collision.GetComponent<Projectile>().Effect == Projectile.EffectType.BigBullet)
            {
                Managers.SoundManager.PlaySFX("Battles/BlowHit");
                GameObject effect = Managers.ResourceManager.Instantiate("Objects/BigBulletHitEffect");
                effect.transform.position = transform.position;
            }
            else if(collision.GetComponent<Projectile>().Effect == Projectile.EffectType.Slash)
            {
                GameObject effect = Managers.ResourceManager.Instantiate("Objects/SlashHitEffect");
                effect.transform.position = transform.position;
            }
            else if (collision.GetComponent<Projectile>().Effect == Projectile.EffectType.Blow)
            {
                Managers.SoundManager.PlaySFX("Battles/BlowHit");
                GameObject effect = Managers.ResourceManager.Instantiate("Objects/BlowHitEffect");
                effect.transform.position = transform.position;
            }

            if (hp > 0)
            {
                _anim.SetTrigger("Hit");
                _hpBar.gameObject.SetActive(true);
                _hpBar.ValueInit(hp);
            }
            else
            {
                _isLive = false;
                _collider.enabled = false;
                _rigid.simulated = false;
                _sprite.sortingOrder = 1;
                _anim.SetBool("Dead", true);

                Managers.ResourceManager.Instantiate("Objects/ExpObject").transform.position = transform.position;
                Managers.GameManagerEx.Kill += 1;
                Managers.ResourceManager.Destroy(_hpBar.gameObject);
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerArea") && _isLive == true)
        {
            float randomAngle = Random.Range(0.0f, 360.0f);
            float randomRadian = randomAngle * Mathf.Deg2Rad;

            Vector3 ranDir = new Vector2(Mathf.Cos(randomRadian), Mathf.Sin(randomRadian));
            float monsterPosX = (_target.transform.position + (ranDir.normalized * (_rePositionOffSet / 2))).x;
            float monsterPosY = (_target.transform.position + (ranDir.normalized * _rePositionOffSet)).y;

            transform.position = new Vector2(monsterPosX, monsterPosY);
        }
    }

    #endregion
}
