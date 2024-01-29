using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private List<GameObject> UIObjs = new List<GameObject>();
    private void Awake()
    {
        UIObjs.Add(InventoryUI);
        UIObjs.Add(StatusUI);
        UIObjs.Add(OptionUI);
        UIObjs.Add(QuestUI);
    }
    public void OpenCloseInventory()
    {
        OnlyOneUI(InventoryUI.name);
        BottomUI.GetComponent<BottomUI>().ItemButton.interactable= InventoryUI.activeSelf;
        InventoryUI.SetActive(!InventoryUI.activeSelf);
    }
    public void OpenCloseStatus()
    {
        OnlyOneUI(StatusUI.name);
        BottomUI.GetComponent<BottomUI>().CharacterButton.interactable = StatusUI.activeSelf;
        StatusUI.SetActive(!StatusUI.activeSelf);
    }
    public void OpenCloseOption()
    {
        OnlyOneUI(OptionUI.name);
        BottomUI.GetComponent<BottomUI>().OptionButton.interactable = OptionUI.activeSelf;
        OptionUI.SetActive(!OptionUI.activeSelf);
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
}
