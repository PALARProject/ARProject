using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUI : MonoBehaviour
{
    public Image image;
    public Text Name;
    public Text DP;
    public Text AP;
    public Text Description;
    public Button Exit;
    public Button Trash;

    private Button temp;
    
    public void Init(int i)
    {
        Trash.onClick.RemoveAllListeners();
        Exit.onClick.RemoveAllListeners();
        image.sprite = GameManager.instance.InventoryManager.Item_Images[i].sprite;
        Name.text = GameManager.instance.UserInfo.inventoryItems[i].name;
        DP.text = GameManager.instance.UserInfo.inventoryItems[i].status.dp.ToString();
        AP.text = GameManager.instance.UserInfo.inventoryItems[i].status.ap.ToString();
        Description.text= GameManager.instance.UserInfo.inventoryItems[i].description.ToString();
        Trash.onClick.AddListener(async () =>
        {
            await GameManager.instance.InventoryManager.OutputInventory(i);
            gameObject.SetActive(false);
        });
        Exit.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }
    public void Result_Init(GameObject obj, ItemInfo item)
    {

        Trash.onClick.RemoveAllListeners();
        Exit.onClick.RemoveAllListeners();
        image.sprite = Resources.Load<Sprite>("Item/Sprite/" + item.name);
        Name.text = item.name;
        DP.text = item.status.dp.ToString();
        AP.text = item.status.ap.ToString();
        Description.text = item.description;
        Trash.interactable = true;
        Trash.onClick.AddListener(async () =>
        {
            string itemName = item.name;
            int list = await GameManager.instance.InventoryManager.InputInventory(itemName);
            Trash.interactable = false;
            Trash.onClick.RemoveAllListeners();
            Exit.onClick.RemoveAllListeners();
            obj.GetComponent<Button>().interactable = false;
            gameObject.SetActive(false);
        });
        Exit.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            Trash.onClick.RemoveAllListeners();
            Exit.onClick.RemoveAllListeners();
        });
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
