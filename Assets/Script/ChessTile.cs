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
    private float moveRange;//���潢�����ƶ�����
    public GameObject MoverangeHighlight;

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
    }

    // Update is called once per frame
    void Update()
    {
        showHighlight();//�϶�ʱ�߹���ʾ�ж�
        ChessbordAdsorption();//���������ж�
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
                moveRange = turnUnit.moverange;
                if (thisObjectPosition.x - gameManager.CurrentObjectX < moveRange && thisObjectPosition.z - gameManager.CurrentObjectZ < moveRange && thisObjectPosition.x - gameManager.CurrentObjectX > -moveRange && thisObjectPosition.z - gameManager.CurrentObjectZ > -moveRange)
                {

                    MoverangeHighlight.SetActive(true);
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
                if (sea)
                {
                    gameManager.CurrentObject.transform.position = new Vector3(thisObjectPosition.x, gameManager.CurrentObject.transform.position.y + 0.025f, thisObjectPosition.z);
                    gameManager.set = false;
                    gameManager.CurrentSealand = gameObject;
                    gameManager.CurrentObjectX = thisObjectPosition.x;
                    gameManager.CurrentObjectZ = thisObjectPosition.z;
                    gameManager.CurrentObject = null;
                }
                else
                {
                    MyTurnUnit turnUnit = gameManager.CurrentObject.GetComponent<MyTurnUnit>();
                    turnUnit.Turnback();
                    print("�������Ǻ���");
                }
            }
        }
    }
}
