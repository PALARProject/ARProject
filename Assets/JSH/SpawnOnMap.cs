﻿namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;

	public class SpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		[Geocode]
		string[] _locationStrings;
		Vector2d[] _locations;

		[SerializeField]
		float _spawnScale = 2f;

		[SerializeField]
		GameObject _markerPrefab;

		List<GameObject> _spawnedObjects;

		void Start()
		{
			Input.location.Start();
			_locations = new Vector2d[5]; // 5개의 랜덤 위치를 생성
			LocationInfo usrlocation = Input.location.lastData;
			_spawnedObjects = new List<GameObject>();
			for (int i = 0; i < 5; i++)
			{
				float la = usrlocation.latitude;
				float lo = usrlocation.longitude;
				// 랜덤한 위도 및 경도 생성
				double randomLat = UnityEngine.Random.Range(la-0.0010f, la+0.0010f);
				double randomLon = UnityEngine.Random.Range(lo-0.0010f,lo+0.0010f);

				_locations[i] = new Vector2d(randomLat, randomLon);

				var instance = Instantiate(_markerPrefab);
				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				instance.transform.localScale = new Vector3(_spawnScale, 0.01f, _spawnScale);
				_spawnedObjects.Add(instance);
			}
			/*	_locations = new Vector2d[_locationStrings.Length];
				_spawnedObjects = new List<GameObject>();
				for (int i = 0; i < _locationStrings.Length; i++)
				{
					var locationString = _locationStrings[i];
					_locations[i] = Conversions.StringToLatLon(locationString);
					var instance = Instantiate(_markerPrefab);
					instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
					instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
					_spawnedObjects.Add(instance);
				}*/
		}

		private void Update()
		{
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			}
		}
	}
}