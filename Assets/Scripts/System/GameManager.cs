using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //userInfo
    UserInfo userInfo=new UserInfo();
    public UserInfo UserInfo { get { return this.userInfo; } set { this.userInfo = value; } }

    //DB
    [SerializeField] protected DBManager dbManager;
    public DBManager DBManager{get{return this.dbManager;} set { this.dbManager = value; } }
    //Inventory
    [SerializeField] protected InventoryManager inventoryManager;
    public InventoryManager InventoryManager { get { return this.inventoryManager; } set { this.inventoryManager = value; } }
    //UI
    [SerializeField] protected UIManager uiManager;
    public UIManager UIManager { get { return this.uiManager; } set { this.uiManager = value; } }


    private async void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        Dictionary<int, ItemInfo> inven = new Dictionary<int, ItemInfo>();
        inven.Add(0,await DBManager.GetItemTable("°¡Á×°©¿Ê"));
        inven.Add(1,await DBManager.GetItemTable("±Ý¼Ó°©¿Ê"));
        inven.Add(2,await DBManager.GetItemTable("³ª¹«¸ùµÕÀÌ"));
        inven.Add(3,await DBManager.GetItemTable("¶±°¥³ª¹« ÁöÆÎÀÌ"));
        inven.Add(4,await DBManager.GetItemTable("ºÓÀº º¸¼®ÀÇ ¸ñ°ÉÀÌ"));
        inven.Add(5,await DBManager.GetItemTable("¾Ë¼ö¾ø´Â Æ÷¼Ç"));
        inven.Add(6,await DBManager.GetItemTable("¿À·¡µÈ ¸¶¹ý¼­"));
        inven.Add(7,await DBManager.GetItemTable("¿ë»çÀÇ °©¿Ê"));
        inven.Add(8,await DBManager.GetItemTable("¿ë»çÀÇ °Ë"));
        InventoryManager.Init();
        //UserInfo.inventoryItems;
    }
}
public class UserInfo
{
    public string userName="";
    public Dictionary<int, ItemInfo> inventoryItems=new Dictionary<int, ItemInfo>();
    public UserInfo() { }
    public UserInfo(string _userName, Dictionary<int, ItemInfo> _inventoryItems)
    {
        this.userName = _userName;
        this.inventoryItems = _inventoryItems;
    }
}