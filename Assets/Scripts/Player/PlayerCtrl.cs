using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Management;

public class PlayerCtrl : MonoBehaviour
{
    public float MoveSpeed = 3.0f;
    private float v;
    float deltaS;

    private Vector2 lastPosInput;
    //private GameObject MainCamera_;
    private float gravity = 19.6f;

    // Start is called before the first frame update
    void Start()
    {
        deltaS = MoveSpeed * Time.deltaTime;
        lastPosInput = Vector2.zero;
        // MainCamera_ = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Move(); //�� �Լ� ������ ���� �ٲ�����~~ ���� �̰� �ʹ� �����Ƽ�,,

        var EulerY = Camera.main.transform.eulerAngles.y;
        //   Debug.Log(Camera.main.transform.eulerAngles.y);
        transform.position += Quaternion.Euler(0, EulerY, 0) * (new Vector3(lastPosInput.x, 0, lastPosInput.y) * deltaS);


    }

    //Move�Լ� ������ ���� �ٲ�����~~ ���� �̰� �ʹ� �����Ƽ�,,
    void Move()
    {


        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.Translate(Vector3.back * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (v < 0) //�ڷ� ���� ȸ���ݴ�
                this.transform.Rotate(0, 10 * MoveSpeed * Time.deltaTime, 0);
            else
                this.transform.Rotate(0, -10 * MoveSpeed * Time.deltaTime, 0);

        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (v < 0) //�ڷΰ��� ȸ���ݴ�
                this.transform.Rotate(0, -10 * MoveSpeed * Time.deltaTime, 0);
            else
                this.transform.Rotate(0, 10 * MoveSpeed * Time.deltaTime, 0);
        }
        
    }

   public void OnMove_(InputAction.CallbackContext context)
    {
        Debug.Log("��Ʈ�ѷ� �̿� ������");
        var v2 = context.ReadValue<Vector2>();
        if ((System.Math.Abs(v2.x) < 0.5) && (System.Math.Abs(v2.y) < 0.5))
        {
            lastPosInput.x = 0;
            lastPosInput.y = 0;
        }
        else
        {
            lastPosInput = context.ReadValue<Vector2>().normalized;
        }
   
    }

}
