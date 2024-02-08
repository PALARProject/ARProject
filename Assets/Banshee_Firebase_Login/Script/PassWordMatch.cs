using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PassWordMatch : MonoBehaviour
{
    public TMP_InputField PassWord;
    public TMP_InputField PassWordConfirm;
    public GameObject NotMatchText;

    public void Update() {
    if((PassWord.text != PassWordConfirm.text)&& PassWordConfirm.text !=""){
            NotMatchText.SetActive(true);
    }else{
            NotMatchText.SetActive(false);
        }
    }
}
