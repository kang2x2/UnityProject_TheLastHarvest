using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class GameResultListWrapper
{
    public List<GameResult> gameResults;
}
public class WebManager
{
    public string BaseUrl { get; private set; }
    public bool IsLogin { get; set; }
    public GameResult MyGameResult { get; private set; }
    public List<GameResult> GameResults { get; private set; } = new List<GameResult>();
    public void Init()
    {
        BaseUrl = "https://localhost:44384/api";
        IsLogin = false;

        Managers.CoroutineManager.StartCoroutine(CoGetAllRequest("ranking", "GET"));
    }

    public IEnumerator CoSignUpRequest(string url, string method, object obj, Action<string> action)
    {
        string sendUrl = $"{BaseUrl}/{url}";

        byte[] jsonBytes = null;
        if (obj != null)
        {
            string json = JsonUtility.ToJson(obj);
            jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
        }

        var uwr = new UnityWebRequest(sendUrl, method);
        uwr.uploadHandler = new UploadHandlerRaw(jsonBytes);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            action.Invoke(uwr.downloadHandler.text);
        }
        else
        {
            action.Invoke("회원가입이 완료되었습니다.");
        }
    }

    public IEnumerator CoLoginRequest(string url, string method, string id, string pw, Action<string> action)
    {
        string sendUrl = $"{BaseUrl}/{url}?id={id}&pw={pw}";
        var uwr = new UnityWebRequest(sendUrl, method);
        uwr.downloadHandler = new DownloadHandlerBuffer();

        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            action.Invoke(uwr.downloadHandler.text);
        }
        else
        {
            // 성공 시 서버에서 반환한 JSON 데이터
            string jsonResponse = uwr.downloadHandler.text;

            // JSON 문자열을 GameResult 객체로 변환
            MyGameResult = JsonUtility.FromJson<GameResult>(jsonResponse);
            IsLogin = true;

            action.Invoke("로그인 완료!");
        }
    }

    public IEnumerator CoGetAllRequest(string url, string method, 
        Define.RankingType rankingType = Define.RankingType.Kill, Action action = null)
    {
        string sendUrl = $"{BaseUrl}/{url}?rankingType={rankingType.ToString()}";
        var uwr = new UnityWebRequest(sendUrl, method);
        uwr.downloadHandler = new DownloadHandlerBuffer();

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            // 서버에서 반환한 JSON 데이터
            string jsonResponse = uwr.downloadHandler.text;
            Debug.Log("Request successful. Response: " + jsonResponse);

            try
            {
                // JSON을 GameResultListWrapper로 변환
                GameResultListWrapper wrapper = JsonUtility.FromJson<GameResultListWrapper>("{\"gameResults\":" + jsonResponse + "}");
                // GameResult 리스트를 전달
                GameResults = wrapper.gameResults;
                action?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError("Error parsing JSON response: " + ex.Message);
            }
        }
        else
        {
            // 실패한 경우, 더 자세한 오류 메시지를 출력
            Debug.LogError("Request failed with error: " + uwr.error);
            Debug.LogError("Response body: " + uwr.downloadHandler.text);
        }
    }

    public IEnumerator CoUpdateRequest(string url, string method, Action action)
    {
        // DownloadHandlerBuffer : 단순한 데이터 스토리지, 서버에서 수신한 데이터를 UTF-8 문자열이나
        // 바이트 배열로 사용할 수 있다.
        string sendUrl = $"{BaseUrl}/{url}";
        byte[] jsonBytes = null;
        if (MyGameResult != null)
        {
            MyGameResult.killScore += Managers.GameManagerEx.Kill;
            MyGameResult.playTime += Managers.GameManagerEx.ProgressTime;

            if(Managers.GameManagerEx.GameOverType == Define.GameOverType.Clear)
            {
                MyGameResult.clearScore += 1;
            }

            string json = JsonUtility.ToJson(MyGameResult);
            jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
        }

        var uwr = new UnityWebRequest(sendUrl, method);
        uwr.uploadHandler = new UploadHandlerRaw(jsonBytes);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        action.Invoke();
    }
}
