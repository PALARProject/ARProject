using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    public float shakeAmount;
    float shakeTime;

    public float camAmount;
    float camTime;
    
    public GameObject cam;
    public GameObject enemy;

    Vector3 initialPosition;
    Vector3 initialCamPosition;

    public bool isShake;
    public bool isCamShake;

    public ChangedCam changeCam;

    public void VibrationObject(float amount, float time)
    {
        if (changeCam.isAR)
        {
            enemy = GameObject.FindWithTag("ARSession").GetComponent<EnemyAR>().enemy;
            initialPosition = enemy.transform.position;
        }
        else
        {
            enemy = GameObject.FindWithTag("Enemy");
            initialPosition = enemy.transform.position;
        }

        shakeAmount = amount;
        shakeTime = time;
        isShake = true;
    }

    public void VibrationCam(float amount, float time)
    {
        cam = GameObject.FindWithTag("MainCamera");
        initialCamPosition = cam.transform.position;

        camAmount = amount;
        camTime = time;
        isCamShake = true;
    }


    private void Start()
    {
        cam = GameObject.FindWithTag("MainCamera");
        initialCamPosition = cam.transform.position;
    }

    private void Update()
    {
        if(isShake)
        {
            if (shakeTime > 0)
            {
                enemy.transform.position = Random.insideUnitSphere * shakeAmount + initialPosition;
                shakeTime -= Time.deltaTime;
            }
            else
            {
                shakeTime = 0.0f;
                enemy.transform.position = initialPosition;
                isShake = false;
            }
        }

        if(isCamShake)
        {
            if (camTime > 0)
            {
                cam.transform.position = Random.insideUnitSphere * camAmount + initialCamPosition;
                camTime -= Time.deltaTime;
            }
            else
            {
                camTime = 0.0f;
                cam.transform.position = initialCamPosition;
                isCamShake = false;
            }
        }

    }
}
