using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public int questID;
    public string completeItem;
    Button btn;
    public bool condition = false;
    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            if (condition)
            {
                CompleteQuest();
                GameManager.instance.UIManager.ResultUI.SetActive(true);
                gameObject.SetActive(false);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteQuest()
    {
        string[] list = new string[2];
        list[0] = "���װ���";
        list[1] = "�ݼӰ���";
        GameManager.instance.UIManager.ResultUI.GetComponent<ResulUI_Ingame>().EatItem(list);
    }
}
