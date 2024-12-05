using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUI_HpBar : UI_Base
{
    float _maxVal;
    float _curVal;

    float _accTime = 0.0f;
    float _disableTime = 2.0f;

    RectTransform _rect;
    Slider _slider;
    Transform _owner;
    Vector3 _offset;

    public void Init(Transform owner, Vector3 offset, float maxVal)
    {
        _rect = GetComponent<RectTransform>();
        _slider = GetComponentInChildren<Slider>();

        _owner = owner;
        transform.SetParent(_owner);

        _offset = offset;
        _maxVal = maxVal;
    }

    public void ValueInit(float curVal)
    {
        _accTime = 0.0f;
        _curVal = curVal;
    }

    private void FixedUpdate()
    {
        _rect.position = _owner.position + _offset;
    }

    private void Update()
    {
        _accTime += Time.deltaTime;
        if(_accTime > _disableTime)
        {
            gameObject.SetActive(false);
            _accTime = 0.0f;
        }
    }

    private void LateUpdate()
    {
        _slider.value = _curVal / _maxVal;
    }
}
