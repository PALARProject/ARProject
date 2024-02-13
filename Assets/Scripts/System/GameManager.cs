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
    //Inventory
    [SerializeField] protected QuestManager questManager;
    public QuestManager QuestManager { get { return this.questManager; } set { this.questManager = value; } }
    //UI
    [SerializeField] protected UIManager uiManager;
    public UIManager UIManager { get { return this.uiManager; } set { this.uiManager = value; } }
    //UI
    [SerializeField] protected AudioManager audioManager;
    public AudioManager AudioManager { get { return this.audioManager; } set { this.audioManager = value; } }

    public bool ready = false;

    private async void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //�Ŵ��� �ʱ�ȭ
        if(AudioManager!=null)
            AudioManager.Init();


        //��������
        if(DBManager!=null)
            UserInfo = await DBManager.GetUserInvenInfo(PlayerPrefs.GetString("UID"));

        if(InventoryManager!=null)
            InventoryManager.Init();

        if (QuestManager != null)
            //QuestManager.UserQuest();
        ready = true;
        //UserInfo.inventoryItems;
    }
    public void SaveUserKey(string key, string userKey)
    {
        PlayerPrefs.SetString(key, userKey);
    }
    public string LoadUserKey(string key, string userKey)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return "";
        }
        //���߿� ��ȣȭ ����
        //prefs�� ��ǻ�� ���� ����Ҹ� ����ϱ� ������ ���Ȼ��� ������ ���� �� �ִ�.
        string userName=PlayerPrefs.GetString(key);
        return userName;
    }
    public void SaveSound(string key, float volume)
    {
        PlayerPrefs.SetFloat(key, volume);
    }
    public float LoadSound(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return -1;
        }
        float sound=PlayerPrefs.GetFloat(key);
        return sound;
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