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
    public Text playerText;
    public Text enemyText;
    public Text resultText;
    public Text stateText;

    public ARFaceManager faceManager;
    public GameObject ChangeCam;
    public GameObject chamCham;

    public Direction playerDirection;
    public Direction AIDirection;

    public bool isCheck;

    public GameObject AvoidUI;
    public GameObject BattleUI;
    public GameObject AwareUI;
    public GameObject ResultUI;
    public GameObject StateUI;

    private void OnEnable()
    {
        AvoidStart();
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

    public void RetryAvoid()
    {
        AvoidStart();
        AwareUI.SetActive(false);
    }

    public void CancelAvoid()
    {
        StartCoroutine(ShowResultReturn(false));
        AwareUI.SetActive(false);
    }

    void AvoidStart()
    {
        countdown = 7;
        isCheck = false;
        StateUI.SetActive(true);
        countText = countUI.GetComponent<Text>();

        chamCham.transform.eulerAngles = Vector3.zero;
        faceManager.facesChanged += OnFaceChanged;
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
        countText.text = "";
        StateUI.SetActive(false);
        ResultUI.SetActive(true);
        playerText.text = playerDirection.ToString();
        enemyText.text = AIDirection.ToString();
        if (playerDirection == AIDirection)
        {
            resultText.text = "ȸ�� ����!";
        }
        else
        {
            resultText.text = "ȸ�� ����!";
        }
    }

    void OnFaceChanged(ARFacesChangedEventArgs args)
    {
        if (args.updated.Count > 0)
        {
            ARFace face = args.updated[0];
            Quaternion faceRotation = face.transform.rotation;

            float yAngle = Quaternion.Euler(0, faceRotation.eulerAngles.y, 0).eulerAngles.y;

            if (yAngle > 14f && yAngle < 30f)
            {
                playerDirection = Direction.RIGHT;
                stateText.text = "������";
            }
            else if (yAngle > 330f && yAngle < 355f)
            {
                playerDirection = Direction.LEFT;
                stateText.text = "����";
            }
            else
            {
                playerDirection = Direction.FORWARD;
                stateText.text = "����";
            }
        }
        else
        {
            AwareUI.SetActive(true);
        }
    }

    public void ShowResult()
    {
        if(playerDirection != AIDirection)
        {
            StartCoroutine(ShowResultReturn(true));
        }
        else
        {
            StartCoroutine(ShowResultReturn(false));
        }
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

        yield return new WaitForSeconds(0.5f);
        AvoidUI.SetActive(false);
        ResultUI.SetActive(false);
        BattleUI.SetActive(true);
        ChangeCam.GetComponent<ChangedCam>().ChangeBattleStateCam();

    }
}
