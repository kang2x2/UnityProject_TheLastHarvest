using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();
    Transform _root = null;

    public void Init()
    {
        if(_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void CreatePool(GameObject original, int amount = 10)
    {
        Pool pool = new Pool();
        pool.Init(original, amount);
        pool.Root.parent = _root;

        _pools.Add(original.name, pool);
    }

    public GameObject Pop(GameObject original)
    {
        if (_pools.ContainsKey(original.name) == false)
        {
            CreatePool(original);
        }

        return _pools[original.name].Pop();
    }

    public void Push(GameObject obj)
    {
        _pools[obj.name].Push(obj);
    }

    public GameObject GetOriginal(string name)
    {
        if (!_pools.ContainsKey(name))
        {
            return null;
        }

        return _pools[name].Original;
    }

    public void Clear()
    {
        // Pool_Root의 자식 Pool 모두 제거
        foreach(Transform child in _root)
        {
            GameObject.Destroy(child.gameObject);
        }

        // 풀 딕셔너리 초기화
        _pools.Clear();
    }
}
