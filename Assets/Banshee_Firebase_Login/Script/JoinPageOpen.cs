using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinPageOpen : MonoBehaviour
{
    public GameObject JoinPage;
    public bool joinPageActive = false;

    public void JoinPageSetActiveChange(){

        joinPageActive = !joinPageActive;
        JoinPage.SetActive(joinPageActive);
    }
    
}
