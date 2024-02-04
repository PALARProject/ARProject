using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyinCount : MonoBehaviour
{
    public GameObject Battle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Enemy")
        {
            Battle.SetActive(true);
            Debug.Log("d");
        }
        else
        {
            Battle.SetActive(false);
            Debug.Log("A");
        }
    }

}
