using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public List<GameObject> Quests;
    public Transform questLocation;
    public GameObject quest;
    public void CreateQuest()
    {
        bool set = false;
        for(int i = 0; i < Quests.Count; i++)
        {
            if (Quests[i].activeSelf == false)
            {
                set = true;
                Quests[i].SetActive(true);
                Quests[i].transform.SetAsLastSibling();
            }
        }
        if (!set)
        {
            GameObject obj = Instantiate(quest, questLocation);
            Quests.Add(obj);
        }
    }
}
