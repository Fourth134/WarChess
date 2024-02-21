using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class plane : MonoBehaviour
{
    public float DistanceX;
    public float DistanceZ;

    public bool sea;

    public Vector3 thisObjectPosition;
    public GameManager gameManager;
    private string objectName; // ���ڴ洢��ǰ��Ϸ���������
    private float moveRange;//���潢�����ƶ�����
    public GameObject MoverangeHighlight;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Transform thisObjectTransform = transform;
        thisObjectPosition = thisObjectTransform.position;// ��ȡ��ǰƽ̨����ĵ�ǰλ�ã��������꣩

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
    }

    // Update is called once per frame
    void Update()
    {
        if (sea)
        {
            if (gameManager.ShowRangHighlight)//��ʾ�ƶ�����߹�
            {
                ReadMoveRangeFromCurrentObject();//��ȡ�ƶ�����
 
                if (thisObjectPosition.x-gameManager.CurrentObjectX < moveRange && thisObjectPosition.z-gameManager.CurrentObjectZ < moveRange && thisObjectPosition.x-gameManager.CurrentObjectX > -moveRange && thisObjectPosition.z-gameManager.CurrentObjectZ > -moveRange)
                {

                    MoverangeHighlight.SetActive(true);
                }
            }
            else
            {
                MoverangeHighlight.SetActive(false);
            }
            if (gameManager.set)
            {
                Transform currentObjectTransform = gameManager.CurrentObject.transform;
                Vector3 currentPosition = currentObjectTransform.position;// ��ȡ��ǰ�����ĵ�ǰλ�ã��������꣩
                DistanceX = currentPosition.x - thisObjectPosition.x;
                DistanceZ = currentPosition.z - thisObjectPosition.z;

                if (currentPosition.x > 42.5f || currentPosition.z > 42.5f || currentPosition.x < -2.5f || currentPosition.z < -2.5f)//������ͼ�߽�
                {
                    Turnback();
                    print("������ͼ�߽�");
                }
                else if (DistanceX < 2.5 && DistanceZ < 2.5 && DistanceX > -2.5 && DistanceZ > -2.5)
                {
                    ReadMoveRangeFromCurrentObject();
                    if (Mathf.Abs(thisObjectPosition.x - gameManager.CurrentObjectX) > moveRange || Mathf.Abs(thisObjectPosition.z - gameManager.CurrentObjectZ) > moveRange)
                    {
                        Turnback();
                        print("�����ƶ���������");
                    }
                    else
                    {
                        gameManager.CurrentObject.transform.position = new Vector3(thisObjectPosition.x, gameManager.CurrentObject.transform.position.y + 0.025f, thisObjectPosition.z);
                    gameManager.set = false;
                    gameManager.CurrentSealand = gameObject;
                    gameManager.CurrentObjectX = thisObjectPosition.x;
                    gameManager.CurrentObjectZ = thisObjectPosition.z;
                    }
                }

            }
        }
        else
        {
            if (gameManager.set)
            {
                Turnback();
                print("��λ�ò��Ǻ��棬�����ƶ�����λ��");
            }
        }
    }

    public void Turnback()//����ԭλ�÷���
    {
        gameManager.CurrentObject.transform.position = new Vector3(gameManager.CurrentObjectX, 0.25f, gameManager.CurrentObjectX);
        gameManager.set = false;
    }
    public void ReadMoveRangeFromCurrentObject()//��ȡ��ǰ
    {
        // ����Ƿ��е�ǰѡ�е���Ϸ����
        if (gameManager.CurrentObject != null)
        {
            // ���Ի�ȡ MyTurnUnit �ű�
            MyTurnUnit turnUnit = gameManager.CurrentObject.GetComponent<MyTurnUnit>();

            // ��� MyTurnUnit �ű��Ƿ����
            if (turnUnit != null)
            {
                // ��ȡ moverange ֵ
                moveRange = turnUnit.moverange;

            }
            else
            {
                // ��� MyTurnUnit �ű������ڣ��������ﴦ�������쳣���
                Debug.LogError("MyTurnUnit script not found on the current object.");
            }
        }
        else
        {
            // ���û�е�ǰѡ�е���Ϸ���󣬿������ﴦ�������쳣���
            Debug.LogError("No current object selected in GameManager.");
        }
    }
}