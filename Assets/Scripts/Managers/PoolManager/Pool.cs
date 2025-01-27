using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public GameObject Original { get; private set; }
    public Transform Root { get; private set; }

    private Queue<GameObject> _poolQ = new Queue<GameObject>();

    
    private GameObject Create()
    {
        GameObject obj = Object.Instantiate(Original);
        obj.name = Original.name;

        return obj;
    }
    public void Init(GameObject original, int amount)
    {
        Original = original;
        Root = new GameObject().transform;
        Root.name = $"{Original.name}_Root";

        for (int i = 0; i < amount; ++i)
        {
            Push(Create());
        }
    }

    public void Push(GameObject obj)
    {
        obj.transform.SetParent(Root);
        obj.SetActive(false);

        _poolQ.Enqueue(obj);
    }

    public GameObject Pop(bool isActive)
    {
        GameObject obj = null;
        if (_poolQ.Count > 0)
        {
            obj = _poolQ.Dequeue();
        }
        else
        {
            Push(Create());
            obj = _poolQ.Dequeue();
            Debug.Log(obj.name + " �ʰ�");
        }

        obj.SetActive(isActive);
        obj.transform.SetParent(Managers.SceneManagerEx.CurScene.transform); 

        return obj;
    }
}
