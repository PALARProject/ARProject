using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;


public class RealtimeDatabase_test : MonoBehaviour {
    DatabaseReference databaseReference;
    Dictionary<string, object> SelectData;
    void Start() {
        // Firebase 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if(task.Exception != null) {
                Debug.LogError($"Firebase initialization failed: {task.Exception}");
                return;
            }

            // Firebase 실시간 데이터베이스 초기화
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

            // 데이터 쓰기 호출
        });
    }


    void ReadDataFromRealtimeDatabase(string[] childs) {
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference;
        // string[] childs를 사용하여 경로 설정
        foreach(string child in childs) {
            userRef = userRef.Child(child);
        }
        userRef.GetValueAsync().ContinueWithOnMainThread(task => {
            if(task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                if(snapshot.Exists) {
                    Dictionary<string, object> userData = snapshot.Value as Dictionary<string, object>;
                    Debug.Log("Data read successfully:");
                    foreach(var pair in userData) {
                        Debug.Log($"{pair.Key}: {pair.Value}");
                    }
                } else {
                    Debug.LogWarning("No data found at the specified location.");
                }
            } else {
                Debug.LogError("Error reading data: " + task.Exception);
            }
        });
    }

    void DeleteDataFromRealtimeDatabase(string[] childs) {
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference; 
        // string[] childs를 사용하여 경로 설정
        foreach(string child in childs) {
            userRef = userRef.Child(child);
        }

    

        userRef.RemoveValueAsync().ContinueWithOnMainThread(task => {
            if(task.IsCompleted) {
                Debug.Log("Data deleted successfully.");
            } else {
                Debug.LogError("Error deleting data: " + task.Exception);
            }
        });
    }
    void WriteDataToRealtimeDatabase(string[] childs, Dictionary<string, object> userData ) {
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference; 
        // string[] childs를 사용하여 경로 설정
        foreach(string child in childs) {
            userRef = userRef.Child(child);
        }

        // 데이터베이스에 쓸 데이터 생성
       // userData = new Dictionary<string, object>
       // {
       //     { "name", "Jane Smith" },
       //     { "age", 25 },
       //     { "email", "janesmith@example.com" }
       // };

        // 데이터베이스에 데이터 쓰기
        userRef.SetValueAsync(userData).ContinueWithOnMainThread(task => {
            if(task.IsCompleted) {
                Debug.Log("Data written to Realtime Database successfully.");
            } else {
                Debug.LogError("Error writing data to Realtime Database: " + task.Exception);
            }
        });
    }

    void UpdateDataInRealtimeDatabase(string[] childs, Dictionary<string, object> updates) {
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference; 
        // string[] childs를 사용하여 경로 설정
        foreach(string child in childs) {
            userRef = userRef.Child(child);
        }

        // 업데이트할 데이터
        //updates = new Dictionary<string, object>
        //{
       ///     { "name", "Banshee" },
       //     { "age", 26 },
        //    { "email", "sou04036@naver.com" }
       // };

        userRef.UpdateChildrenAsync(updates).ContinueWithOnMainThread(task => {
            if(task.IsCompleted) {
                Debug.Log("Data updated successfully.");
            } else {
                Debug.LogError("Error updating data: " + task.Exception);
            }
        });
    }
    public void MakeDictionaryData(string Key, object Data){
        SelectData.Add(Key, Data);

    }



    public void SignUp_InventoryMaker(string username, string email, string password) {
        // 회원가입 로직 수행

        // 회원가입이 성공하면 데이터베이스에 플레이어 데이터를 쓰기
        string[] playerPath = { "플레이어", username }; // 반시와 같은 이름의 사용자에 해당하는 경로 생성
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        string uid = auth.CurrentUser.UserId;
        Dictionary<string, object> playerData = new Dictionary<string, object>
        {
        { "계정 정보", new Dictionary<string, object>
            {
                { "UID", uid },
                { "비밀번호", password },
                { "이메일", email }
            }
        },
        { "인벤토리", new Dictionary<string, object>
            {
                { "box_001", "null" },
                { "box_002", "null" },
                { "box_003", "null" },
                { "box_004", "null" },
                { "box_005", "null" },
                { "box_006", "null" },
                { "box_007", "null" },
                { "box_008", "null" },
                { "box_009", "null" }
            }
        }
    };

        WriteDataToRealtimeDatabase(playerPath, playerData);
    }
}