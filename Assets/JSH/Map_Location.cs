using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;

public class Map_Location : MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;

    void Start()
    {
        GetObjectCoordinates();
    }

    void GetObjectCoordinates()
    {
        if (_map != null)
        {
            // 현재 오브젝트의 위치를 Unity 좌표에서 지도상의 좌표로 변환
            Vector2d objectCoordinates = _map.WorldToGeoPosition(transform.position);

            // 변환된 좌표 출력
            Debug.Log("Object Coordinates (Latitude, Longitude): " + objectCoordinates.x + ", " + objectCoordinates.y);
        }
        else
        {
            Debug.LogError("Map reference is not set!");
        }
    }
}
