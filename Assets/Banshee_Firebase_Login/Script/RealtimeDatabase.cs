using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;


public class RealtimeDatabase : MonoBehaviour {
  
    Dictionary<string, object> SelectData;
    void Start() {
        //ReadDataFromRealtimeDatabase(new string[] { "������", "������ �̸�"});
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
    public void WriteDataToRealtimeDatabase(string[] childs, Dictionary<string, object> userData ) {
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



  
}