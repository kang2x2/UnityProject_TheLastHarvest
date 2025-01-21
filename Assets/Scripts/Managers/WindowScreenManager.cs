using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowScreenManager
{
    // 9:10 ���� ���� (�ʺ�:����)
    private float targetAspect = 9.0f / 10.0f;

    public void SetWindowSize()
    {
        // ���� ȭ�� �ػ� ��������
        int curWidth = Screen.currentResolution.width;
        int curHeight = Screen.currentResolution.height;

        // ȭ�� ������ �°� â ũ�� ����
        int changeWidth = Mathf.FloorToInt(curHeight * targetAspect);
        int changeHeight = curHeight;

        // �ػ󵵰� �ʹ� �۾Ƽ� ������ �� ������, �⺻�� ����
        if (changeWidth > curWidth)
        {
            changeWidth = curWidth;
            changeHeight = Mathf.FloorToInt(curWidth / targetAspect);
        }

        // â ���� �����ϰ� ������ ������ �ػ󵵷� ����
        Screen.SetResolution(changeWidth, changeHeight, true);
    }
}
