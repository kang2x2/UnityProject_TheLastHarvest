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
            action.Invoke("ȸ�������� �Ϸ�Ǿ����ϴ�.");
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
            // ���� �� �������� ��ȯ�� JSON ������
            string jsonResponse = uwr.downloadHandler.text;

            // JSON ���ڿ��� GameResult ��ü�� ��ȯ
            MyGameResult = JsonUtility.FromJson<GameResult>(jsonResponse);
            IsLogin = true;

            action.Invoke("�α��� �Ϸ�!");
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
            // �������� ��ȯ�� JSON ������
            string jsonResponse = uwr.downloadHandler.text;
            Debug.Log("Request successful. Response: " + jsonResponse);

            try
            {
                // JSON�� GameResultListWrapper�� ��ȯ
                GameResultListWrapper wrapper = JsonUtility.FromJson<GameResultListWrapper>("{\"gameResults\":" + jsonResponse + "}");
                // GameResult ����Ʈ�� ����
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
            // ������ ���, �� �ڼ��� ���� �޽����� ���
            Debug.LogError("Request failed with error: " + uwr.error);
            Debug.LogError("Response body: " + uwr.downloadHandler.text);
        }
    }

    public IEnumerator CoUpdateRequest(string url, string method, Action action)
    {
        // DownloadHandlerBuffer : �ܼ��� ������ ���丮��, �������� ������ �����͸� UTF-8 ���ڿ��̳�
        // ����Ʈ �迭�� ����� �� �ִ�.
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
