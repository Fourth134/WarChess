using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject CurrentObject;
    public GameObject CurrentSealand;
    public bool set = false;//�ж��Ƿ񴥷�������������
    public bool ShowRangHighlight = false;//�жϾ���߹���ʾ

    public bool RoundMove;

    public float CurrentObjectX;
    public float CurrentObjectZ; //��ǰ���������¼����һ����ȷ����

    
    public string[] landProperties;// String���飬���ڴ洢�ؿ������

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
