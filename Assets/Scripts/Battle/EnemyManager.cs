using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> EnemyPrefabs;
    void Start()
    {
        EnemyPrefabs = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
