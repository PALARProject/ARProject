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

            // Firebase �ǽð� �����ͺ��̽� �ʱ�ȭ
            //databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            //auth = FirebaseAuth.DefaultInstance;
            // ������ ���� ȣ��
            //ReadDataFromRealtimeDatabase( new string[] {"������","������ �̸�"});
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
