using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene_Title : Scene_Base
{
    Vector2[] _cameraDestPositons = new Vector2[4];
    int _curPosIndex;

    public override void Init()
    {
        SceneType = Define.SceneType.TitleScene;
        Managers.SceneManagerEx.CurScene = this;

        _cameraDestPositons[0] = new Vector2(4, 0);
        _cameraDestPositons[1] = new Vector2(0, 4);
        _cameraDestPositons[2] = new Vector2(-4, 0);
        _cameraDestPositons[3] = new Vector2(0, -4);

        _curPosIndex = 0;

        Managers.ResourceManager.Instantiate("Objects/TitlePalController");
    }

    private void Update()
    {
        if(Camera.main != null)
        {
            Camera.main.transform.position = Vector2.MoveTowards(Camera.main.transform.position, _cameraDestPositons[_curPosIndex], Time.deltaTime);
            float dist = Vector2.Distance(Camera.main.transform.position, _cameraDestPositons[_curPosIndex]);

            if (dist < 0.02f)
            {
                _curPosIndex += 1;
                if (_curPosIndex >= _cameraDestPositons.Length)
                {
                    _curPosIndex = 0;
                }
            }
        }
    }
}
