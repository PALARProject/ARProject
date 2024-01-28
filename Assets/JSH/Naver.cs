using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



public class Naver : MonoBehaviour
{
    float timer;
    int waitingtime;
    public RawImage mapRawImage;

    [Header("�� ���� �Է�")]
    public string strBaseURL = "";
    public string latutuede = "";
    public string longitude = "";
    public int level = 14;
    public int mapWidth;
    public int mapHeight;
    public string strAPIKey = "";
    public string secretKey = "";

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        waitingtime = 2;
        mapRawImage = GetComponent<RawImage>();
        StartCoroutine(MapLoader());
    }

    IEnumerator MapLoader()
    {
        string str = strBaseURL + "?w=" + mapWidth.ToString() + "&h=" + mapHeight.ToString() + "&center=" + longitude + "," + latutuede + "&level=" + level.ToString();
        Debug.Log(str);
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(str);
        request.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", strAPIKey);
        request.SetRequestHeader("X-NCP-APIGW-API-KEY", secretKey);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            mapRawImage.texture = DownloadHandlerTexture.GetContent(request);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > waitingtime)
        {
            latutuede = Input.location.lastData.latitude.ToString();
            longitude = Input.location.lastData.longitude.ToString();
            MapLoader();
            timer = 0;
        }
    }
}
