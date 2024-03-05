using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChessTile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] public bool sea;

    public Vector3 thisObjectPosition;//��ǰ������������
    public GameManager gameManager;
    private string objectName; // ���ڴ洢��ǰ��Ϸ���������
    public GameObject MoverangeHighlight;//������ɫ�߹�
    public GameObject AttackrangeHighlight;//�����ɫ�߹�
    public GameObject ScoutrangHighlight;//�����ɫ�߹�
    public bool EnemyExist;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Transform thisObjectTransform = transform;
        thisObjectPosition = thisObjectTransform.position;// ��ȡ��ǰƽ̨�ĵ�ǰλ�ã��������꣩

        // ��ȡ��ǰ��Ϸ���������
        objectName = gameObject.name;

        // ��鵱ǰ��Ϸ����������Ƿ��� GameManager �� landProperties �����У�����ǣ��� sea ����Ϊ false
        foreach (string landProperty in gameManager.landProperties)
        {
            if (landProperty == objectName)
            {
                sea = false;
                break;
            }
        }
        //��鵱ǰ��Ϸ�����Ƿ��е���
        foreach (string enemyProperty in gameManager.landEnemyExist)
        {
            if (enemyProperty == objectName)
            {
                EnemyExist = true;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        showHighlight();//�϶�ʱ�߹���ʾ�ж�
        ChessbordAdsorption();//���������ж�
        AttackMode();//�򿪵�ͼ����ģʽ�ж�
        ScoutMode();//�򿪵�ͼ���ģʽ�ж�
    }
    public void Init(bool isOffset)
    {
        _renderer.material.color = isOffset ? _offsetColor : _baseColor;
    }

    private void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    private void showHighlight()
    {
        if (sea)
        {
            if (gameManager.ShowRangHighlight)//��ʾ�ƶ�����߹�
            {
                MyTurnUnit turnUnit = gameManager.CurrentObject.GetComponent<MyTurnUnit>();
                float moveRange = turnUnit.moverange;
                if (thisObjectPosition.x - gameManager.CurrentObjectX < moveRange && thisObjectPosition.z - gameManager.CurrentObjectZ < moveRange && thisObjectPosition.x - gameManager.CurrentObjectX > -moveRange && thisObjectPosition.z - gameManager.CurrentObjectZ > -moveRange)
                {
                    if (gameManager.RoundMove)
                    {
                        MoverangeHighlight.SetActive(true);
                    }
                    else
                    {
                        MoverangeHighlight.SetActive(false);
                    }
                }
            }
            else
            {
                MoverangeHighlight.SetActive(false);
            }
        }
    }
    public void ChessbordAdsorption()
    {
        if (gameManager.set)
        {
            Transform currentObjectTransform = gameManager.CurrentObject.transform;
            Vector3 currentPosition = currentObjectTransform.position;// ��ȡ��ǰ�����ĵ�ǰλ�ã��������꣩
            float DistanceX = currentPosition.x - thisObjectPosition.x;
            float DistanceZ = currentPosition.z - thisObjectPosition.z;
            if (Mathf.Abs(DistanceX) < 2.5 && Mathf.Abs(DistanceZ) < 2.5)
            {
                if (gameManager.RoundMove)
                {
                    if (sea)
                    {
                        gameManager.CurrentObject.transform.position = new Vector3(thisObjectPosition.x, gameManager.CurrentObject.transform.position.y + 0.025f, thisObjectPosition.z);
                        gameManager.set = false;
                        gameManager.CurrentSealand = gameObject;
                        gameManager.CurrentObjectX = thisObjectPosition.x;
                        gameManager.CurrentObjectZ = thisObjectPosition.z;
                        gameManager.RoundMove = false;//ÿ�غ��ƶ�����
                    }
                    else
                    {
                        MyTurnUnit turnUnit = gameManager.CurrentObject.GetComponent<MyTurnUnit>();
                        turnUnit.Turnback();
                        print("�������Ǻ���");
                    }
                }
                else
                {
                    MyTurnUnit turnUnit = gameManager.CurrentObject.GetComponent<MyTurnUnit>();
                    turnUnit.Turnback();
                    print("���غ��Ѿ��ƶ���");
                }
            }
        }
    }
    public void AttackMode()
    {
        if (gameManager.attackmode)
        {
            Transform currentObjectTransform = gameManager.CurrentObject.transform;
            Vector3 currentPosition = currentObjectTransform.position;// ��ȡ��ǰ�����ĵ�ǰλ�ã��������꣩
            float AttackDistanceX = currentPosition.x - thisObjectPosition.x;
            float AttackDistanceZ = currentPosition.z - thisObjectPosition.z;//��ȡ����

            MyTurnUnit turnUnit = gameManager.CurrentObject.GetComponent<MyTurnUnit>();
            float attackRange = turnUnit.attackrange;
            if (Mathf.Abs(AttackDistanceX) < attackRange && Mathf.Abs(AttackDistanceZ) < attackRange)
            {
                AttackrangeHighlight.SetActive(true);
            }
            else
            {
                AttackrangeHighlight.SetActive(false);
            }
        }
        else
        {
            AttackrangeHighlight.SetActive(false);
        }
    }
    public void ScoutMode()
    {
        if (gameManager.scoutmode)
        {
            Transform currentObjectTransform = gameManager.CurrentObject.transform;
            Vector3 currentPosition = currentObjectTransform.position;// ��ȡ��ǰ�����ĵ�ǰλ�ã��������꣩
            float AttackDistanceX = currentPosition.x - thisObjectPosition.x;
            float AttackDistanceZ = currentPosition.z - thisObjectPosition.z;//��ȡ����

            MyTurnUnit turnUnit = gameManager.CurrentObject.GetComponent<MyTurnUnit>();
            float ScoutRange = turnUnit.scoutrange;
            if (Mathf.Abs(AttackDistanceX) < ScoutRange && Mathf.Abs(AttackDistanceZ) < ScoutRange)
            {
                ScoutrangHighlight.SetActive(true);
            }
            else
            {
                ScoutrangHighlight.SetActive(false);
            }
        }
        else
        {
            ScoutrangHighlight.SetActive(false);
        }
    }
    public void OnMouseDown()
    {
        if (gameManager.attackmode)
        {
            if (gameManager.Roundaction)
            {
                print("�ɻ����ǰ��"+ objectName+"���й���");
                if (EnemyExist)
                {
                    print("��������");
                }
                else
                {
                    print("������δ���ֵ���");
                }
                gameManager.Roundaction = false;
                gameManager.attackmode = false;//�رչ���ģʽ�߹���ʾ
            }
            else
            {
                print("���غ��Ѿ����ж���");
            }
        }
    }
}
