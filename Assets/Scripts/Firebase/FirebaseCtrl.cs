using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

public class Info
{
    public object price;
    public object soldOut;

    public Info(object _price, object _soldOut)
    {
        this.price = _price;
        this.soldOut = _soldOut;
    }

}
public class FirebaseCtrl : MonoBehaviour
{
    public Dictionary<object, Info> productDict;
    public static FirebaseCtrl _Instance;
    DataSnapshot snapshot;
    private void Awake()
    {
        _Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        productDict = new Dictionary<object, Info>();
        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance.GetReference("product").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("������ �������� ����");
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                Debug.Log(snapshot.ChildrenCount);
                // Do something with snapshot...
                foreach (DataSnapshot data in snapshot.Children)
                {
                    IDictionary productInfo = (IDictionary)data.Value;
                    Debug.Log("�̸�: " + productInfo["name"] + ", ǰ������ : " + productInfo["soldOut"]);
                    productDict.Add(productInfo["name"], new Info(productInfo["price"], productInfo["soldOut"]));
                }
                if (productDict.Count == 0)
                {
                    Debug.Log("������ �ȹ޾������~~");
                }
                //Debug.Log(productDict["milk"].price);
            }
        });
    }

    IEnumerator DataToDict()
    {
        Debug.Log("DataToDict ����");
        while (productDict.Count == 0) //������ �ȹ޾�������
        {
            Debug.Log("productDict.Count�� 0��/");

            foreach (DataSnapshot data in snapshot.Children)
            {
                IDictionary productInfo = (IDictionary)data.Value;
                Debug.Log("�̸�: " + productInfo["name"] + ", ǰ������ : " + productInfo["soldOut"]);
                productDict.Add(productInfo["name"], new Info(productInfo["price"], productInfo["soldOut"]));
                yield return null;
            }
            if (productDict.Count == 0)
            {
                Debug.Log("������ �ȹ޾������~~");
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

}
