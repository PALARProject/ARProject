using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Slider hpSlider;
    public TextMeshProUGUI currentText;
    public TextMeshProUGUI maxText;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;

        maxText.text = (unit.maxHP).ToString();
        currentText.text = (unit.currentHP).ToString();
    }

    public void SetHP(Unit unit, int hp)
    {
        hpSlider.value = hp;
        maxText.text = (unit.maxHP).ToString();
        currentText.text = (unit.currentHP).ToString();
    }
}
