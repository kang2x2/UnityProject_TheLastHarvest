using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowScreenManager
{
    // 9:10 비율 설정 (너비:높이)
    private float targetAspect = 9.0f / 10.0f;

    public void SetWindowSize()
    {
        // 현재 화면 해상도 가져오기
        int curWidth = Screen.currentResolution.width;
        int curHeight = Screen.currentResolution.height;

        // 화면 비율에 맞게 창 크기 설정
        int changeWidth = Mathf.FloorToInt(curHeight * targetAspect);
        int changeHeight = curHeight;

        // 해상도가 너무 작아서 설정할 수 없으면, 기본값 설정
        if (changeWidth > curWidth)
        {
            changeWidth = curWidth;
            changeHeight = Mathf.FloorToInt(curWidth / targetAspect);
        }

        // 창 모드로 설정하고 비율을 유지한 해상도로 설정
        Screen.SetResolution(changeWidth, changeHeight, true);
    }
}
