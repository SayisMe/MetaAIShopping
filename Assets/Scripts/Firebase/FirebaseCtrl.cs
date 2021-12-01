using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;
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
    private bool IsReceivedData;
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
                UICtrl._Instance.debugText.text += "\n���̾�̽� ������ �������� ����";
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                Debug.Log(snapshot.ChildrenCount);
                // Do something with snapshot...
                foreach (DataSnapshot data in snapshot.Children)
                {
                    try
                    {
                        IDictionary productInfo = (IDictionary)data.Value;
                        Debug.Log("�̸�: " + productInfo["name"] + ", ǰ������ : " + productInfo["soldOut"]);
                        productDict.Add(productInfo["name"], new Info(productInfo["price"], productInfo["soldOut"]));
                        IsReceivedData = true;
                    }
                    catch(Exception ex)
                    {
                        Debug.Log(ex.ToString());
                    }
                    
                }
                if (productDict.Count == 0)
                {
                    Debug.Log("������ �ȹ޾������~~");
                    UICtrl._Instance.debugText.text += "\n���̾�̽� �� �޾���";
                    IsReceivedData = false;
                }
                if (productDict.ContainsKey("water"))
                {
                    Debug.Log("����� �޾������~~");
                    UICtrl._Instance.debugText.text += "\n���̾�̽� �� �޾���";
                    IsReceivedData = false;
                    productDict.Clear();
                }
                if (!IsReceivedData)
                {
                    NoticeCtrl._Instance.CreateNotice("��ǰ �����Ͱ� �ùٸ��� ������Ʈ���� �ʾҽ��ϴ�.\n��Ʈ��ũ�� Ȯ���ϰ� �ٽ� �������ּ���.", true);
                }
                //Debug.Log(productDict["milk"].price);
            }
        });
        InvokeRepeating("DataToDict", 3.0f ,3.0f );
    }
    private void DataToDict()
    {
        Debug.Log("DataToDict ����");
        UICtrl._Instance.debugText.text += "\nDataToDict ����";
        if (!IsReceivedData) //������ �ȹ޾�������
        {
            Debug.Log("productDict.Count�� 0��/");

            foreach (DataSnapshot data in snapshot.Children)
            {
                IDictionary productInfo = (IDictionary)data.Value;
                Debug.Log("�̸�: " + productInfo["name"] + ", ǰ������ : " + productInfo["soldOut"]);
                productDict.Add(productInfo["name"], new Info(productInfo["price"], productInfo["soldOut"]));
            }
            if (productDict.Count == 0)
            {
                Debug.Log("������ �ȹ޾������~~");
                IsReceivedData = false;
            }
            if (productDict.ContainsKey("water"))
            {
                Debug.Log("����� �޾������~~");
                IsReceivedData = false;
                productDict.Clear();
            }
            Debug.Log(IsReceivedData);
        }
        if (IsReceivedData)
        {
            CancelInvoke("DataToDict");
        }
  
    }

}
