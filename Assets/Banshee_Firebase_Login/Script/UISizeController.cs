using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UISizeController : MonoBehaviour
{
    public RectTransform[] uiElements; // 모든 UI 요소를 배열로 저장
    public float currentScreenWidth = 1080f;
    public float currentScreenHeight = 1920f;
    void Start() {
        // 현재 비율과 새로운 비율 계산
       

        float newScreenWidth = Screen.width; 
        float newScreenHeight = Screen.height;

        float widthRatio = newScreenWidth / currentScreenWidth;
        float heightRatio = newScreenHeight / currentScreenHeight;

        // 모든 UI 요소에 대해 반복하여 위치와 크기를 조정
        foreach(RectTransform uiElement in uiElements) {
            // 현재 위치와 크기 가져오기
            Vector2 currentOffsetMin = uiElement.offsetMin;
            Vector2 currentOffsetMax = uiElement.offsetMax;

            // 새로운 위치와 크기 계산
            Vector2 newOffsetMin = new Vector2(currentOffsetMin.x * widthRatio, currentOffsetMin.y * heightRatio);
            Vector2 newOffsetMax = new Vector2(currentOffsetMax.x * widthRatio, currentOffsetMax.y * heightRatio);

            // 위치와 크기 적용
            uiElement.offsetMin = newOffsetMin;
            uiElement.offsetMax = newOffsetMax;
        }
    }
}
