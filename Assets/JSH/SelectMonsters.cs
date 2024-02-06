using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectMonsters : MonoBehaviour
{
    GameObject obj;
    public GameObject Montext;
    private RaycastHit Hit2;
    private int i;
    // Start is called before the first frame update
    void Start()
    {
        Montext.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector3 screen = new Vector3(touch.position.x, touch.position.y, 0);
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 120));
                Debug.Log("P" + touch.position);
                Debug.Log("M" + Input.mousePosition);
                Debug.Log(touchPos);
                //Vector3 touchPos = Camera.main.transform.position + new Vector3(touch.position.x,0,touch.position.y);

                Vector3 rayvec = touchPos - Camera.main.transform.position;

                RaycastHit hit;
                Physics.Raycast(Camera.main.transform.position, rayvec,out hit);
                Hit2 = hit;
                Debug.Log(hit.collider);
                Debug.DrawRay(Camera.main.transform.position, rayvec, Color.red, 1f);
            }
            if (Hit2.collider != null && Hit2.collider.tag == "Enemy"&& i == 1 )
            {
                SceneManager.LoadScene("BattleScene");
            }
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Enemy")
        {
            i = 1;
            Montext.SetActive(true);
            Debug.Log(collision.transform.position);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy")
        {
            i = 0;
            Montext.SetActive(false);
        }
    }

}