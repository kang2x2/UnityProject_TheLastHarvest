using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Game : Scene_Base
{
    bool isCameraFollowSet = false;

    public override void Init()
    {
        SceneType = Define.SceneType.GameScene;
        Managers.SceneManagerEx.CurScene = this;
        isCameraFollowSet = false;
    }

    public override IEnumerator Loading(Action<float> onProgress)
    {
        int taskCount = 9;
        float progressRatio = 1.0f / taskCount;
        int successTaskCount = 0;

        Managers.PoolManager.CreatePool(Managers.ResourceManager.Load<GameObject>("UI/Worlds/WorldUI_DamageText"), 50);
        onProgress?.Invoke(progressRatio * ++successTaskCount);
        yield return null;

        Managers.PoolManager.CreatePool(Managers.ResourceManager.Load<GameObject>("UI/Worlds/WorldUI_HpBar"), 1000);
        onProgress?.Invoke(progressRatio * ++successTaskCount);
        yield return null;

        Managers.PoolManager.CreatePool(Managers.ResourceManager.Load<GameObject>("Objects/Monster"), 1000);
        onProgress?.Invoke(progressRatio * ++successTaskCount);
        yield return null;

        Managers.PoolManager.CreatePool(Managers.ResourceManager.Load<GameObject>("Objects/ExpObject"), 200);
        onProgress?.Invoke(progressRatio * ++successTaskCount);
        yield return null;

        Managers.PoolManager.CreatePool(Managers.ResourceManager.Load<GameObject>("Objects/Projectile_Bullet"), 50);
        onProgress?.Invoke(progressRatio * ++successTaskCount);
        yield return null;

        Managers.PoolManager.CreatePool(Managers.ResourceManager.Load<GameObject>("Objects/Projectile_BigBullet"), 5);
        onProgress?.Invoke(progressRatio * ++successTaskCount);
        yield return null;

        Managers.PoolManager.CreatePool(Managers.ResourceManager.Load<GameObject>("Objects/BulletHitEffect"), 100);
        onProgress?.Invoke(progressRatio * ++successTaskCount);
        yield return null;

        Managers.PoolManager.CreatePool(Managers.ResourceManager.Load<GameObject>("Objects/BigBulletHitEffect"), 50);
        onProgress?.Invoke(progressRatio * ++successTaskCount);
        yield return null;

        Managers.PoolManager.CreatePool(Managers.ResourceManager.Load<GameObject>("Objects/SlashHitEffect"), 100);
        onProgress?.Invoke(progressRatio * ++successTaskCount);
        yield return null;

        // Managers.PoolManager.CreatePool(Managers.ResourceManager.Load<GameObject>("Objects/BlowHitEffect"), 100);
        // onProgress?.Invoke(progressRatio * ++successTaskCount);
        // yield return null;

        Managers.GameManagerEx.Init();
        // 시네머신 카메라 팔로워 세팅
        while (isCameraFollowSet == false)
        {
            if(CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera != null)
            {
                CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.Follow = Managers.GameManagerEx.Player.transform;
                isCameraFollowSet = true;
            }
        }
        yield return true;
    }
}
