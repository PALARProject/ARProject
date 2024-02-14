using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class ResulUI_Ingame : MonoBehaviour
{
    public GameObject Result_Item;
    public List<GameObject> resultLocation;
    public ItemInfoUI info = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public async void EatItem(string[] items)
    {

        List<ItemInfo> collectItem = new List<ItemInfo>();
        //아이템 급구
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
                continue;
            int index = i;
            ItemInfo list =await GameManager.instance.DBManager.GetItemTable(items[index]);

            ItemInfo copy = list.DeepCopy();
            collectItem.Add(copy);
        }
        //아이템 정산
        for (int i = 0; i < collectItem.Count; i++)
        {
            int index = i;
            GameObject obj = null;
            if (resultLocation[index].transform.childCount>0) {
                obj=resultLocation[index].transform.GetChild(0).gameObject;
            }
            else
            {
                obj = Instantiate(Result_Item, resultLocation[index].transform);
            }
            Button btn = obj.GetComponent<Button>();
            btn.interactable = true;
            Image image = obj.GetComponent<Image>();
            image.sprite = Resources.Load<Sprite>("Item/Sprite/" + collectItem[index].name);
            string name = collectItem[index].name;
            btn.onClick.AddListener(() =>
            {
                if (info != null)
                {
                    info.gameObject.GetComponent<ItemInfoUI>().Result_Init(obj,collectItem[index]);
                    info.gameObject.SetActive(true);
                }
            });
        }
    }
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
