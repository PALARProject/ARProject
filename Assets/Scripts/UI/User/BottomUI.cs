using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomUI : MonoBehaviour
{
    [SerializeField] protected Button characterButton;
    public Button CharacterButton{get{ return this.characterButton; }set { this.characterButton = value; } }
    [SerializeField] protected Button itemButton;
    public Button ItemButton { get{ return this.itemButton; }set { this.itemButton = value; } }
    [SerializeField] protected Button optionButton;
    public Button OptionButton { get{ return this.optionButton; }set { this.optionButton = value; } }
    [SerializeField] protected Button questButton;
    public Button QuestButton { get{ return this.questButton; }set { this.questButton = value; } }

    public List<Button> btns = new List<Button>();
    private void Awake()
    {
        btns.Add(CharacterButton);
        btns.Add(ItemButton);
        btns.Add(OptionButton);
        btns.Add(QuestButton);
        for(int i = 0; i < btns.Count; i++)
        {
            int index = i;
            if (btns[index] == null)
                continue;
            btns[index].onClick.AddListener(() => {
                OnlyOneBtn(index);
            });
        }
    }
    public void OnlyOneBtn(int n)
    {
        for(int i = 0; i < btns.Count; i++)
        {
            if (btns[i] == null)
                continue;
            if (i != n)
            {
                btns[i].interactable = true;
            }
        }
    }
}
