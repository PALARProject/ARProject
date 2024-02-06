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
        // Firebase �ʱ�ȭ
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null)
            {
                Debug.LogError($"Firebase initialization failed: {task.Exception}");
                return;
            }

            // Firebase �ǽð� �����ͺ��̽� �ʱ�ȭ
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

            // ������ ���� ȣ��
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
            userRef = userRef.Child("������").Child("������ �̸�");
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
                            result.itemId = (int)(long)item["�������ڵ�"];
                            result.name = pair.Key;
                            result.category = item["������ ī�װ�"].ToString();
                            result.grade = (int)(long)item["������ ��͵�"];
                            result.description = item["������ ����"].ToString();
                            result.status = new Status((int)(long)item["������ ���ݷ�"], (int)(long)item["������ ��ȣ��"]);

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
            userRef = userRef.Child("������").Child("������ �̸�").Child(itemName);
            await userRef.GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        Dictionary<string, object> itemData = snapshot.Value as Dictionary<string, object>;
                        Debug.Log(itemName+": ItemData read successfully:");    
                        result.itemId = (int)(long)itemData["�������ڵ�"];
                        result.name = itemName;
                        result.category = itemData["������ ī�װ�"].ToString();
                        result.grade = (int)(long)itemData["������ ��͵�"];
                        result.description = itemData["������ ����"].ToString();
                        result.status = new Status((int)(long)itemData["������ ���ݷ�"], (int)(long)itemData["������ ��ȣ��"]);
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
                //�׽�Ʈ �ڵ�
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
            userRef = userRef.Child("�÷��̾�").Child(UID).Child("�κ��丮");
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
        // string[] childs�� ����Ͽ� ��� ����
        userRef = userRef.Child("�÷��̾�").Child(userName).Child("�κ��丮").Child(inventoryNum);
        // ������Ʈ�� ������
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
