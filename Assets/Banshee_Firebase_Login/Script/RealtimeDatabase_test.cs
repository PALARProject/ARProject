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
        // Firebase �ʱ�ȭ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if(task.Exception != null) {
                Debug.LogError($"Firebase initialization failed: {task.Exception}");
                return;
            }

            // Firebase �ǽð� �����ͺ��̽� �ʱ�ȭ
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

            // ������ ���� ȣ��
        });
    }


    void ReadDataFromRealtimeDatabase(string[] childs) {
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference;
        // string[] childs�� ����Ͽ� ��� ����
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
        // string[] childs�� ����Ͽ� ��� ����
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
        // string[] childs�� ����Ͽ� ��� ����
        foreach(string child in childs) {
            userRef = userRef.Child(child);
        }

        // �����ͺ��̽��� �� ������ ����
       // userData = new Dictionary<string, object>
       // {
       //     { "name", "Jane Smith" },
       //     { "age", 25 },
       //     { "email", "janesmith@example.com" }
       // };

        // �����ͺ��̽��� ������ ����
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
        // string[] childs�� ����Ͽ� ��� ����
        foreach(string child in childs) {
            userRef = userRef.Child(child);
        }

        // ������Ʈ�� ������
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
        // ȸ������ ���� ����

        // ȸ�������� �����ϸ� �����ͺ��̽��� �÷��̾� �����͸� ����
        string[] playerPath = { "�÷��̾�", username }; // �ݽÿ� ���� �̸��� ����ڿ� �ش��ϴ� ��� ����
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        string uid = auth.CurrentUser.UserId;
        Dictionary<string, object> playerData = new Dictionary<string, object>
        {
        { "���� ����", new Dictionary<string, object>
            {
                { "UID", uid },
                { "��й�ȣ", password },
                { "�̸���", email }
            }
        },
        { "�κ��丮", new Dictionary<string, object>
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