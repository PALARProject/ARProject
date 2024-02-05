using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextUI : MonoBehaviour
{
    public enum Status
    {
        HP,APK
    }
    public Status status;
    private Text text;
    private void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.UserInfo == null)
            return;
        switch (status)
        {
            case Status.HP:
                int sum_HP = 0;
                for(int i = 0; i < GameManager.instance.UserInfo.inventoryItems.Count; i++)
                {
                    try
                    {
                        int index = i;
                        sum_HP += GameManager.instance.UserInfo.inventoryItems[i].status.dp;
                    }
                    catch
                    {
                        sum_HP += 0;
                    }
                }
                sum_HP += 50;
                text.text = sum_HP.ToString();
                break;
            case Status.APK:
                int sum_AP = 0;
                for (int i = 0; i < GameManager.instance.UserInfo.inventoryItems.Count; i++)
                {
                    try
                    {
                        int index = i;
                        sum_AP += GameManager.instance.UserInfo.inventoryItems[i].status.ap;
                    }
                    catch
                    {
                        sum_AP += 0;
                    }
                }
                sum_AP += 10;
                text.text = sum_AP.ToString();
                break;
            default:
                break;
        }    
    }
}
