using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UISizeController : MonoBehaviour
{
    public RectTransform[] uiElements; // ��� UI ��Ҹ� �迭�� ����

    void Start() {
        // ���� ������ ���ο� ���� ���
        float currentScreenWidth = 1080f;
        float currentScreenHeight = 1920f;

        float newScreenWidth = Screen.width; 
        float newScreenHeight = Screen.height;

        float widthRatio = newScreenWidth / currentScreenWidth;
        float heightRatio = newScreenHeight / currentScreenHeight;

        // ��� UI ��ҿ� ���� �ݺ��Ͽ� ��ġ�� ũ�⸦ ����
        foreach(RectTransform uiElement in uiElements) {
            // ���� ��ġ�� ũ�� ��������
            Vector2 currentOffsetMin = uiElement.offsetMin;
            Vector2 currentOffsetMax = uiElement.offsetMax;

            // ���ο� ��ġ�� ũ�� ���
            Vector2 newOffsetMin = new Vector2(currentOffsetMin.x * widthRatio, currentOffsetMin.y * heightRatio);
            Vector2 newOffsetMax = new Vector2(currentOffsetMax.x * widthRatio, currentOffsetMax.y * heightRatio);

            // ��ġ�� ũ�� ����
            uiElement.offsetMin = newOffsetMin;
            uiElement.offsetMax = newOffsetMax;
        }
    }
}
