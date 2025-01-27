using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    List<Transform> _evenPoints = new List<Transform>();
    List<Transform> _oddPoints = new List<Transform>();
    List<Transform> _allPoints = new List<Transform>();

    bool _isOdd = false;
    float _nmSpawnAccTime = 0.0f;

    // float _flySpawnTime = 8.0f;
    // float _flySpawnAccTime = 0.0f;

    private void Awake()
    {
        for (int i = 0; i < 8; ++i)
        {
            if(i % 2 == 1)
            {
                _oddPoints.Add(transform.GetChild(i));
            }
            else
            {
                _evenPoints.Add(transform.GetChild(i));
            }

            _allPoints.Add(transform.GetChild(i));
            transform.GetChild(i).position = transform.position;
        }

        SpawnPosSet();
    }

    private void SpawnPosSet()
    {
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Camera.main.aspect;

        GameObject point = GameObject.Find("SpawnPointUp");
        point.transform.position = new Vector2(transform.position.x, transform.position.y + (height / 2) + 1.0f);
        
        point = GameObject.Find("SpawnPointRightUp");
        point.transform.position = new Vector2(transform.position.x + (width / 2) + 1.0f, transform.position.y + (height / 2) + 1.0f);

        point = GameObject.Find("SpawnPointRight");
        point.transform.position = new Vector2(transform.position.x + (width / 2) + 1.0f, transform.position.y);

        point = GameObject.Find("SpawnPointRightDown");
        point.transform.position = new Vector2(transform.position.x + (width / 2) + 1.0f, transform.position.y - (height / 2) - 1.0f);

        point = GameObject.Find("SpawnPointDown");
        point.transform.position = new Vector2(transform.position.x, transform.position.y - (height / 2) - 1.0f);

        point = GameObject.Find("SpawnPointLeftDown");
        point.transform.position = new Vector2(transform.position.x - (width / 2) - 1.0f, transform.position.y - (height / 2) - 1.0f);

        point = GameObject.Find("SpawnPointLeft");
        point.transform.position = new Vector2(transform.position.x - (width / 2) - 1.0f, transform.position.y);

        point = GameObject.Find("SpawnPointLeftUp");
        point.transform.position = new Vector2(transform.position.x - (width / 2) - 1.0f, transform.position.y + (height / 2) + 1.0f);
    }

    private void Update()
    {
        if(Managers.GameManagerEx.IsPause == true ||
           Managers.GameManagerEx.IsClear == true ||
           Managers.SceneManagerEx.IsLoading == true)
        {
            return;
        }

        NormalSpawn();

        //if(Managers.GameManagerEx.ProgressTime >= 240.0f)
        //{
        //    _flySpawnAccTime += Time.deltaTime;
        //    if(_flySpawnAccTime >= _flySpawnTime)
        //    {
        //        _flySpawnAccTime = 0.0f;
        //        IEnumerator coSpawn = FlySpawn();
        //        StartCoroutine(coSpawn);
        //    }
        //}
    }

    void NormalSpawn()
    {
        _nmSpawnAccTime += Time.deltaTime;
        // Managers.GameManagerEx.SpawnData.spawnTimes[Managers.GameManagerEx.SpawnTimeLevel]
        if (_nmSpawnAccTime >= Managers.GameManagerEx.SpawnData.spawnTimes[Managers.GameManagerEx.SpawnTimeLevel])
        {
            for (int i = 0; i < 4; ++i)
            {
                GameObject monster = Managers.ResourceManager.Instantiate("Objects/Monster", null, false);

                #region Lagacy : 주변 360도 랜덤 위치
                // 삼각 함수 메서드들은 Radian 단위를 사용.
                //float randomAngle = Random.Range(0.0f, 360.0f);
                //float randomRadian = randomAngle * Mathf.Deg2Rad;
                //
                //Vector3 ranDir = new Vector2(Mathf.Cos(randomRadian), Mathf.Sin(randomRadian));
                //float monsterPosX = (playerPos + (ranDir.normalized * (_spawnDist / 2))).x;
                //float monsterPosY = (playerPos + (ranDir.normalized * _spawnDist)).y;
                #endregion

                if (_isOdd == true)
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
                monster.GetComponent<NormalMonster>().Init(monsterIndex);
            }

            _nmSpawnAccTime = 0.0f;
            _isOdd = !_isOdd;
        }
    }

    IEnumerator FlySpawn()
    {
        int spawnCount = 0;
        while(spawnCount < 16)
        {
            int monsterIndex = 0;
            switch (Managers.GameManagerEx.MapType)
            {
                case Define.MapType.Field:
                    monsterIndex = (int)Define.FlyMonsterType.Bird;
                    break;
                case Define.MapType.Cave:
                    monsterIndex = (int)Define.FlyMonsterType.Ghost;
                    break;
            }

            int ranIndex = Random.Range(0, _allPoints.Count);
            Vector3 playerPos = Managers.GameManagerEx.Player.transform.position;
            GameObject monster = Managers.ResourceManager.Instantiate("Objects/FlyMonster");

            Vector3 spawnPos = new Vector2(_allPoints[ranIndex].position.x, _allPoints[ranIndex].position.y);
            Vector3 dir = playerPos - spawnPos;
            Vector3 reversDir = spawnPos - playerPos;
            monster.transform.position = spawnPos + (reversDir.normalized * 2.5f);
            monster.GetComponent<FlyMonster>().Init(monsterIndex, dir.normalized);

            spawnCount += 1;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
