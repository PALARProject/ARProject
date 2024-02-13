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
        Trash.onClick.AddListener(async() =>
        {
            await GameManager.instance.InventoryManager.OutputInventory(i);
            gameObject.SetActive(false);
        });
        Exit.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
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
