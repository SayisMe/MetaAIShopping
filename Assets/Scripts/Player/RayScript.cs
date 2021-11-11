using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
public class RayScript : MonoBehaviour
{
    public GraphicRaycaster graphicRaycaster; //Canvas�� �ִ� GraphicRaycaster
    private List<RaycastResult> raycastResults; //Raycast�� �浹�� UI���� ��� ����Ʈ
    private PointerEventData pointerEventData; //Canvas ���� ������ ��ġ �� ����
    LineRenderer layser;
    RaycastHit hit;
    Vector3 RayDir;
    float MaxDistance = 15f;
    public Image Info_Pannel;
    public Button soldOutBtn;
    public Button buyBtn;
    public Text name_text;
    public Text price_text;
   // public Text amount_text;
    public static bool IsPause = false;

    public Material highlight; //�ܰ��� material
    Material originalMaterial;
    GameObject lastHighlightedObject;


    public static RayScript _Instance;
    private void Awake()
    {
        _Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        layser = this.gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RayDir = transform.rotation * Vector3.forward;
        layser.SetPosition(0, transform.position); // ù��° ������ ��ġ
        layser.SetPosition(1, transform.position + MaxDistance * RayDir);

        // Mouse�� �������� Ray cast �� ��ȯ
        RaycastHit hit;

        Ray cast = Camera.main.ScreenPointToRay(Input.mousePosition);
     /*   if (!ClickObject._Instance.isNewSpace)
        {
            if (Physics.Raycast(cast, out hit)) //���콺 ����(����� ��)
            {
                //��ǰ�̸�
                if (hit.collider.gameObject.tag == "product")
                {
                    //�ܰ��� ���̰�
                    originalMaterial = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
                    HighlightObject(hit.collider.gameObject);
                }
                else
                {
                    ClearHighlighted();
                }
                // if(highlightObj != null)
                //      highlightObj.GetComponent<MeshRenderer>().material = default;
            }
        }
     */


        if (Physics.Raycast(transform.position, RayDir, out hit, MaxDistance)) //(Ray ����, Ray ����, �浹 ������ RaycastHit, Ray �Ÿ�(����))
        {
            //��ư �Ӽ��̸�
            if (hit.collider.gameObject.GetComponent<Button>() != null)
            {
                Button btn = hit.collider.gameObject.GetComponent<Button>();
                if (btn != null)
                {
                    btn.Select();
                }
            }
            if (!ViewCtrl._Instance.isNewSpace)
            {
                //��ǰ�̸�
                if (hit.collider.gameObject.tag == "product")
                {
                    //�ܰ��� ���̰�
                    originalMaterial = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
                    HighlightObject(hit.collider.gameObject);
                }
                else
                {
                    ClearHighlighted();
                }
            }
        }
        //test��
        if (Input.GetKeyDown(KeyCode.Z))
           SelectProduct_();
        
    }

    void HighlightObject(GameObject gameObject)
    {
        if (lastHighlightedObject != gameObject)
        {
            ClearHighlighted();
            Debug.Log(gameObject.name + " ���̶���Ʈ Ű��");
            gameObject.GetComponent<MeshRenderer>().materials = new Material[2]{  originalMaterial, highlight };
            //gameObject.GetComponent<MeshRenderer>().material = highlight;
            lastHighlightedObject = gameObject;
        }

    }

