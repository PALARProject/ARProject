using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;

public class DBManager : MonoBehaviour
{
    DatabaseReference databaseReference;
    // Start is called before the first frame update
    async void Start()
    {
        // Firebase 초기화
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null)
            {
                Debug.LogError($"Firebase initialization failed: {task.Exception}");
                return;
            }

            // Firebase 실시간 데이터베이스 초기화
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

            // 데이터 쓰기 호출
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public async Task<List<ItemInfo>> GetItemsTable()
    {
        try
        {
            List<ItemInfo> resultList = new List<ItemInfo>();
            DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference;
            userRef = userRef.Child("아이템").Child("아이템 이름");
            await userRef.GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        Dictionary<string, object> itemData = snapshot.Value as Dictionary<string, object>;
                        Debug.Log("ItemData read successfully");
                        foreach(var pair in itemData)
                        {
                            Dictionary<string, object> item = pair.Value as Dictionary<string, object>;
                            ItemInfo result = new ItemInfo();
                            result.itemId = (int)(long)item["아이템코드"];
                            result.name = pair.Key;
                            result.category = item["아이템 카테고리"].ToString();
                            result.grade = (int)(long)item["아이템 희귀도"];
                            result.description = item["아이템 설명"].ToString();
                            result.status = new Status((int)(long)item["아이템 공격력"], (int)(long)item["아이템 보호막"]);

                            resultList.Add(result);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No data found at the specified location.");
                    }
                }
                else
                {
                    Debug.LogError("Error reading data: " + task.Exception);
                }
            });
            return resultList;
        }
        catch
        {
            Debug.Log("item list Error");
            return null;
        }
    }
    public async Task<ItemInfo> GetItemTable(string itemName)
    {
        try
        {
            ItemInfo result = new ItemInfo();
            DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference;
            userRef = userRef.Child("아이템").Child("아이템 이름").Child(itemName);
            await userRef.GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        Dictionary<string, object> itemData = snapshot.Value as Dictionary<string, object>;
                        Debug.Log(itemName+": ItemData read successfully:");    
                        result.itemId = (int)(long)itemData["아이템코드"];
                        result.name = itemName;
                        result.category = itemData["아이템 카테고리"].ToString();
                        result.grade = (int)(long)itemData["아이템 희귀도"];
                        result.description = itemData["아이템 설명"].ToString();
                        result.status = new Status((int)(long)itemData["아이템 공격력"], (int)(long)itemData["아이템 보호막"]);
                    }
                    else
                    {
                        Debug.LogWarning("No data found at the specified location.");
                    }
                }
                else
                {
                    Debug.LogError("Error reading data: " + task.Exception);
                }
            });
            return result;
        }
        catch
        {
            Debug.Log(itemName+": Error");
            return new ItemInfo();
        }
    }
    public async Task<DropTable> GetDropTableAsync(int mobId)
    {
        try
        {
            UnityWebRequest uwr = UnityWebRequest.Get("https://www.localhost:3000/");
            UnityWebRequestAsyncOperation ao = uwr.SendWebRequest();
            await ao;
            if (ao.isDone)
            {
                Dictionary<int, float> list=new Dictionary<int, float>();
                //테스트 코드
                list.Add(1, 20);
                return new DropTable(1, list);
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
    }
    public async Task<UserInfo> GetUserInfo(string UID)
    {
        Debug.Log(UID);
        try
        {
            UserInfo result = new UserInfo();
            Dictionary<string, object> userData = new Dictionary<string, object>();
            DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference;
            userRef = userRef.Child("플레이어").Child(UID).Child("인벤토리");
            await userRef.GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        userData = snapshot.Value as Dictionary<string, object>;
                        result.userName = UID;
                        Debug.Log("Data read successfully:"+ UID);
                    }
                    else
                    {
                        Debug.LogWarning("No data found at the specified location.");
                    }
                }
                else
                {
                    Debug.LogError("Error reading data: " + task.Exception);
                }
            });
            for (int i = 0; i < userData.Count; i++)
            {
                int index = i;
                result.inventoryItems.Add(index, await GameManager.instance.DBManager.GetItemTable(userData["box_" + string.Format("{0:D3}", index + 1)].ToString()));
            }
            return result;
        }
        catch
        {
            return new UserInfo();
        }
    }

    public async void UpdateUserInfo(string userName, string inventoryNum,string itemName)
    {
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference;
        // string[] childs를 사용하여 경로 설정
        userRef = userRef.Child("플레이어").Child(userName).Child("인벤토리").Child(inventoryNum);
        // 업데이트할 데이터
        Dictionary<string, object> updates = new Dictionary<string, object>();
        updates.Add(inventoryNum, itemName);

        await userRef.UpdateChildrenAsync(updates).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log("Data updated successfully.");
            }
            else
            {
                Debug.LogError("Error updating data: " + task.Exception);
            }
        });
    }
}
