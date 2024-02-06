using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    public float shakeAmount;
    float shakeTime;

    Vector3 initialPosition;
    public void VibrationObject(float amount, float time)
    {
        shakeAmount = amount;
        shakeTime = time;
    }


    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if(shakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * shakeAmount + initialPosition;
            shakeTime -= Time.deltaTime;
        }
        else
        {
            shakeTime = 0.0f;
            transform.position = initialPosition;
        }
    } 
}
