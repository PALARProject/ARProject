using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public Sprite baseIMG;
    public Button[] Items;
    [HideInInspector]public Image[] Item_Images;
    public GameObject ErrorUI;
    private void Awake()
    {
        Item_Images = new Image[Items.Length];
        for(int i = 0; i < Items.Length; i++)
        {
            int index = i;
            if (Items[index] == null)
                continue;

            Items[index].onClick.AddListener(()=> { 

            });
            Item_Images[index] = Items[index].gameObject.GetComponent<Image>();
        }
    }
    public void Init()
    {
        UserInfo userInfo = GameManager.instance.UserInfo;
        for (int i = 0; i < Items.Length; i++)
        {
            int index = i;
            Sprite image;
            try
            {
                if (userInfo.inventoryItems[index] == null)
                {
                    Item_Images[index].sprite = baseIMG;
                    Items[index].interactable = false;
                    userInfo.inventoryItems[index] = null;
                    continue;
                }
                image=Resources.Load<Sprite>("Item/Sprite/"+ userInfo.inventoryItems[index].name);
                //image =Resources.Load<Sprite>("Item/Sprite/"+index);
                if (image != null)
                {
                    Item_Images[index].sprite = image;
                    Items[index].interactable = true;
                }
                else
                {
                    Item_Images[index].sprite = baseIMG;
                    Items[index].interactable = false;
                    userInfo.inventoryItems[index] = null;
                }
            }
            catch
            {
                Item_Images[index].sprite = baseIMG;
                Items[index].interactable = false;
                userInfo.inventoryItems[index] = null;
                Debug.Log(index + "-등록되지 않은 경로");
            }
        }

    }
    public async Task<int> InputInventory(string itemName)
    {

        ItemInfo getItemInfo = await GameManager.instance.DBManager.GetItemTable(itemName);
        if (getItemInfo == null)
        {
            return -1;
        }
        //ItemInfo getItemInfo = new ItemInfo(0, itemName, "", 0, new Status());
        int num = -1;
        int sameItem = -1;
        for (int i = 0; i < Items.Length; i++)
        {
            int index = i;
            if (Items[index].interactable == false && num == -1)
            {
                num = index;
            }
            ItemInfo haveItem = GameManager.instance.UserInfo.inventoryItems[index];
            if (haveItem != null)
            {
                if (haveItem.itemId == getItemInfo.itemId)
                {
                    //UI - 같은 아이템 먹었다 출력

                    Debug.Log(itemName + "- 이미 먹은 아이템");
                    ErrorUI.SetActive(true);
                    return 0;
                }
                if (getItemInfo.category == haveItem.category)
                {
                    if (getItemInfo.grade>= haveItem.grade)
                    {
                        if (sameItem == -1)
                        {
                            sameItem = index;
                        }
                        else
                        {
                            if(haveItem.grade<GameManager.instance.UserInfo.inventoryItems[sameItem].grade)
                            {
                                sameItem = index;
                            }
                        }
                    }
                }
            }
        }
        if (num == -1)
        {
            num = sameItem;
        }
        Sprite image;
        try
        {
            image = Resources.Load<Sprite>("Item/Sprite/" + getItemInfo.name);
            if (image != null)
            {
                Item_Images[num].sprite = image;
                Items[num].interactable = true;
                GameManager.instance.UserInfo.inventoryItems[num] = getItemInfo;

                await GameManager.instance.DBManager.UpdateUserInfo(GameManager.instance.UserInfo.userName, num, getItemInfo.name);
                Debug.Log("Inventory " + num + "- 아이템:"+ itemName + "이 들어가 활성화됩니다.");
            }
        }
        catch
        {
            Debug.Log("-등록되지 않은 경로");
            return -1;
        }
        Debug.Log("box_" + string.Format("{0:D3}", num + 1));
        await GameManager.instance.DBManager.UpdateUserInfo(GameManager.instance.UserInfo.userName,  num , getItemInfo.name);
        return 1;
    }
     
    
    public async Task<ItemInfo> OutputInventory(int inventoryNum)
    {
        Debug.Log("Inventory " + inventoryNum + "- 아이템 선택되어 비활성화됩니다.");
        ItemInfo outItem = null;
        if (GameManager.instance.UserInfo.inventoryItems[inventoryNum] != null)
        {
            outItem = GameManager.instance.UserInfo.inventoryItems[inventoryNum].DeepCopy();
        }
        GameManager.instance.UserInfo.inventoryItems[inventoryNum] = null;
        Item_Images[inventoryNum].sprite = baseIMG;
        Items[inventoryNum].interactable = false;

        try
        {
            await GameManager.instance.DBManager.UpdateUserInfo(GameManager.instance.UserInfo.userName, inventoryNum, "null");
        }
        catch
        {
            //return null;
        }
        return outItem;
        
        //아이템 기능 사용 or 버리기 기능 추가
    }
}
