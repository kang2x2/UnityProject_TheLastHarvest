using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
            {
                name = name.Substring(index + 1);
            }

            GameObject obj = Managers.PoolManager.GetOriginal(name);
            if (obj != null)
            {
                return obj as T;
            }
        }

        return Resources.Load<T>($"Prefabs/{path}");
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>(path);
        if (original == null)
        {
            Debug.Log($"Not Found Object/Prefab : {path}");
            return null;
        }

        if(original.GetComponent<UsePooling>() != null)
        {
            return Managers.PoolManager.Pop(original);
        }

        GameObject obj = Object.Instantiate(original, parent);
        obj.name = original.name;

        return obj;
    }

    public void Destroy(GameObject destroyObj, float destroyTime = 0.0f)
    {
        if (destroyObj == null)
            return;

        if(destroyObj.GetComponent<UsePooling>() != null)
        {
            Managers.PoolManager.Push(destroyObj);
            return;
        }

        Object.Destroy(destroyObj, destroyTime);
    }
}
