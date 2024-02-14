using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EnemyAR : MonoBehaviour
{
    ARRaycastManager arManager;
    public EnemyManager EnemyManager;
    public GameObject enemy;
    public GameObject PlacedObject;
    // Start is called before the first frame update
    void Start()
    {
        arManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        DeleteGround();
    }

    void DeleteGround()
    {
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        List<ARRaycastHit> hitInfos = new List<ARRaycastHit>();
        if (arManager.Raycast(screenSize, hitInfos, TrackableType.Planes))
        {
            enemy = EnemyManager.EnemyPrefabs[0];
            if (PlacedObject == null)
            {
                PlacedObject = Instantiate(enemy, hitInfos[0].pose.position, hitInfos[0].pose.rotation);
            }
        }
    }
}
