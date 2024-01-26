using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public Button[] Items;
    private Image[] Item_Images;

    private void Awake()
    {
        Item_Images = new Image[Items.Length];
        for(int i = 0; i < Items.Length; i++)
        {
            int index = i;
            Items[index].onClick.AddListener(()=> { 
                Debug.Log(index);
                OutputInventory(index);
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
                //image=Resources.Load<Sprite>("Item/Sprite/"+GameManager.instance.DBManager.GetInventoryImage(GameManager.instance.UserInfo.inventoryItems[index]));
                image =Resources.Load<Sprite>("Item/Sprite/"+index);
                Debug.Log(image.name);
                if (image != null)
                {
                    Item_Images[index].sprite = image;
                    Items[index].interactable = true;
                    GameManager.instance.UserInfo.inventoryItems[index] = new ItemInfo(index, index.ToString(), 0, index, new Status());
                }
                else
                {
                    Item_Images[index].sprite = null;
                    Items[index].interactable = false;
                    GameManager.instance.UserInfo.inventoryItems[index] = new ItemInfo();
                }
            }
            catch
            {
                Item_Images[index].sprite = null;
                Items[index].interactable = false;
                GameManager.instance.UserInfo.inventoryItems[index] = new ItemInfo();
                Debug.Log(index + "-��ϵ��� ���� ���");
            }
        }

    }
    public async void InputInventory(int itemId)
    {

        //ItemInfo getItemInfo = await GameManager.instance.DBManager.GetItemTable(itemId);
        //if (getItemInfo == null)
        //{
        //    return;
        //}
        ItemInfo getItemInfo = new ItemInfo(itemId, itemId.ToString(), 0, itemId, new Status());
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
            if (haveItem.itemId == getItemInfo.itemId)
            {
                //UI - ���� ������ �Ծ��� ���

                Debug.Log(itemId + "- �̹� ���� ������");
                return;
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
        if (num == -1)
        {
            num = sameItem;
        }
        Sprite image;
        try
        {
            image = Resources.Load<Sprite>("Item/Sprite/" + getItemInfo.itemId);
            if (image != null)
            {
                Item_Images[num].sprite = image;
                Items[num].interactable = true;
                GameManager.instance.UserInfo.inventoryItems[num] = getItemInfo;

                //byte[] updateInven = System.Text.Encoding.UTF8.GetBytes(num+" "+itemId);
                //await GameManager.instance.DBManager.UpdateUserInfo(GameManager.instance.UserInfo.userId, updateInven);
                Debug.Log("Inventory " + num + "- ������:"+ itemId+"�� �� Ȱ��ȭ�˴ϴ�.");
            }
        }
        catch
        {
            Debug.Log("-��ϵ��� ���� ���");
        }
        return;
    }
     
    
    public ItemInfo OutputInventory(int inventoryNum)
    {
        Debug.Log("Inventory " + inventoryNum + "- ������ ���õǾ� ��Ȱ��ȭ�˴ϴ�.");
        ItemInfo outItem = GameManager.instance.UserInfo.inventoryItems[inventoryNum].DeepCopy();
        GameManager.instance.UserInfo.inventoryItems[inventoryNum] = new ItemInfo();
        Item_Images[inventoryNum].sprite = null;
        Items[inventoryNum].interactable = false;

        try
        {
            byte[] updateInven = System.Text.Encoding.UTF8.GetBytes(inventoryNum + " " + -1);
            GameManager.instance.DBManager.UpdateUserInfo(GameManager.instance.UserInfo.userId, updateInven);
        }
        catch
        {
            //return null;
        }
        return outItem;
        
        //������ ��� ��� or ������ ��� �߰�
    }
}
