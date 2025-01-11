using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopUpUI_BoxOpen : UI_PopUp
{
    enum Images
    {
        BoxImage,
    }

    Vector3 _boxInitPos;

    [SerializeField]
    protected Material _whiteMaterial;

    Animator _anim;
    Image _boxImage;
    ParticleSystem _gatherEffect;
    ParticleSystem _spreadParticle;
    public override void Init()
    {
        base.Init();
        GetComponent<Canvas>().worldCamera = GameObject.Find("UIParticleCamera").GetComponent<Camera>();

        UI_Bind<Image>(typeof(Images));

        _boxImage = UI_Get<Image>((int)Images.BoxImage);
        _anim = _boxImage.GetComponent<Animator>();

        _gatherEffect = _boxImage.transform.Find("GatherParticle").GetComponent<ParticleSystem>();
        _gatherEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        _boxImage.gameObject.SetActive(false);
    }

    public override void Show(object param = null)
    {
        _whiteMaterial.SetFloat("_Amount", 0.0f);

        _boxInitPos = UI_Get<Image>((int)Images.BoxImage).rectTransform.position;

        IEnumerator coDrop = CoDropBox();
        StartCoroutine(coDrop);

        Managers.SoundManager.PlaySFX("UISounds/CharacterGet"); 
    }

    IEnumerator CoDropBox()
    {
        _boxImage.gameObject.SetActive(true);
        _anim.SetBool("IsDrop", true);

        while(true)
        {
            _gatherEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f &&
               _anim.GetCurrentAnimatorStateInfo(0).IsName("BoxDrop"))
            { 
                IEnumerator coVibration = CoVibrationBox();
                StartCoroutine(coVibration);
                break;
            }
            yield return null;
        }
    }

    IEnumerator CoVibrationBox()
    {
        float openTime = 2.0f;
        float processTime = 0.0f;
        float whiteValue = 0.0f;

        _gatherEffect.Play();
        while (true)
        {
            float x = Random.Range(-1.0f, 1.0f);
            float y = Random.Range(-1.0f, 1.0f);
            UI_Get<Image>((int)Images.BoxImage).rectTransform.position = 
                        new Vector3(_boxInitPos.x + x, _boxInitPos.y + y, _boxInitPos.z + 0.0f);

            float t = processTime / openTime;
            whiteValue = Mathf.Lerp(0.0f, 1.0f, t);
            _whiteMaterial.SetFloat("_Amount", whiteValue);

            if (processTime > openTime)
            {
                _anim.SetBool("IsDrop", false);
                _anim.SetBool("IsOpen", true);

                _gatherEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

                UI_Get<Image>((int)Images.BoxImage).rectTransform.position = _boxInitPos;
                _whiteMaterial.SetFloat("_Amount", 0.0f);

                yield return new WaitForSeconds(1.5f);

                Managers.UIManager.CloseCurPopUpUI(() =>
                {
                    _anim.SetBool("IsOpen", false);
                    _boxImage.gameObject.SetActive(false);
                    Managers.UIManager.ShowPopUpUI("PopUpUI_BoxSelect");
                });
                break;
            }

            processTime += Time.deltaTime;
            yield return null;
        }
    }
}
