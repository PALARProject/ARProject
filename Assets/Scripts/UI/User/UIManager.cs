using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    }

    private void Start()
    {
        // AudioManager ������Ʈ�� ã�� ������
        audioManager = GameManager.instance.AudioManager;
        if (audioManager == null)
            return;

        // �����̴��� �ʱⰪ�� AudioManager�� ���� ������ ����
        bgmSlider.value = audioManager.bgmVolume;
        sfxSlider.value = audioManager.sfxVolume;

        // �����̴� ���� ����� ������ AudioManager�� ������ ������Ʈ�ϴ� �̺�Ʈ ������ �߰�
        bgmSlider.onValueChanged.AddListener(delegate { OnBgmVolumeChanged(); });
        sfxSlider.onValueChanged.AddListener(delegate { OnSfxVolumeChanged(); });
    }

    public void OnBgmVolumeChanged()
    {
        audioManager.SetBgmVolume(bgmSlider.value);
    }

    // SFX ������ ����� �� ȣ��Ǵ� �޼ҵ�
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
}
