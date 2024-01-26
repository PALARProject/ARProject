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

    public void OpenCloseInventory()
    {
        InventoryUI.SetActive(!InventoryUI.activeSelf);
    }
    public void OpenCloseStatus()
    {
        StatusUI.SetActive(!StatusUI.activeSelf);
    }
}
