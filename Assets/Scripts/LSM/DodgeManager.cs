using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public enum Direction { LEFT, RIGHT, FORWARD };

public class DodgeManager : MonoBehaviour
{
    public float countdown;
    public GameObject countUI;
    public Text countText;

    public ARFaceManager faceManager;

    public Direction playerDirection;
    public Direction AIDirection;

    public bool isCheck;

    void Start()
    {
        countdown = 7;
        isCheck = false;
        countText = countUI.GetComponent<Text>();

        faceManager.facesChanged += OnFaceChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCheck) return;

        countdown -= Time.deltaTime;
        if(countdown <=3)
        {
            countText.text = Mathf.FloatToHalf(countdown).ToString();
        }

        if(countdown <= 0)
        {
            countdown = 0f;
            countUI.SetActive(false);
            ControlDodgeValue();

            isCheck = true;
        }
    }

    void ControlDodgeValue()
    {
        int randValue = Random.Range(0, 3);
        switch(randValue)
        {
            case 0:
                AIDirection = Direction.RIGHT;
                break;
            case 1:
                AIDirection = Direction.LEFT;
                break;
            case 2:
                AIDirection = Direction.FORWARD;
                break;
        }
    }

    void CheckDodge()
    {

    }

    void OnFaceChanged(ARFacesChangedEventArgs args)
    {
        if (args.updated.Count > 0)
        {
            ARFace face = args.updated[0];
            Quaternion faceRotation = face.transform.rotation;

            float yAngle = Quaternion.Euler(0, faceRotation.eulerAngles.y, 0).eulerAngles.y;

            if (yAngle > 10f && yAngle < 30f)
            {
                playerDirection = Direction.RIGHT;
                Debug.Log("사용자가 오른쪽을 향하고 있습니다");
            }
            else if (yAngle > 340f && yAngle < 355f)
            {
                playerDirection = Direction.LEFT;
                Debug.Log("사용자가 왼쪽을 향하고 있습니다");
            }
            else
            {
                playerDirection = Direction.FORWARD;
                Debug.Log("사용자가 정면을 향하고 있습니다.");
            }
        }
        else
        {
            Debug.LogError("얼굴을 찾을 수 없습니다.");
        }
    }
}
