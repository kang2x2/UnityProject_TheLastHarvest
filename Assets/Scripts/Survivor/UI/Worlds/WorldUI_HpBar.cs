using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUI_HpBar : UI_Base
{
    RectTransform _rect;
    Slider _slider;
    Transform _owner;

    float _maxVal;
    float _curVal;

    float _accTime;
    float _disableTime;

    Vector3 _offset;

    public void Init(Transform owner, float maxVal)
    {
        _rect = GetComponent<RectTransform>();
        _slider = GetComponentInChildren<Slider>();

        if(owner == null)
        {
            Debug.Log("WorldHpBar Error. Owner Is Null.");
        }

        _owner = owner;
        transform.SetParent(_owner);

        _accTime = 0.0f;
        _disableTime = 2.0f;
        _offset = new Vector3(0.0f, 0.75f, 0.0f);

        _maxVal = maxVal;
        ValueInit(_maxVal);
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
