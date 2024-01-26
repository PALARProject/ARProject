using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomUI : MonoBehaviour
{
    public Button[] UI;

    private void Awake()
    {
        for (int i = 0; i < UI.Length; i++)
        {
            int index = i;
            UI[index].onClick.AddListener(() => {

            });
        }
    }
    private void LateUpdate()
    {
        if (GameManager.instance.UIManager.InventoryUI.activeSelf)
        {
        }
    }
}
