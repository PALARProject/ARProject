using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class RotateDetection : MonoBehaviour
{
    public ARFaceManager faceManager;
    public Text script;
    public Text value;
    // Start is called before the first frame update
    void Start()
    {
        faceManager.facesChanged += OnFaceChanged;
    }

    // Update is called once per frame
    void OnFaceChanged(ARFacesChangedEventArgs args)
    {
        if (args.updated.Count > 0)
        {
            ARFace face = args.updated[0];
            Quaternion faceRotation = face.transform.rotation;

            float yAngle = Quaternion.Euler(0, faceRotation.eulerAngles.y, 0).eulerAngles.y;
            value.text = yAngle.ToString();

            if (yAngle > 10f && yAngle < 30f)
            {
                script.text = "사용자가 오른쪽을 향하고 있습니다";
            }
            else if (yAngle > 340f && yAngle < 355f)
            {
                script.text = "사용자가 왼쪽을 향하고 있습니다";
            }
            else
            {
                script.text = "사용자가 정면을 향하고 있습니다.";
            }
        }
        else
        {
            value.text = "-";
            script.text = "얼굴을 찾을 수 없습니다.";
        }
    }
}
