using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    protected List<GameObject> popUpUI;
    public List<GameObject> PopUPUI { get { return this.popUpUI; }set { this.popUpUI = value; } }

    private void Awake()
    {
        PopUPUI.Add(InventoryUI);
        PopUPUI.Add(StatusUI);
    }

    public void OpenCloseInventory()
    {
        ClosePoP();
        InventoryUI.SetActive(!InventoryUI.activeSelf);
    }
    public void OpenCloseStatus()
    {
        ClosePoP();
        StatusUI.SetActive(!StatusUI.activeSelf);
    }

    private void ClosePoP()
    {
        InventoryUI.SetActive(false);
        StatusUI.SetActive(false);
    }
}
