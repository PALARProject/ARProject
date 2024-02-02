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
            // ���� ������Ʈ�� ��ġ�� Unity ��ǥ���� �������� ��ǥ�� ��ȯ
            Vector2d objectCoordinates = _map.WorldToGeoPosition(transform.position);

            // ��ȯ�� ��ǥ ���
            Debug.Log("Object Coordinates (Latitude, Longitude): " + objectCoordinates.x + ", " + objectCoordinates.y);
        }
        else
        {
            Debug.LogError("Map reference is not set!");
        }
    }
}
