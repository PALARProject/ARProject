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
        if (_map != null && A != null)
        {
            // ������Ʈ�� ����Ƽ ��ǥ�� �������� ��ǥ�� ��ȯ
            Vector2d geoCoordinates = _map.WorldToGeoPosition(A.transform.position);
            if (!double.IsInfinity(geoCoordinates.x) && !double.IsInfinity(geoCoordinates.y))
            {
                Debug.Log("Object Coordinates (Latitude, Longitude): " + geoCoordinates.x + ", " + geoCoordinates.y);
            }
            else
            {
                Debug.LogError("Invalid or out-of-bounds coordinates!");
            }
        }
    }
}
