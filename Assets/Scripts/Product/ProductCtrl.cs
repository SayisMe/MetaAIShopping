using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//��ư ������ ���� ��ȭ�ϴ� ��ũ��Ʈ
//Product_Info_Pannel ���� ��ũ��Ʈ
public class ProductCtrl : MonoBehaviour
{
    public Button UpBtn;
    public Button DownBtn;
    private int BuyCnt;
    public Text BuyCntText;
    public Text priceText;
    public Text nameText;
    public Text sumPriceText;
    public Text sumPriceText_buy;
    public GameObject cartContent; // īƮ ����Ʈ���� Content
    public GameObject buyContent; // ���� ����Ʈ���� Content
    public int sumPrice = 0;

    public static ProductCtrl _Instance;
    public GameObject cartListOnePrefab;
    public GameObject buyListOnePrefab;

    private void Awake()
    {
        _Instance = this;
    }
 
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
            GameObject cartListOne = Instantiate(cartListOnePrefab);
            GameObject buyListOne = Instantiate(buyListOnePrefab);
            //buyListOne.transform.parent.gameObject.GetComponent<BuyListOne>();
            cartListOne.transform.SetParent(cartContent.transform, false);
            buyListOne.transform.SetParent(buyContent.transform, false);
            int priceResult = int.Parse(BuyCntText.text) * int.Parse(priceText.text);
            sumPrice += priceResult;
            cartListOne.GetComponent<BuyListOne>().nameText.text = nameText.text;
            buyListOne.GetComponent<BuyListOne>().nameText.text = nameText.text + " x " + BuyCntText.text;
            cartListOne.GetComponent<BuyListOne>().buyCntText.text = BuyCntText.text;
            cartListOne.GetComponent<BuyListOne>().priceText.text = priceResult.ToString() + "��";
            buyListOne.GetComponent<BuyListOne>().priceText.text = priceResult.ToString() + "��";
            sumPriceText.text = sumPrice.ToString() + "��";
            sumPriceText_buy.text = sumPrice.ToString() + "��";
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



}
