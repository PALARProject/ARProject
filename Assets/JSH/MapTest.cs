namespace Mapbox.Unity.Map
{ 
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
using Mapbox.Unity.Location;
    using Mapbox.Unity.Map;

    public class MapTest : MonoBehaviour
    {
        AbstractMap _map;

        ILocationProvider _locationProvider;
        // Start is called before the first frame update
        void Start()
        {
            _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
        }

        // Update is called once per frame
        void Update()
        {

        }
        public  void Maptesty()
        {
            Debug.Log(_locationProvider.CurrentLocation.LatitudeLongitude);
        }
    }
}
