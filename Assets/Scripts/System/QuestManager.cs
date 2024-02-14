using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public List<GameObject> Quests;
    public Transform questLocation;
    public GameObject quest;
    public void Init()
    {
        foreach(var pair in GameManager.instance.UserInfo.haveQuest)
        {
            CreateQuest(pair.Value);
        }
    }
    public void CreateQuest(int questId)
    {
        bool set = false;
        for(int i = 0; i < Quests.Count; i++)
        {
            if (Quests[i].activeSelf == false)
            {
                set = true;
                Quests[i].SetActive(true);
                Quests[i].transform.SetAsLastSibling();
                Quests[i].GetComponent<QuestUI>().Init(questId);
            }
        }
        if (!set)
        {
            GameObject obj = Instantiate(quest, questLocation);
            obj.GetComponent<QuestUI>().Init(questId);
            Quests.Add(obj);
        }
    }

}

public class QuestInfo
{
    public int questId=-1;
    public string title="";
    public string desc="";
    public string[] compen;

    public QuestInfo() { compen = null; }
    public QuestInfo(int _questId,string _title,string _desc, string[] _compen)
    {
        questId = _questId;
        title = _title;
        desc = _desc;
        compen = _compen;
    }
}
