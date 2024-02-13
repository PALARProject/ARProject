using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public int questId;
    public string[] completeItem;
    Text title;
    Text desc;
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
    public async void Init(int questId)
    {
        this.questId = questId;
        this.condition = false;
        QuestInfo quest = await GameManager.instance.DBManager.GetQuestInfo(questId);
        title.text = quest.title;
        desc.text = quest.desc;
        completeItem = quest.compen;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompleteQuest()
    {
        string[] list
            = new string[2];
            //= completeItem;
        
        list[0] = "°¡Á×°©¿Ê";
        list[1] = "±Ý¼Ó°©¿Ê";
        GameManager.instance.UIManager.ResultUI.GetComponent<ResulUI_Ingame>().EatItem(list);
    }
}
