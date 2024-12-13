using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    float _hp;
    float _maxHp;
    float _speed;

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

    public float Attack { get; private set; }

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
        _hpBar.Init(transform, _maxHp);
        _hpBar.gameObject.SetActive(false);
    }

    void MonsterSetting(Data_Monster data, int gameLevel)
    {
        _anim = GetComponent<Animator>();
        _anim.runtimeAnimatorController = data.Animator;

        if (gameLevel < 10)
        {
            _maxHp = data.maxHp + (gameLevel * 1.1f);
            _hp = _maxHp;
            Attack = data.attack + (gameLevel * 1.1f);
        }
        else
        {
            _maxHp = data.maxHp + (gameLevel * 1.2f);
            _hp = _maxHp;
            Attack = data.attack + (gameLevel * 1.2f);
        }

        _speed = data.speed;
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
            Vector2 destPos = dir.normalized * _speed * Time.fixedDeltaTime;
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

        if(Managers.GameManagerEx.IsClear == true)
        {
            Managers.ResourceManager.Instantiate("Objects/ExpObject").transform.position = transform.position;
            Managers.ResourceManager.Destroy(_hpBar.gameObject);

            _isLive = false;
            _collider.enabled = false;
            _rigid.simulated = false;
            _sprite.sortingOrder = 1;
            _anim.SetBool("Dead", true);
        }
    }

    void Dead()
    {
        _collider.enabled = false;
        Managers.ResourceManager.Destroy(gameObject);
    }

    IEnumerator HitEvent(Collider2D collision)
    {
        Vector3 knockBackDir = transform.position - _target.transform.position;
        _rigid.AddForce(knockBackDir.normalized * _knockBackPower, ForceMode2D.Impulse);

        Projectile projectile = collision.transform.GetComponent<Projectile>();
        for (int i = 0; i < projectile.AttackCount; ++i)
        {
            _hp -= collision.transform.GetComponent<Projectile>().Attack;

            if (collision.GetComponent<Projectile>().Effect == Projectile.EffectType.Bullet)
            {
                Managers.SoundManager.PlaySFX("Battles/BlowHit");
                GameObject effect = Managers.ResourceManager.Instantiate("Objects/BulletHitEffect");
                effect.transform.position = transform.position;
            }
            else if (collision.GetComponent<Projectile>().Effect == Projectile.EffectType.BigBullet)
            {
                Managers.SoundManager.PlaySFX("Battles/BlowHit");
                GameObject effect = Managers.ResourceManager.Instantiate("Objects/BigBulletHitEffect");
                effect.transform.position = transform.position;
            }
            else if (collision.GetComponent<Projectile>().Effect == Projectile.EffectType.Slash)
            {
                GameObject effect = Managers.ResourceManager.Instantiate("Objects/SlashHitEffect");
                effect.transform.position = transform.position;
            }
            else if (collision.GetComponent<Projectile>().Effect == Projectile.EffectType.Blow)
            {
                Managers.SoundManager.PlaySFX("Battles/BlowHit");
                GameObject effect = Managers.ResourceManager.Instantiate("Objects/SlashHitEffect");
                // GameObject effect = Managers.ResourceManager.Instantiate("Objects/BlowHitEffect");
                effect.transform.position = transform.position;
            }

            if (_hp > 0)
            {
                _anim.SetTrigger("Hit");
                _hpBar.gameObject.SetActive(true);
                _hpBar.ValueInit(_hp);
            }
            else
            {
                Managers.ResourceManager.Instantiate("Objects/ExpObject").transform.position = transform.position;
                Managers.GameManagerEx.Kill += 1;
                Managers.ResourceManager.Destroy(_hpBar.gameObject);

                _isLive = false;
                _collider.isTrigger = true;
                _rigid.simulated = false;
                _sprite.sortingOrder = 1;
                _anim.SetBool("Dead", true);
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

 #region Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerProjectile"))
        {
            StartCoroutine(HitEvent(collision));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerArea") && _isLive == true)
        {
            if(Managers.GameManagerEx.Player.GetComponent<Player>().IsLive == true)
            {
                float randomAngle = Random.Range(0.0f, 360.0f);
                float randomRadian = randomAngle * Mathf.Deg2Rad;

                Vector3 ranDir = new Vector2(Mathf.Cos(randomRadian), Mathf.Sin(randomRadian));
                float monsterPosX = (_target.transform.position + (ranDir.normalized * (_rePositionOffSet / 2))).x;
                float monsterPosY = (_target.transform.position + (ranDir.normalized * _rePositionOffSet)).y;

                transform.position = new Vector2(monsterPosX, monsterPosY);
            }
        }
    }

    #endregion
}
