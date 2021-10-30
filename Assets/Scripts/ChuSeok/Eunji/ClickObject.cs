using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickObject : MonoBehaviour
{
    public Button soldOutBtn;
    public Button buyBtn;
    public Text priceText;
    public Text nameText;
    public Text sumPriceText;

    public Button UpBtn;
    public Button DownBtn;
    private int BuyCnt = 0;
    public Text BuyCntText;
    public GameObject content; // īƮ ����Ʈ���� Content
    public int sumPrice = 0;

    public static ClickObject _Instance;
    public GameObject buyListOnePrefab;
    private void Awake()
    {
        _Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);
            if(hit.collider != null)
            {
                GameObject CurrentTouch = hit.transform.gameObject;
                Debug.Log("Ŭ���� ������Ʈ �̸� : " + hit.transform.gameObject.name);
                //Ŭ���� ������Ʈ �̸����� ����, ����, (,) ����� 
                //�̸��� ���̾�̽� �ȿ� �ִ� name�� �̸��� ���ٸ�, 
                if (FirebaseCtrl._Instance.productDict.ContainsKey(hit.transform.gameObject.name))
                {
                    Debug.Log("ǰ���ΰ�? " + FirebaseCtrl._Instance.productDict[hit.transform.gameObject.name].soldOut.ToString());
                    nameText.text = hit.transform.gameObject.name;
                    priceText.text = FirebaseCtrl._Instance.productDict[hit.transform.gameObject.name].price.ToString();
                    UICtrl._Instance.OpenClose_Product_Info();
                    //���̾�̽����� ����, ǰ������ �ƴ��� �޾ƿ´�.
                   if ( FirebaseCtrl._Instance.productDict[hit.transform.gameObject.name].soldOut.ToString() == "1")
                    {
                        //ǰ�� ��ư Ȱ��ȭ
                        soldOutBtn.gameObject.SetActive(true);
                        //���Ź�ư ��Ȱ��ȭ
                        buyBtn.gameObject.SetActive(false);
                        //���� üũ ���ϰ�
                        Debug.Log("�ش� ��ǰ�� ǰ�� ��ǰ�Դϴ�");
                    }
                    else
                    {
                        soldOutBtn.gameObject.SetActive(false);
                        buyBtn.gameObject.SetActive(true);
                        //���Ź�ư Ȱ��ȭ
                        Debug.Log("���� ������ ��ǰ�Դϴ�.");

                    }
                }
            }
        }
    }

    /*
    //UP��ư ������ ���ż��� �ø���
    public void Up_Click()
    {
        BuyCnt = int.Parse(BuyCntText.text);
        BuyCnt++;
        BuyCntText.text = BuyCnt.ToString();
    }
    //Down��ư ������ ���ż��� ���̱�
    public void Down_Click()
    {
        BuyCnt = int.Parse(BuyCntText.text);
        if (BuyCnt > 0)
        {
            BuyCnt--;
        }
        else
        {
            Debug.Log("���ż����� 0 ���ϰ� �Ұ����մϴ�.");
        }
        BuyCntText.text = BuyCnt.ToString();

    }


    public void ClickBuyButton()
    {
        if (BuyCnt > 0)
        {
            //������ �߰������Ѵ����� �θ� �佺ũ�� Content�� �Ѵ�.
            //GameObject buyListOne = Instantiate(Resources.Load<GameObject>("Prefabs/BuyListOne"));
            GameObject buyListOne = Instantiate(buyListOnePrefab);
            //buyListOne.transform.parent.gameObject.GetComponent<BuyListOne>();
            buyListOne.transform.SetParent(content.transform, false);
            int priceResult = int.Parse(BuyCntText.text) * int.Parse(priceText.text);
            sumPrice += priceResult;
            buyListOne.GetComponent<BuyListOne>().nameText.text = nameText.text;
            buyListOne.GetComponent<BuyListOne>().buyCntText.text = BuyCntText.text;
            buyListOne.GetComponent<BuyListOne>().priceText.text = priceResult.ToString() + "��";
            sumPriceText.text = sumPrice.ToString() + "��";
        }
        else
        {
            Debug.Log("���ż����� 0�Դϴ�.");
        }
        UICtrl._Instance.Product_Info_Panel.SetActive(false);

        //  buyListOne.SetParent(content);
        //��ٱ��Ͽ� �� ����Ʈ�Ѱ��� �̸�, ���� �ִ´�!
        //���⼭�� ��ҹ�ư ������ �ֵ���..?
        //������ ��ü ������ ��� �ؾ��ҵ�

    }
    */
}
