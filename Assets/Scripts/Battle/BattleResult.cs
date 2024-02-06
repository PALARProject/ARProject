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

    public float animSpeed = 2f;
    public bool isResult;
    // Start is called before the first frame update

    private void Awake()
    {
        isResult = false;
    }

    public void ActiveResultUI(bool result)
    {
        endUI.SetActive(true);
        if(result == true)
        {
            endText.text = "Victory";
        }
        else if(result == false)
        {
            endText.text = "Dead";
        }
        StartCoroutine(AnimateUI(result));
    }

    IEnumerator AnimateUI(bool result)
    {
        yield return new WaitForSeconds(2f);

        resultUI.SetActive(true);
        if (result == true)
        {
            resultText.text = "Victory";
        }
        else if (result == false)
        {
            resultText.text = "Dead";
        }
        RectTransform rectTransform = resultUI.GetComponent<RectTransform>();
        Vector3 targetPosition = Vector3.zero; 

        while (rectTransform.anchoredPosition.y < targetPosition.y)
        {
            rectTransform.anchoredPosition += Vector2.up * animSpeed * Time.deltaTime;
            
        }
    }

    public void ReturnMainScene()
    {
        //SceneManager.LoadScene("MainCameraScene");
        SceneManager.LoadScene("Ingame");
    }
}
