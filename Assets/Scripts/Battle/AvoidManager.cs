using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public enum Direction { LEFT, RIGHT, FORWARD };

public class AvoidManager : MonoBehaviour
{
    public float countdown;
    public GameObject countUI;
    public Text countText;
    public Text angleText;

    public ARFaceManager faceManager;
    public GameObject ChangeCam;
    public GameObject chamCham;

    public Direction playerDirection;
    public Direction AIDirection;

    public bool isCheck;

    public GameObject AvoidUI;
    public GameObject BattleUI;

    private void OnEnable()
    {
        countdown = 7;
        isCheck = false;
        angleText.text = "";
        countText = countUI.GetComponent<Text>();

        chamCham.transform.eulerAngles = Vector3.zero;
        faceManager.facesChanged += OnFaceChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCheck) return;

        countdown -= Time.deltaTime;
        if(countdown <=4f && countdown > 1f)
        {
            countUI.SetActive(true);
            countText.text = Mathf.FloorToInt(countdown).ToString();
        }
        else if(countdown <= 1)
        {
            countText.text = "ȸ��!";
        }

        if(countdown <= 0)
        {
            countdown = 0f;
            ControlAvoidValue();
            CheckAvoid();

            isCheck = true;
            faceManager.facesChanged -= OnFaceChanged;
        }
    }

    void ControlAvoidValue()
    {
        int randValue = Random.Range(0, 3);
        switch(randValue)
        {
            case 0:
                AIDirection = Direction.RIGHT;
                chamCham.transform.eulerAngles -= new Vector3(0, 0, 30f);
                break;
            case 1:
                AIDirection = Direction.LEFT;
                chamCham.transform.eulerAngles += new Vector3(0, 0, 30f);
                break;
            case 2:
                AIDirection = Direction.FORWARD;
                chamCham.transform.eulerAngles = Vector3.zero;
                break;
        }
    }

    void CheckAvoid()
    {
        if (playerDirection == AIDirection)
        {
            Debug.Log("���� ����!");
            ShowResult(false);

        }
        else
        {
            Debug.Log("ȸ�� ����!");
            ShowResult(true);
        }
    }

    void OnFaceChanged(ARFacesChangedEventArgs args)
    {
        if (args.updated.Count > 0)
        {
            ARFace face = args.updated[0];
            Quaternion faceRotation = face.transform.rotation;

            float yAngle = Quaternion.Euler(0, faceRotation.eulerAngles.y, 0).eulerAngles.y;
            angleText.text = "";

            if (yAngle > 14f && yAngle < 30f)
            {
                playerDirection = Direction.RIGHT;
                Debug.Log("����ڰ� �������� ���ϰ� �ֽ��ϴ�");
            }
            else if (yAngle > 330f && yAngle < 355f)
            {
                playerDirection = Direction.LEFT;
                Debug.Log("����ڰ� ������ ���ϰ� �ֽ��ϴ�");
            }
            else
            {
                playerDirection = Direction.FORWARD;
                Debug.Log("����ڰ� ������ ���ϰ� �ֽ��ϴ�.");
            }
        }
        else
        {
            angleText.text = "���� ã�� �� �����ϴ�.";
        }
    }

    void ShowResult(bool success)
    {
        StartCoroutine(ShowResultReturn(success));
    }

    IEnumerator ShowResultReturn(bool success)
    {
        if (success)
        {
            countText.text = AIDirection.ToString() + playerDirection.ToString() +"����!";
            BattleManager.instance.AvoidSuccess();
        }
        else
        { 
            countText.text = AIDirection.ToString() + playerDirection.ToString() + "����!";
            BattleManager.instance.AvoidFailed();
        }

        yield return new WaitForSeconds(3f);

        countText.text = "";
        BattleUI.SetActive(true);
        AvoidUI.SetActive(false);
        ChangeCam.GetComponent<ChangedCam>().ChangeBattleStateCam();

    }
}
