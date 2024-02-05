using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //userInfo
    UserInfo userInfo=null;
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
        UserInfo = await DBManager.GetUserInfo("�ݽ�");

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