    void ClearHighlighted()
    {
        if (lastHighlightedObject != null)
        {
           // Debug.Log(lastHighlightedObject.name + " ���̶���Ʈ ���ֱ�");
            if(lastHighlightedObject.GetComponent<MeshRenderer>().materials.Length > 1)
                lastHighlightedObject.GetComponent<Renderer>().materials = new Material[1] { originalMaterial };
            lastHighlightedObject = null;
        }
    }
    public void SelectProduct(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.blue, 10f); //������ ���̸� ���̰� �ϴ� �뵵(�̰� ������ �Ⱥ���)
            if (Physics.Raycast(transform.position, RayDir, out hit, MaxDistance)) //(Ray ����, Ray ����, �浹 ������ RaycastHit, Ray �Ÿ�(����))
            {
                Debug.Log("Ray�浹����" + hit.transform.name);
                UICtrl._Instance.debugText.text += "\nRay�浹����";
                if (hit.collider.tag == "product")
                {
                    UICtrl._Instance.debugText.text += "\n�浹�Ѱ� �±װ� product��";
                    GameObject CurrentTouch = hit.collider.transform.gameObject;
                    Debug.Log("Ŭ���� ������Ʈ �̸� : " + hit.collider.transform.gameObject.name);
                    string productName = hit.collider.transform.gameObject.name;
                    productName = productName.Replace("(Clone)", "");
                    UICtrl._Instance.debugText.text += "\nŬ�� ������Ʈ �̸� " + productName + " ��";
                    //Ŭ���� ������Ʈ �̸����� ����, ����, (,) ����� 
                    //�̸��� ���̾�̽� �ȿ� �ִ� name�� �̸��� ���ٸ�, 
                    if (FirebaseCtrl._Instance.productDict.ContainsKey(productName))
                    {
                        UICtrl._Instance.debugText.text += "\n���̾�̽� �ȿ� ����";
                        Debug.Log("ǰ���ΰ�? " + FirebaseCtrl._Instance.productDict[productName].soldOut.ToString());
                        name_text.text = productName;
                        price_text.text = FirebaseCtrl._Instance.productDict[productName].price.ToString();
                        UICtrl._Instance.OpenClose_Product_Info();
                        //���̾�̽����� ����, ǰ������ �ƴ��� �޾ƿ´�.
                        if (FirebaseCtrl._Instance.productDict[productName].soldOut.ToString() == "1")
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
                    else
                    {
                        UICtrl._Instance.debugText.text += "\n���̾�̽� �ȿ� ����";
                    }
                    
                    foreach (string s in ChangeSpace._Instance.spaceDict.Keys)
                    {
                        string[] values;
                        ChangeSpace._Instance.spaceDict.TryGetValue(s, out values);

                        if (Array.Exists(values, x => x.Equals(hit.collider.gameObject.name)))
                        {
                            Debug.Log(s + "�������� �̵�");
                            //s �������� �̵�
                            switch (s)
                            {
                                case "��": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.houseSphere, Camera.main.transform.position, Quaternion.identity); break;
                                case "�ѷ���": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.rollerSphere, Camera.main.transform.position, Quaternion.identity); break;
                                case "����": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.roadSphere, Camera.main.transform.position, Quaternion.identity); break;
                                case "����": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.basketballSphere, Camera.main.transform.position, Quaternion.identity); break;
                                case "�ܿ�": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.winterSphere, Camera.main.transform.position, Quaternion.identity); break;
                            }
                            UICtrl._Instance.shoppingCenter.SetActive(false);
                            ViewCtrl._Instance._CloneProduct = Instantiate(hit.collider.gameObject, this.transform.position + new Vector3(0.0f, 0.0f, -1.0f), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
                            ViewCtrl._Instance._CloneProduct.GetComponent<Renderer>().materials = new Material[1] { ViewCtrl._Instance._CloneProduct.GetComponent<Renderer>().material };
                            ViewCtrl._Instance.isNewSpace = true;
                            //1,2,3�� �г��� ���ְ�
                            UICtrl._Instance.FloorPanel.SetActive(false);
                            PlayerCtrl._Instance.boyoung.SetActive(false);
                            //�ڷΰ��� ��ư ���̰�
                            UICtrl._Instance.BackToShoppingBtn.SetActive(true);
                            break;
                        }
                    }

                }
                //��ư �Ӽ��̶��
                if (hit.collider.gameObject.GetComponent<Button>() != null)
                {
                    Debug.Log("��ư Ŭ��");
                    Button btn = hit.collider.gameObject.GetComponent<Button>();
                    if (btn != null)
                    {
                        btn.Select();
                        btn.onClick.Invoke();
                    }
                }
            }
        }
    }
    public void SelectProduct_()
    {
        Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.blue, 10f); //������ ���̸� ���̰� �ϴ� �뵵(�̰� ������ �Ⱥ���)
        if (Physics.Raycast(transform.position, RayDir, out hit, MaxDistance)) //(Ray ����, Ray ����, �浹 ������ RaycastHit, Ray �Ÿ�(����))
        {
            Debug.Log("Ray�浹����" + hit.transform.name);
            if (hit.collider.tag == "product")
            {
                GameObject CurrentTouch = hit.transform.gameObject;
                Debug.Log("Ŭ���� ������Ʈ �̸� : " + hit.transform.gameObject.name);
                string productName = hit.transform.gameObject.name;
                productName = productName.Replace("(Clone)", "");
                //Ŭ���� ������Ʈ �̸����� ����, ����, (,) ����� 
                //�̸��� ���̾�̽� �ȿ� �ִ� name�� �̸��� ���ٸ�, 
                if (FirebaseCtrl._Instance.productDict.ContainsKey(productName))
                {
                    Debug.Log("ǰ���ΰ�? " + FirebaseCtrl._Instance.productDict[productName].soldOut.ToString());
                    name_text.text = productName;
                    price_text.text = FirebaseCtrl._Instance.productDict[productName].price.ToString();
                    UICtrl._Instance.OpenClose_Product_Info();
                    //���̾�̽����� ����, ǰ������ �ƴ��� �޾ƿ´�.
                    if (FirebaseCtrl._Instance.productDict[productName].soldOut.ToString() == "1")
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
                

                foreach (string s in ChangeSpace._Instance.spaceDict.Keys)
                {
                    string[] values;
                    ChangeSpace._Instance.spaceDict.TryGetValue(s, out values);

                    if (Array.Exists(values, x => x.Equals(hit.collider.gameObject.name)))
                    {
                        Debug.Log(s + "�������� �̵�");
                        //s �������� �̵�
                        switch (s)
                        {
                            case "��": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.houseSphere, Camera.main.transform.position, Quaternion.identity); break;
                            case "�ѷ���": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.rollerSphere, Camera.main.transform.position, Quaternion.identity); break;
                            case "����": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.roadSphere, Camera.main.transform.position, Quaternion.identity); break;
                            case "����": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.basketballSphere, Camera.main.transform.position, Quaternion.identity); break;
                            case "�ܿ�": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.winterSphere, Camera.main.transform.position, Quaternion.identity); break;
                        }
                        UICtrl._Instance.shoppingCenter.SetActive(false);
                        ViewCtrl._Instance._CloneProduct = Instantiate(hit.collider.gameObject, this.transform.position + new Vector3(0.0f, 1.5f, -1.0f), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
                        ViewCtrl._Instance._CloneProduct.GetComponent<Renderer>().materials = new Material[1] { ViewCtrl._Instance._CloneProduct.GetComponent<Renderer>().material };
                        ViewCtrl._Instance.isNewSpace = true;
                        //1,2,3�� �г��� ���ְ�
                        UICtrl._Instance.FloorPanel.SetActive(false);
                        //�ڷΰ��� ��ư ���̰�
                        UICtrl._Instance.BackToShoppingBtn.SetActive(true);
                        break;
                    }
                }

            }
            //��ư �Ӽ��̶��
            if (hit.collider.gameObject.GetComponent<Button>() != null)
            {
                Debug.Log("��ư Ŭ��");
                Button btn = hit.collider.gameObject.GetComponent<Button>();
                if (btn != null)
                {
                    btn.Select();
                    btn.onClick.Invoke();
                }
            }
        }
        
    }
    /*
        Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.blue, 10f); //������ ���̸� ���̰� �ϴ� �뵵(�̰� ������ �Ⱥ���)
        if (Physics.Raycast(transform.position, RayDir, out hit, MaxDistance)) //(Ray ����, Ray ����, �浹 ������ RaycastHit, Ray �Ÿ�(����))
        {
            Debug.Log("Ray�浹����" + hit.transform.name);
            if (hit.collider.tag == "product")
            {
                Info_Pannel.gameObject.SetActive(true);
                Debug.Log("��ǰ ����");
                Debug.Log("Ŭ���� ������Ʈ �̸� : " + hit.transform.gameObject.name);
                if (FirebaseCtrl._Instance.productDict.ContainsKey(hit.transform.name))
                {
                    Debug.Log("ǰ���ΰ�? " + FirebaseCtrl._Instance.productDict[hit.transform.gameObject.name].soldOut.ToString());
                    name_text.text = hit.transform.name;
                    price_text.text = FirebaseCtrl._Instance.productDict[hit.transform.name].price.ToString();
                    // amount_text.text = FirebaseCtrl.productDict[hit.transform.name].amount.ToString();

                    //���̾�̽����� ����, ǰ������ �ƴ��� �޾ƿ´�.
                    if (FirebaseCtrl._Instance.productDict[hit.transform.gameObject.name].soldOut.ToString() == "1")
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
            //��ư �Ӽ��̶��
            if (hit.collider.gameObject.GetComponent<Button>() != null)
            {
                Debug.Log("��ư Ŭ��");
                Button btn = hit.collider.gameObject.GetComponent<Button>();
                if (btn != null)
                {
                    btn.Select();
                    btn.onClick.Invoke();
                }
            }
        }

    }
    */
    //���̰� ��Ҵµ� �±װ� product�� ������
}