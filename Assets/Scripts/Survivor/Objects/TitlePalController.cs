using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePalController : MonoBehaviour
{
    GameObject[] _ShowPals = new GameObject[(int)Define.MapType.End];
    float _accTime;
    float _changeTime;
    int _palIndex;

    private void Start()
    {
        _ShowPals[(int)Define.MapType.Field] = Managers.ResourceManager.Instantiate("Tiles/FieldPal");
        _ShowPals[(int)Define.MapType.Cave] = Managers.ResourceManager.Instantiate("Tiles/CavePal");

        foreach(GameObject pal in _ShowPals)
        {
            pal.transform.SetParent(transform);
            pal.SetActive(false);
        }

        _accTime = 0.0f;
        _changeTime = 5.0f;
        _palIndex = 0;

        _ShowPals[_palIndex].SetActive(true);
    }

    private void Update()
    {
        _accTime += Time.deltaTime;
        
        if(_accTime >= _changeTime)
        {
            _ShowPals[_palIndex].SetActive(false);

            _palIndex += 1;
            if(_palIndex >= _ShowPals.Length)
            {
                _palIndex = 0;
            }

            _ShowPals[_palIndex].SetActive(true);
            _accTime = 0.0f;
        }
    }
}
