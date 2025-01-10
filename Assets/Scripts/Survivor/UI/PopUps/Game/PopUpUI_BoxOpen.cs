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

    [SerializeField]
    Sprite closeImage;
    [SerializeField]
    Sprite openImage;

    Vector3 _boxInitPos;

    [SerializeField]
    protected Material _whiteMaterial;

    ParticleSystem _gatherEffect;
    ParticleSystem _spreadParticle;
    public override void Init()
    {
        base.Init();
        // GetComponent<Canvas>().worldCamera = GameObject.Find("UIParticleCamera").GetComponent<Camera>();

        UI_Bind<Image>(typeof(Images));

        Image boxImage = UI_Get<Image>((int)Images.BoxImage);

        _gatherEffect = boxImage.transform.Find("GatherParticle").GetComponent<ParticleSystem>();
        _gatherEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        _spreadParticle = boxImage.transform.Find("SpreadParticle").GetComponent<ParticleSystem>();
        _spreadParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    public override void Show(object param = null)
    {
        Time.timeScale = 1.0f;
        _whiteMaterial.SetFloat("_Amount", 0.0f);

        _boxInitPos = UI_Get<Image>((int)Images.BoxImage).rectTransform.position;
        UI_Get<Image>((int)Images.BoxImage).sprite = closeImage;

        IEnumerator coVibration = CoVibration();
        StartCoroutine(coVibration);

        Managers.SoundManager.PlaySFX("UISounds/CharacterGet"); 
    }

    IEnumerator CoVibration()
    {
        float openTime = 2.0f;
        float spreadTime = 1.0f;
        float processTime = 0.0f;
        float whiteValue = 0.0f;

        bool isSpread = false;

        _gatherEffect.Play();
        while (true)
        {
            float x = Random.Range(-2.0f, 2.0f);
            float y = Random.Range(-2.0f, 2.0f);
            UI_Get<Image>((int)Images.BoxImage).rectTransform.position = 
                        new Vector3(_boxInitPos.x + x, _boxInitPos.y + y);

            float t = processTime / openTime;
            whiteValue = Mathf.Lerp(0.0f, 1.0f, t);
            _whiteMaterial.SetFloat("_Amount", whiteValue);

            if(processTime > spreadTime && isSpread == false)
            {
                isSpread = true;
                _spreadParticle.Play();
            }

            if (processTime > openTime)
            {
                _gatherEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                _spreadParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

                UI_Get<Image>((int)Images.BoxImage).rectTransform.position = _boxInitPos;
                UI_Get<Image>((int)Images.BoxImage).sprite = openImage;
                _whiteMaterial.SetFloat("_Amount", 0.0f);

                yield return new WaitForSeconds(1.5f);

                Managers.UIManager.ClosePopUpUI("PopUpUI_BoxOpen");
                Managers.UIManager.ShowPopUpUI("PopUpUI_BoxSelect");
                break;
            }

            processTime += Time.deltaTime;
            spreadTime += Time.deltaTime;
            yield return null;
        }
    }
}
