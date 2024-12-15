using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _uiChilds = new Dictionary<Type, UnityEngine.Object[]>();

    public virtual void Init() { }

    public virtual void Show(object param = null) { }

    protected void UI_Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] findObjects = new UnityEngine.Object[names.Length];
        _uiChilds.Add(typeof(T), findObjects);

        for(int i = 0; i < findObjects.Length; ++i)
        {
            findObjects[i] = UI_FindChild<T>(names[i]);
        }
    }

    protected void UI_BindEvent(GameObject obj, Action<PointerEventData> action, Define.UIEvent eventType = Define.UIEvent.Click)
    {
        UI_Event uiEvent = obj.GetComponent<UI_Event>();
        if(uiEvent == null)
        {
            uiEvent = obj.AddComponent<UI_Event>();
        }

        switch(eventType)
        {
            case Define.UIEvent.Click:
                uiEvent.OnClickHandler -= action;
                uiEvent.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                uiEvent.OnDragHandler -= action;
                uiEvent.OnDragHandler += action;
                break;
        }
    }

    protected T UI_Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects;
       
        if(_uiChilds.TryGetValue(typeof(T), out objects) == false)
        {
            return null;
        }

        return objects[index] as T;
    }

    protected T[] UI_GetAll<T>() where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects;

        if (_uiChilds.TryGetValue(typeof(T), out objects) == false)
        {
            return null;
        }

        return objects.Cast<T>().ToArray();
    }
    // name 탐색을 위해 T는 Object 타입이어야 함
    private UnityEngine.Object UI_FindChild<T>(string name) where T : UnityEngine.Object
    {
        if(typeof(T) == typeof(GameObject))
        {
            foreach (Transform t in GetComponentsInChildren<Transform>())
            {
                if (t.name == name)
                {
                    return t.gameObject;
                }
            }
        }
        else
        {
            foreach (T component in GetComponentsInChildren<T>())
            {
                if (component.name == name)
                {
                    return component;
                }
            }
        }


        return null;
    }
}
