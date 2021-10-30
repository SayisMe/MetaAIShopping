using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class Product
{
    public static Material origin_mat;
    public static bool highlight_distance;
}*/
public class ObjectHighlight : MonoBehaviour
{
    public Material highlight; //�ܰ��� material
    public float highlight_distance = 2.0f;

    private Material origin_mat; //���� material
    private GameObject Player;
    private bool highlight_bool; //�ܰ����� �ִ��� ������

    // Start is called before the first frame update
    void Start()
    {
        highlight_bool = false; //ó���� �ܰ��� ����.
                                // highlight_distance = 3.0f;
        Player = GameObject.FindWithTag("Player");
        origin_mat = this.GetComponent<MeshRenderer>().material; //ó�� material ����
        this.GetComponent<Renderer>().materials = new Material[2] { origin_mat, origin_mat };
    }

    // Update is called once per frame
    void Update()
    {
        setOutline(getDistance());

    }
    //�Ÿ�����Լ�
    float getDistance()
    {
        float distance = Vector3.Distance(Player.transform.position, this.transform.position);

        return distance;
    }
    //�ܰ��� ����(�Ÿ��� 3���ϸ�)
    void setOutline(float distance)
    {
        //�Ÿ��� 3�����̰� ���� �ܰ��� ������
        if (distance < highlight_distance && !highlight_bool)
        {
            Debug.Log("�Һ�");
            //meshRenderer material�� �ܰ���,���� mat���� ����
            // �ܰ��� mat�� ���� �ְ� �� ���� ���� mat�� ���� ����� ����
            this.GetComponent<MeshRenderer>().material = highlight;
            highlight_bool = true;
        }
        //�Ÿ��� 3 �̻��̰� ���� �ܰ��� ������
        else if (distance >= highlight_distance && highlight_bool)
        {
            this.GetComponent<MeshRenderer>().material = origin_mat;
            highlight_bool = false;
        }
    }
    /* public Material highlight; //�ܰ��� material
     public float highlight_distance = 3.0f;

     private Material origin_mat; //���� material
     private GameObject[] Product;
     private bool[] highlight_bool; //�ܰ����� �ִ��� ������

     // Start is called before the first frame update
     void Start()
     {
        // highlight_distance = 3.0f;
         Product = GameObject.FindGameObjectsWithTag("product");
         highlight_bool = new bool[Product.Length];
         for (int i = 0; i < Product.Length; i++)
         {
            // Product[i] = new Product();
             highlight_bool[i] = false; //ó���� �ܰ��� ����.
             origin_mat = Product[i].GetComponent<MeshRenderer>().material; //ó�� material ����
             Product[i].GetComponent<Renderer>().materials = new Material[2] { origin_mat, origin_mat };
         }

     }

     // Update is called once per frame
     void Update()
     {
         //setOutline(getDistance());
         for (int i = 0; i < Product.Length; i++)
         {
             setOutline(getDistance(Product[i]), Product[i], highlight_bool[i]);
         }
     }
     //�Ÿ�����Լ�
     float getDistance(GameObject gameObject)
     {
         float distance  = Vector3.Distance(gameObject.transform.position, this.transform.position);

         return distance;
     }
     //�ܰ��� ����(�Ÿ��� 3���ϸ�)
     void setOutline(float distance, GameObject gameObject, bool highlight_bool)
     {
         Debug.Log("distance: " + distance + ", highlight : " + highlight_bool);
         //�Ÿ��� 3�����̰� ���� �ܰ��� ������
         if (distance < highlight_distance && !highlight_bool)
         {
             Debug.Log("�Һ�");
             //meshRenderer material�� �ܰ���,���� mat���� ����
             // �ܰ��� mat�� ���� �ְ� �� ���� ���� mat�� ���� ����� ����
             gameObject.GetComponent<MeshRenderer>().material = highlight;
             highlight_bool = true;
         }
         //�Ÿ��� 3 �̻��̰� ���� �ܰ��� ������
         else if(distance >= highlight_distance && highlight_bool)
         {
             gameObject.GetComponent<MeshRenderer>().material = origin_mat;
             highlight_bool = false;
         }
     }
    */
}
