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

    void Start()
    {
        arManager = GetComponent<ARRaycastManager>();
        enemy = EnemyManager.EnemyPrefabs[0];
    }

    void Update()
    {
        DeleteGround();
    }

    void DeleteGround()
    {
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        List<ARRaycastHit> hitInfos = new List<ARRaycastHit>();


        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 spawnPosition = cameraForward * 2f;

        if (PlacedObject == null)
        {
            PlacedObject = Instantiate(enemy, spawnPosition, Quaternion.identity);
        }
        /*if (arManager.Raycast(screenCenter, hitInfos, TrackableType.AllTypes))
        {
            Vector3 cameraForward = Camera.main.transform.forward;
        }*/
    }


    private void OnDisable()
    {
        if(PlacedObject != null)
        {
            PlacedObject.Destroy();
        }
    }
}
