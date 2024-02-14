using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //userInfo
    UserInfo userInfo = null;
    public UserInfo UserInfo { get { return this.userInfo; } set { this.userInfo = value; } }

    //DB
    [SerializeField] protected DBManager dbManager;
    public DBManager DBManager { get { return this.dbManager; } set { this.dbManager = value; } }
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
            if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
        }
        else {
            gameObject.Destroy();
        }
        //매니저 초기화
        if (AudioManager != null)
        {
            AudioManager.Init();
        }


        //유저설정
        if (DBManager != null)
        {
            UserInfo = await DBManager.GetUserInfo(PlayerPrefs.GetString("UID"));
            await DBManager.GetUserQuestInfo();
        }

        if (InventoryManager != null)
        {
            InventoryManager.Init();
        }

        if (QuestManager != null)
        {
            QuestManager.Init();
        }
        
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
        //나중에 암호화 예정
        //prefs는 컴퓨터 내부 저장소를 사용하기 때문에 보안상의 문제가 생길 수 있다.
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
        float sound = PlayerPrefs.GetFloat(key);
        return sound;
    }

    private void LateUpdate()
    {
    }
    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        try
        {
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                AudioManager.PlayBgm(true, 0);
            }
            else if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                int random = Random.Range(0, 3);
                AudioManager.PlayBgm(true, random + 1);
            }
        }
        catch
        {
            return;
        }
    }
}
public class UserInfo
{
    public string userName="";
    public Dictionary<int, ItemInfo> inventoryItems=new Dictionary<int, ItemInfo>();
    public Dictionary<int, int> haveQuest = new Dictionary<int, int>();
    public UserInfo() { }
    public UserInfo(string _userName, Dictionary<int, ItemInfo> _inventoryItems,Dictionary<int,int> _haveQuest)
    {
        this.userName = _userName;
        this.inventoryItems = _inventoryItems;
        this.haveQuest = _haveQuest;
    }
}