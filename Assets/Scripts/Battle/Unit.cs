using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int damage;
    public int maxHP;
    public int currentHP;
   
    public void Init()
    {
        //체력 정산
        int sum_HP = 0;
        for (int i = 0; i < GameManager.instance.UserInfo.inventoryItems.Count; i++)
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
        maxHP = sum_HP;
        currentHP = sum_HP;
        //공격력 정산
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
        damage = sum_AP;
    }
    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
        {
            if(currentHP < 0)
            {
                currentHP = 0;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

}
