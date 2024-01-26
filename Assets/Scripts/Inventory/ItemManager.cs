using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ItemManager : MonoBehaviour
{
    private List<GameObject> DropItems;
    public async Task<GameObject> DropItem(int mobId)
    {
        if (true)
        {
            DropTable dropTableId=await GameManager.instance.DBManager.GetDropTableAsync(mobId);
            List<KeyValuePair<int, float>> itemList = new List<KeyValuePair<int, float>>(dropTableId.dropItems);
            itemList.Sort((a, b) => { 
                if (a.Value < b.Value)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            });

            float randomNum=Random.Range(0, 1000)/10.0f;
            float sum = 0;
            foreach(KeyValuePair<int,float> item in itemList)
            {
                sum += item.Value;
                if(randomNum < sum)
                {
                    ItemInfo itemInfo=await GameManager.instance.DBManager.GetItemTable(item.Key);
                    GameObject obj=Resources.Load<GameObject>("item/" + itemInfo.itemId);
                    DropItems.Add(obj);
                    return obj;
                }
            }

        }
        return null;
    }
    public void FindResource(string path)
    {
        try
        {
            Object obj=Resources.Load(path);
            return;
        }
        catch
        {
            return;
        }
    }
}
public class ItemInfo
{
    public int itemId=-1;
    public string name="";
    public int category=-1;
    public int grade=-1;
    public Status status=new Status();
    public ItemInfo() { }
    public ItemInfo(int _itemId,string _name,int _category, int _grade,Status _status)
    {
        itemId = _itemId;
        name = _name;
        grade = _grade;
        category = _category;
        status = _status;
    }
    public ItemInfo DeepCopy()
    {
        ItemInfo newCopy = new ItemInfo();
        newCopy.itemId = this.itemId;
        newCopy.name = this.name;
        newCopy.grade = this.grade;
        newCopy.category = this.category;
        newCopy.status = this.status;
        return newCopy;
    }
}
public class Status
{
    public float hp=0;
    public float ap=0;
    public float dp = 0;
    public Status() { }
    public Status(float _hp, float _ap, float _dp)
    {
        hp = _hp;
        ap = _ap;
        dp = _dp;
    }
}