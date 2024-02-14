using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    QuestInfo quest;
    public int questId;
    public string[] completeItem;
    public Text title;
    public Text desc;
    Button btn;
    public bool condition = false;
    int count = 0;
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
        quest = await GameManager.instance.DBManager.GetQuestInfo(questId);
        quest.compen = await GameManager.instance.DBManager.GetQuestCompenInfo(questId);
        this.title.text = quest.title;
        this.desc.text = quest.desc;
        completeItem = quest.compen;
    }

    private void LateUpdate()
    {
        switch (questId)
        {
            case 1:
                //상자 여는 코드
                if (PlayerPrefs.HasKey("OpenBox"))
                {
                    if (PlayerPrefs.GetInt("OpenBox") == 1)
                    {
                        condition = true;
                    }
                }
                PlayerPrefs.SetInt("OpenBox", 0);
                break;
            case 2:
                if (PlayerPrefs.HasKey("CatchMob"))
                {
                    if(PlayerPrefs.GetInt("CatchMob") == 1)
                    {
                        count++;
                        Debug.Log("count:" +count);
                        condition = true;
                    }
                }
                PlayerPrefs.SetInt("CatchMob", 0);
                break;
            case 3:
                break;
            default:
                break;
        }
    }

    public async void CompleteQuest()
    {
        string[] list
            = completeItem;
        GameManager.instance.UIManager.ResultUI.GetComponent<ResulUI_Ingame>().EatItem(list);
        await GameManager.instance.DBManager.UpdateQuestInfo(GameManager.instance.UserInfo.userName,questId);
    }
}
