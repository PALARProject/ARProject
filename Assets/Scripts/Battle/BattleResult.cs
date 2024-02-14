using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
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

    public GameObject Result_Item;
    public GameObject[] resultLocation=new GameObject[9];
    // Start is called before the first frame update

    private void Awake()
    {
        isResult = false;
    }

    public void ActiveResultUI(int result)
    {
        endUI.SetActive(true);
        if(result == 1)
        {
            endText.text = "Victory";
        }
        else if(result == 2)
        {
            endText.text = "Dead";
        }
        else if(result == 3)
        {
            endText.text = "Escape";
        }
        StartCoroutine(AnimateUI(result));
    }
    
    IEnumerator AnimateUI(int result)
    {
        //아이템 정산
        Task<List<ItemInfo>> list= GameManager.instance.DBManager.GetItemsTable();
        float time = 0;
        while (!list.IsCompletedSuccessfully && time<5)
        {
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        List<ItemInfo> collectItem = new List<ItemInfo>();
        if (time < 5)
        {
            List<ItemInfo> itemList = list.Result;
            for (int i = 0; i < 3; i++)
            {
                int random = Random.Range(0, itemList.Count);
                int n = 0;
                foreach (ItemInfo pair in itemList)
                {
                    if (n == random)
                    {
                        ItemInfo copy = pair.DeepCopy();
                        collectItem.Add(copy);
                        Debug.Log(collectItem[i].name);
                        itemList.Remove(pair);
                        break;
                    }
                    n++;
                }
            }
        }
        for (int i=0;i< collectItem.Count; i++)
        {
            int index = i;
            GameObject obj = Instantiate(Result_Item, resultLocation[i].transform);
            Button btn=obj.GetComponent<Button>();
            Image image = obj.GetComponent<Image>();
            image.sprite = Resources.Load<Sprite>("Item/Sprite/" + collectItem[i].name);
            string name = collectItem[index].name;
            btn.onClick.AddListener(async () =>
            {
                string itemName = name;
                int list = await GameManager.instance.InventoryManager.InputInventory(itemName);
                btn.interactable = false;
                btn.onClick.RemoveAllListeners();
            });
        }
        resultUI.SetActive(true);
        if (result == 1)
        {
            resultText.text = "Victory";
        }
        else if (result == 2)
        {
            resultText.text = "Dead";
        }
        else if (result == 3)
        {
            resultText.text = "Escape";
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
