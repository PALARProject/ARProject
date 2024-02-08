using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections.Generic;
using UnityEngine;

public class DataToFirestoreLib {
    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;




    void WriteDataToFirestore() {
        DocumentReference docRef = db.Collection("users").Document("user123");
        Dictionary<string, object> user = new Dictionary<string, object>
        {
        { "name", "Jane Smith" },
        { "age", 25 },
        { "email", "janesmith@example.com" }
    };
        docRef.SetAsync(user).ContinueWithOnMainThread(task => {
            if(task.IsCompleted) {
                Debug.Log("Document written successfully.");
            } else {
                Debug.LogError("Error writing document: " + task.Exception);
            }
        });
    }
}