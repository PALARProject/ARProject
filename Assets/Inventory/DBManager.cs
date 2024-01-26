using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class DBManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public async Task<ItemInfo> GetItemTable(int itemId)
    {
        try
        {
            UnityWebRequest uwr = UnityWebRequest.Get("https://www.localhost:3000/");
            UnityWebRequestAsyncOperation ao = uwr.SendWebRequest();
            await ao;
            if (ao.isDone)
            {
                return null;
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
    public async Task<string> GetUserInfo(int userId)
    {
        try
        {
            UnityWebRequest uwr = UnityWebRequest.Get("https://www.localhost:3000/");
            UnityWebRequestAsyncOperation ao = uwr.SendWebRequest();
            await ao;
            if (ao.isDone)
            {
                return "";
            }
            else
            {
                return "";
            }
        }
        catch
        {
            return "";
        }
    }

    public IEnumerator UpdateUserInfo(int userId, byte[] updateData)
    {
        //byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
        UnityWebRequest uwr = UnityWebRequest.Put("https://www.localhost:3000/", updateData);
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            Debug.Log("Upload complete!");
        }
        uwr.Dispose();
    }
}

public class DropTable
{
    public int itemId;
    public Dictionary<int, float> dropItems;
    public DropTable(int _itemId,Dictionary<int,float> _dropItems)
    {
        this.itemId = _itemId;
        this.dropItems = _dropItems;
    }
}