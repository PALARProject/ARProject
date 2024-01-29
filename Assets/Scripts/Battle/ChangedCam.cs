using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangedCam : MonoBehaviour
{
    public GameObject mainCam;
    public GameObject ARSessionOrgin;
    public GameObject ARSession;

    public bool isAR;

    // Start is called before the first frame update
    void Start()
    {
        isAR = false;
    }

    public void ChangedCamBtn()
    {
        if (isAR == true)
        {
            mainCam.SetActive(true);
            ARSession.SetActive(false);
            ARSessionOrgin.SetActive(false);
            isAR = false;
        }
        else if (isAR == false)
        {
            mainCam.SetActive(false);
            ARSession.SetActive(true);
            ARSessionOrgin.SetActive(true);
            isAR = true;
        }
    }
}
