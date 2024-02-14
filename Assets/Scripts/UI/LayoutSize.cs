using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LayoutSize : MonoBehaviour
{
    public GridLayoutGroup uiElement; // 모든 UI 요소를 배열로 저장
    public float currentScreenWidth = 1080f;
    public float currentScreenHeight = 1920f;
    void Start() {
        // 현재 비율과 새로운 비율 계산
       

        float newScreenWidth = Screen.width; 
        float newScreenHeight = Screen.height;

        float widthRatio = newScreenWidth / currentScreenWidth;
        float heightRatio = newScreenHeight / currentScreenHeight;
        uiElement.cellSize= new Vector2(uiElement.gameObject.GetComponent<RectTransform>().rect.width, 600);
    }
}
