using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMonsters : MonoBehaviour
{
    GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        
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
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x,touch.position.y,120));
                Debug.Log("P"+touch.position);
                Debug.Log("M"+Input.mousePosition);
                Debug.Log(touchPos);
                //Vector3 touchPos = Camera.main.transform.position + new Vector3(touch.position.x,0,touch.position.y);

                Vector3 rayvec = touchPos-Camera.main.transform.position;

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.transform.position, rayvec);
                Debug.Log(hit.collider);
                Debug.DrawRay(Camera.main.transform.position, rayvec , Color.red, 1f);

            }
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Enemy")
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

                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.transform.position, rayvec);
                    Debug.Log(hit.collider);
                    Debug.DrawRay(Camera.main.transform.position, rayvec, Color.red, 1f);
                    
                }
            }
          //  SceneManager.LoadScene("BattleScene")
;        }
    }
}
