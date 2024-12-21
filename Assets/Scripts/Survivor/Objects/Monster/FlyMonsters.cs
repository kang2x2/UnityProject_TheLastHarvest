using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMonsters : MonoBehaviour
{
    FlyMonster[] monsters;
    private void Awake()
    {
        monsters = GetComponentsInChildren<FlyMonster>();
    }
    private void Update()
    {
        bool isDestory = false;
        foreach(FlyMonster monster in monsters)
        {
            if(monster.IsLive == false)
            {
                isDestory = true;
                break;
            }
        }

        if(isDestory == true)
        {
            Managers.ResourceManager.Destroy(gameObject);
        }
    }
}
