using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpace : MonoBehaviour
{
    public Dictionary<string, string[]> spaceDict = new Dictionary<string, string[]>();
    public static ChangeSpace _Instance;
    // Start is called before the first frame update
    void Awake()
    {
        _Instance = this;
      
        spaceDict.Add("��", new string[] { "����","å��","����" });
       // spaceDict.Add("�ѷ���", new string[] { "�ζ��ν�����Ʈ","�ѷ�������Ʈ" });
        spaceDict.Add("����", new string[] { "ű����","������" });
        spaceDict.Add("����", new string[] { "�󱸰�","�౸��","�󱸰��" });
        spaceDict.Add("�ܿ�", new string[] { "Ƽ����" });

    }

}
