using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Firebase.Auth;

public class UIManager : MonoBehaviour
{
    //InventoryUI
    [SerializeField] protected GameObject inventoryUI;
    public GameObject InventoryUI { get { return this.inventoryUI; }set { this.inventoryUI = value; } }
    //BottomUI
    [SerializeField] protected GameObject bottomUI;
    public GameObject BottomUI { get { return this.bottomUI; }set { this.bottomUI = value; } }
    //StatusUI
    [SerializeField] protected GameObject statusUI;
    public GameObject StatusUI { get { return this.statusUI; } set { this.statusUI = value; } }

    [SerializeField] protected GameObject optionUI;
    public GameObject OptionUI { get { return this.optionUI; } set { this.optionUI = value; } }
    [SerializeField] protected GameObject questUI;
    public GameObject QuestUI { get { return this.questUI; } set { this.questUI = value; } }
    [SerializeField] protected GameObject resultUI;
    public GameObject ResultUI { get { return this.resultUI; } set { this.resultUI = value; } }
    public GameObject LogoutUI { get { return this.logoutUI; } set { this.logoutUI = value; } }
    [SerializeField] protected GameObject logoutUI;

    private List<GameObject> UIObjs = new List<GameObject>();

    public Slider bgmSlider;
    public Slider sfxSlider;
    private AudioManager audioManager;

    private void Awake()
    {
        UIObjs.Add(InventoryUI);
        UIObjs.Add(StatusUI);
        UIObjs.Add(OptionUI);
        UIObjs.Add(QuestUI);
        UIObjs.Add(LogoutUI);
    }

    private void Start()
    {
        // AudioManager 오브젝트를 찾아 가져옴
        audioManager = GameManager.instance.AudioManager;
        if (audioManager == null)
            return;

        // 슬라이더의 초기값을 AudioManager의 볼륨 값으로 설정
        bgmSlider.value = audioManager.bgmVolume;
        sfxSlider.value = audioManager.sfxVolume;

        // 슬라이더 값이 변경될 때마다 AudioManager의 볼륨을 업데이트하는 이벤트 리스너 추가
        bgmSlider.onValueChanged.AddListener(delegate { OnBgmVolumeChanged(); });
        sfxSlider.onValueChanged.AddListener(delegate { OnSfxVolumeChanged(); });
    }

    public void OnBgmVolumeChanged()
    {
        audioManager.SetBgmVolume(bgmSlider.value);
    }

    // SFX 볼륨이 변경될 때 호출되는 메소드
    public void OnSfxVolumeChanged()
    {
        audioManager.SetSfxVolume(sfxSlider.value);
    }

    public void OpenCloseInventory()
    {
        if (InventoryUI == null)
            return;

        OnlyOneUI(InventoryUI.name);
        if(BottomUI!=null)
            BottomUI.GetComponent<BottomUI>().ItemButton.interactable= InventoryUI.activeSelf;
        InventoryUI.SetActive(!InventoryUI.activeSelf);

    }
    public void OpenCloseStatus()
    {
        if (StatusUI == null)
            return;
        OnlyOneUI(StatusUI.name);
        if (BottomUI != null)
            BottomUI.GetComponent<BottomUI>().CharacterButton.interactable = StatusUI.activeSelf;
        StatusUI.SetActive(!StatusUI.activeSelf);

    }
    public void OpenCloseOption()
    {
        if (OptionUI == null)
            return;
        OnlyOneUI(OptionUI.name);
        if (BottomUI != null)
            BottomUI.GetComponent<BottomUI>().OptionButton.interactable = OptionUI.activeSelf;
        OptionUI.SetActive(!OptionUI.activeSelf);

    }
    
    public void OpenCloseQuest()
    {
        if (QuestUI == null)
            return;
        OnlyOneUI(QuestUI.name);
        if (BottomUI != null)
            BottomUI.GetComponent<BottomUI>().QuestButton.interactable = QuestUI.activeSelf;
        QuestUI.SetActive(!QuestUI.activeSelf);

    }
    public void OpenCloseLogout()
    {
        if (LogoutUI == null)
            return;
        OnlyOneUI(LogoutUI.name);
        if (BottomUI != null)
            BottomUI.GetComponent<BottomUI>().OptionButton.interactable = LogoutUI.activeSelf;
        LogoutUI.SetActive(!LogoutUI.activeSelf);

    }
    public void OnlyOneUI(string name)
    {
        for (int i = 0; i < UIObjs.Count; i++)
        {
            if (UIObjs[i] == null)
                continue;
            if (UIObjs[i].name != name)
            {
                UIObjs[i].SetActive(false);
            }
        }
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene("LoginPage");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Logout() {
        FirebaseAuth.DefaultInstance.SignOut();
        SceneManager.LoadScene("LoginPage");
    }
}
