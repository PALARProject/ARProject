using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;

public class FireBaseInitialize : MonoBehaviour {
    DatabaseReference databaseReference;
    private FirebaseAuth auth;
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if(task.Exception != null) {
                Debug.LogError($"Firebase initialization failed: {task.Exception}");
                return;
            }

            // Firebase 실시간 데이터베이스 초기화
            //databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            //auth = FirebaseAuth.DefaultInstance;
            // 데이터 쓰기 호출
            //ReadDataFromRealtimeDatabase( new string[] {"아이템","아이템 이름"});
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
