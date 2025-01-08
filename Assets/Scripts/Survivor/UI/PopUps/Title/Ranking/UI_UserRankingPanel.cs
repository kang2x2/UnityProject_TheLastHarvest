using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using System.IO;
public class UI_UserRankingPanel : UI_Base
{
    enum Texts
    {
        UserRankingText,
        UserNameText,
        UserScoreText,
    }

    int _killScore;
    int _clearScore;

    public void Init(int rank, GameResult gameResult, Define.RankingType type)
    {
        UI_Bind<Text>(typeof(Texts));

        _killScore = gameResult.killScore;
        _clearScore = gameResult.clearScore;

        UI_Get<Text>((int)Texts.UserRankingText).text = rank.ToString();
        UI_Get<Text>((int)Texts.UserNameText).text = gameResult.userName;
        UI_Get<Text>((int)Texts.UserScoreText).text = _killScore.ToString();

        switch(type)
        {
            case Define.RankingType.Kill:
                UI_Get<Text>((int)Texts.UserScoreText).text = _killScore.ToString();
                break;
            case Define.RankingType.Clear:
                UI_Get<Text>((int)Texts.UserScoreText).text = _clearScore.ToString();
                break;
        }

        switch (rank)
        {
            case 1:
                GetComponent<Image>().color = Color.yellow;
                break;
            case 2:
                GetComponent<Image>().color = new Color(1.0f, 0.7f, 0.4f);
                break;
            case 3:
                GetComponent<Image>().color = new Color(0.6f, 1.0f, 1.0f);
                break;
            default:
                GetComponent<Image>().color = new Color(0.85f, 0.85f, 0.85f);
                break;
        }
    }
}
