using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;

public class MapTest: MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;
    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;

    void Start()
    {
        GetObjectCoordinates();
    }

    void GetObjectCoordinates()
    {
        if (_map != null)
        {
            // 현재 오브젝트의 위치를 Unity 좌표에서 지도상의 좌표로 변환
            Vector2d AobjectCoordinates = _map.WorldToGeoPosition(A.transform.position);

            // 변환된 좌표 출력
            Debug.Log("Object Coordinates (Latitude, Longitude): " + AobjectCoordinates.x + ", " + AobjectCoordinates.y);
            double AA = Mathf.Clamp((float)AobjectCoordinates.y,-180f,180f);
            Debug.Log(AA);
        }
        else
        {
            Debug.LogError("Map reference is not set!");
        }
    }
}
