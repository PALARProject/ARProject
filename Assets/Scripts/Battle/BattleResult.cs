using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleResult : MonoBehaviour
{
    public GameObject endUI;
    public GameObject resultUI;

    public TextMeshProUGUI endText;
    public TextMeshProUGUI resultText;

    public float animSpeed;
    public bool isResult;

    private bool isAnimating;

    private void Awake()
    {
        isResult = false;
    }

    public void ActiveResultUI(bool result)
    {
        endUI.SetActive(true);
        if (result)
        {
            endText.text = "Victory";
        }
        else
        {
            endText.text = "Dead";
        }
        StartCoroutine(AnimateUI(result));
    }

    IEnumerator AnimateUI(bool result)
    {
        yield return new WaitForSeconds(2f);

        resultUI.SetActive(true);
        if (result)
        {
            resultText.text = "Victory";
        }
        else
        {
            resultText.text = "Dead";
        }
        isAnimating = true;
    }

    private void Update()
    {
        if (isAnimating)
        {
            AnimateResultUI();
        }
    }

    private void AnimateResultUI()
    {
        RectTransform rectTransform = resultUI.GetComponent<RectTransform>();
        Vector3 targetPosition = Vector3.zero;

        if (rectTransform.anchoredPosition.y <= targetPosition.y)
        {
            rectTransform.anchoredPosition += new Vector2(0, animSpeed);

            if (rectTransform.anchoredPosition.y > 0)
                rectTransform.anchoredPosition = Vector3.zero;
        }
        else
        {
            isAnimating = false;
        }
    }

    public void ReturnMainScene()
    {
        SceneManager.LoadScene("MainCameraScene");
    }
}
