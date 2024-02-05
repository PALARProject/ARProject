using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    public void OnShaking()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float t = 3f;
        float shakePower = 0.02f;
        Vector3 origin = transform.position;

        while(t > 0f)
        {
            t -= 0.05f;
            transform.position = origin + (Vector3)Random.insideUnitCircle * shakePower * t;
            yield return null;
        }

        transform.position = origin;
    }
}
