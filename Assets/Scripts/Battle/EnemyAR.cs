using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EnemyAR : MonoBehaviour
{
    ARRaycastManager arManager;
    public GameObject Enemy;
    GameObject PlacedObject;
    // Start is called before the first frame update
    void Start()
    {
        arManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        DeleteGround();
    }

    void DeleteGround()
    {
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        List<ARRaycastHit> hitInfos = new List<ARRaycastHit>();

        if(arManager.Raycast(screenSize, hitInfos, TrackableType.Planes))
        {
            if (PlacedObject == null)
            {
                PlacedObject = Instantiate(Enemy, hitInfos[0].pose.position, hitInfos[0].pose.rotation);
            }
            else
            {
                PlacedObject.transform.SetPositionAndRotation(hitInfos[0].pose.position, hitInfos[0].pose.rotation);
            }
        }
    }
}
