using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public GameObject ItemInfoUI;
    // Start is called before the first frame update
    void Start()
    {
        Button[] items=GameManager.instance.InventoryManager.Items;
        for(int i = 0; i < items.Length; i++)
        {
            int index = i;
            items[index].onClick.AddListener(() => {
                ItemInfoUI.SetActive(true);
                ItemInfoUI.GetComponent<ItemInfoUI>().Init(index);
                //아이템 버리기 및 닫기 구현
            });
        }
    }
    private void OnDisable()
    {
        ItemInfoUI.SetActive(false);
    }
}
