namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;
	using UnityEngine.SceneManagement;


	public class SpawnOnMapBoss : MonoBehaviour
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

		public List<GameObject> _spawnedObjects;

		Vector3 v3 = new Vector3(0, 1, 0);
		public RaycastHit Hit2;
		public int i;
		string location;
		public GameObject Montext;
		void Start()
		{
			_locations = new Vector2d[_locationStrings.Length];
			_spawnedObjects = new List<GameObject>();
			for (int i = 0; i < _locationStrings.Length; i++)
			{
				var locationString = _locationStrings[i];
				_locations[i] = Conversions.StringToLatLon(locationString);
				var instance = Instantiate(_markerPrefab);
				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
				_spawnedObjects.Add(instance);
			}			
		}
		public void FindLocationFromObject()
        {
			if (Hit2.collider.gameObject != null)
			{
				GameObject objectname = Hit2.collider.gameObject;
				for (int u = 0; u < _spawnedObjects.Count; u++)
				{
					objectname.transform.localPosition = _map.GeoToWorldPosition(_locations[u], true);
					if (_spawnedObjects[u] == objectname)
					{
						location = _locationStrings[u];
					}
				}
			}
        }

		private void Update()
		{
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true) + v3;
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			}
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);

				if (touch.phase == TouchPhase.Began)
				{
					Vector3 screen = new Vector3(touch.position.x, touch.position.y, 0);
					Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 120));
					Debug.Log("P" + touch.position);
					Debug.Log("M" + Input.mousePosition);
					Debug.Log(touchPos);
					//Vector3 touchPos = Camera.main.transform.position + new Vector3(touch.position.x,0,touch.position.y);

					Vector3 rayvec = touchPos - Camera.main.transform.position;

					RaycastHit hit;
					Physics.Raycast(Camera.main.transform.position, rayvec, out hit);
					Hit2 = hit;
					FindLocationFromObject();
					Debug.Log("d"+hit.collider);
					Debug.Log(Hit2.collider.name);
					Debug.Log(location);
					Debug.DrawRay(Camera.main.transform.position, rayvec, Color.red, 1f);
				}
				if (Hit2.collider != null && Hit2.collider.tag == "Enemy" && i == 1)
				{
					//SceneManager.LoadScene("BattleScene");
					Hit2.collider.gameObject.SetActive(false);
					//Debug.Log(Hit2.collider.gameObject.name);
				}
			}
		}
		public void OnTriggerStay(Collider collision)
		{
			if (collision.tag == "Enemy")
			{
				i = 1;
				Montext.SetActive(true);
				// Debug.Log(Hit2.collider.gameObject.name);
			}
		}
		public void OnTriggerExit(Collider other)
		{
			if (other.tag == "Enemy")
			{
				i = 0;
				Montext.SetActive(false);
			}
		}
	}
}