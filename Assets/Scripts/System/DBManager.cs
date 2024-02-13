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
                        foreach (var pair in itemData)
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
                        Debug.Log(itemName + ": ItemData read successfully:");
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
            Debug.Log(itemName + ": Error");
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
                Dictionary<int, float> list = new Dictionary<int, float>();
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
                        Debug.Log("Item Data read successfully:" + UID);
                    }
                    else
                    {
                        Debug.LogWarning("No Item data found at the specified location.");
                    }
                }
                else
                {
                    Debug.LogError("Error reading Item data: " + task.Exception);
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
    public async Task<int> GetUserQuestInfo()
    {
        string UID = PlayerPrefs.GetString("UID");
        Dictionary<string, object> questData = new Dictionary<string, object>();
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference;
        userRef = userRef.Child("플레이어").Child(UID).Child("퀘스트");
        await userRef.GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    questData = snapshot.Value as Dictionary<string, object>;
                    Debug.Log("Data read Quest successfully:" + UID);
                }
                else
                {
                    Debug.LogWarning("No Quest data found at the specified location.");
                }
            }
            else
            {
                Debug.LogError("Error reading Quest data: " + task.Exception);
            }
        });
        int n = 0;
        foreach (var pair in questData)
        {
            if ((bool)pair.Value == false)
                GameManager.instance.UserInfo.haveQuest.Add(n, int.Parse(pair.Key.Split("_")[1]));
            n++;
        }
        return 1;
    }
    public async Task<QuestInfo> GetQuestInfo(int questId)
    {
        try
        {

            QuestInfo result = new QuestInfo();
            DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference;
            userRef = userRef.Child("퀘스트").Child("퀘스트 번호").Child(questId.ToString());
            await userRef.GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        Dictionary<string, object> questData = snapshot.Value as Dictionary<string, object>;
                        Debug.Log(questId + ": Quest Data read successfully");
                        result.questId = questId;
                        result.title = questData["이름"].ToString();
                        result.desc = questData["설명"].ToString();
                    }
                    else
                    {
                        Debug.LogWarning("No data found at the specified location.");
                    }
                }
                else
                {
                    Debug.LogError("Error reading data: " + task.Exception);
                    result.questId = -1;
                }
            });
            return result;
        }
        catch
        {
            Debug.Log(questId + ": Error");
            return new QuestInfo();
        }
    }

    public async Task<string[]> GetQuestCompenInfo(int questId)
    {
        try
        {
            string[] compenItem = new string[0];
            DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference;
            userRef = userRef.Child("퀘스트").Child("퀘스트 번호").Child(questId.ToString()).Child("보상");
            await userRef.GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        Dictionary<string, object> questData = snapshot.Value as Dictionary<string, object>;
                        Debug.Log(questId + ": Quest Compen Data read successfully");
                        compenItem = new string[questData.Count];
                        int n = 0;
                        foreach (var pair in questData)
                        {
                            compenItem[n] = pair.Key.ToString();
                            n++;
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
            return compenItem;
        }
        catch
        {
            Debug.Log(questId + ": Error");
            return null;
        }
    }
    public async Task<Dictionary<string, object>> GetUserQuestInfo(string UID)
    {
        try
        {
            Dictionary<string, object> userData = new Dictionary<string, object>();
            DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference;
            userRef = userRef.Child("플레이어").Child(UID).Child("퀘스트");
            await userRef.GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        userData = snapshot.Value as Dictionary<string, object>;
                        Debug.Log("Data read successfully:" + UID);
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
            return userData;
        }
        catch
        {
            return null;
        }
    }
    public async Task<int> UpdateUserInfo(string userName, int inventoryNum, string itemName)
    {
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference;
        // string[] childs를 사용하여 경로 설정
        userRef = userRef.Child("플레이어").Child(userName).Child("인벤토리");
        // 업데이트할 데이터
        Dictionary<string, object> updates = new Dictionary<string, object>();
        updates.Add("box_" + string.Format("{0:D3}", inventoryNum + 1), itemName);

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
        return 1;
    }
    public async Task<int> UpdateQuestInfo(string userName, int questId)
    {
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference;
        // string[] childs를 사용하여 경로 설정
        userRef = userRef.Child("플레이어").Child(userName).Child("퀘스트");
        // 업데이트할 데이터
        Dictionary<string, object> updates = new Dictionary<string, object>();
        updates.Add("q_" + questId, true);

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
        return 1;
    }
}