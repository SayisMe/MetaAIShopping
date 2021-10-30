using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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

        if (Physics.Raycast(transform.position, RayDir, out hit, MaxDistance)) //(Ray ����, Ray ����, �浹 ������ RaycastHit, Ray �Ÿ�(����))
        {
            if (hit.collider.gameObject.GetComponent<Button>() != null)
            {
                Debug.Log("��ư ����ħ");
                Button btn = hit.collider.gameObject.GetComponent<Button>();
                if (btn != null)
                {
                    btn.Select();
                }
            }
        }
        //test��
        if (Input.GetKeyDown(KeyCode.Space))
           SelectProduct_();
    }
    
    public void SelectProduct(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.blue, 10f); //������ ���̸� ���̰� �ϴ� �뵵(�̰� ������ �Ⱥ���)
            if (Physics.Raycast(transform.position, RayDir, out hit, MaxDistance)) //(Ray ����, Ray ����, �浹 ������ RaycastHit, Ray �Ÿ�(����))
            {
                Debug.Log("Ray�浹����" + hit.transform.name);
                if (hit.collider.tag == "product")
                {
                    /*if (hit.collider.name == "tshirt")
                    {
                        SceneManager.LoadScene("retro", LoadSceneMode.Additive);
                    }
                    */
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
    }
    public void SelectProduct_()
    {

        Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.blue, 10f); //������ ���̸� ���̰� �ϴ� �뵵(�̰� ������ �Ⱥ���)
        if (Physics.Raycast(transform.position, RayDir, out hit, MaxDistance)) //(Ray ����, Ray ����, �浹 ������ RaycastHit, Ray �Ÿ�(����))
        {
            Debug.Log("Ray�浹����" + hit.transform.name);
            if (hit.collider.tag == "product")
            {
                if (hit.collider.name == "tshirt")
                {
                    SceneManager.LoadScene("retro", LoadSceneMode.Additive);
                }
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
    //���̰� ��Ҵµ� �±װ� product�� ������
}