using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    List<Transform> _evenPoints = new List<Transform>();
    List<Transform> _oddPoints = new List<Transform>();

    bool _isOdd = false;

    float _accTime = 0.0f;

    private void Awake()
    {
        for(int i = 0; i < 8; ++i)
        {
            if(i % 2 == 1)
            {
                _oddPoints.Add(transform.GetChild(i));
            }
            else
            {
                _evenPoints.Add(transform.GetChild(i));
            }
        }
    }

    private void Update()
    {
        if(Managers.GameManagerEx.IsPause == true || Managers.GameManagerEx.IsBossBattle == true)
        {
            return;
        }

        if(Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }

        _accTime += Time.deltaTime;

        if (_accTime > Managers.GameManagerEx.SpawnData.spawnTimes[Managers.GameManagerEx.SpawnTimeLevel])
        {
            for(int i = 0; i < 4; ++i)
            {
                Vector3 playerPos = Managers.GameManagerEx.Player.transform.position;
                GameObject monster = Managers.ResourceManager.Instantiate("Objects/Monster");

                #region Lagacy : 주변 360도 랜덤 위치
                // 삼각 함수 메서드들은 Radian 단위를 사용.
                //float randomAngle = Random.Range(0.0f, 360.0f);
                //float randomRadian = randomAngle * Mathf.Deg2Rad;
                //
                //Vector3 ranDir = new Vector2(Mathf.Cos(randomRadian), Mathf.Sin(randomRadian));
                //float monsterPosX = (playerPos + (ranDir.normalized * (_spawnDist / 2))).x;
                //float monsterPosY = (playerPos + (ranDir.normalized * _spawnDist)).y;
                #endregion

                if(_isOdd == true)
                {
                    monster.transform.position = new Vector2(_oddPoints[i].position.x, _oddPoints[i].position.y);
                }
                else
                {
                    monster.transform.position = new Vector2(_evenPoints[i].position.x, _evenPoints[i].position.y);
                }

                int minType = (int)Managers.GameManagerEx.SpawnData.
                    minTypes[Managers.GameManagerEx.SpawnTypeLevel];
                int maxType = (int)Managers.GameManagerEx.SpawnData.
                    maxTypes[Managers.GameManagerEx.SpawnTypeLevel];

                int monsterIndex = Random.Range(minType, maxType + 1);
                monster.GetComponent<Monster>().Init(monsterIndex);
            }

            _accTime = 0.0f;
            _isOdd = !_isOdd;
        }
    }
}
