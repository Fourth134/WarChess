using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyTurnUnit : MonoBehaviour
{
    public Color focusColor;//�߹����
    private Color defaultColor;//Ĭ�ϲ���
    private MeshRenderer meshRenderer;//��������
    private bool isShowTip = false;//��Ʒ��Ϣ��ʾ
    public string itemName = "Object Information"; // Replace with your object's information text
    private float initialYPos;//ƽ̨�Ӵ���
    private Plane dragPlane;//ƽ̨
    public Vector3 thisShipPosition;
    public GameManager gameManager;
    public GameObject ThisObj;
    public float moverange;//�ƶ�����
    public float attackrange;//��������


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        meshRenderer = GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material.color;
        initialYPos = transform.position.y;

        // ����ƽ�棬ʹ����������ƽ��ķ��ߺ�ƽ����һ���λ��
        dragPlane = new Plane(Vector3.up, transform.position);
    }

    public void OnMouseEnter()
    {
        isShowTip = true;
        meshRenderer.material.color = focusColor;
    }

    public void OnMouseExit()
    {
        isShowTip = false;
        meshRenderer.material.color = defaultColor;
        gameManager.ShowRangHighlight = false;
    }

    public void OnMouseDrag()
    {
        gameManager.ShowRangHighlight = true;
        gameManager.CurrentObject = ThisObj;
        Vector3 newPos = GetMousePos();
        // ���������ƽ�����ƶ�
        transform.position = newPos;
    }

    public void OnMouseUp()
    {
        Boundary();
        gameManager.set = true;
        gameManager.ShowRangHighlight = false;      
        //gameManager.CurrentObject = null;
    }

    public void Turnback()//����ԭλ�÷���
    {
        gameManager.CurrentObject.transform.position = new Vector3(gameManager.CurrentObjectX, 0.25f, gameManager.CurrentObjectZ);
        gameManager.set = false;
    }

    Vector3 GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;

        // Ͷ�����ߵ�ƽ����
        if (dragPlane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            return new Vector3(hitPoint.x, initialYPos, hitPoint.z);
        }

        return transform.position;
    }

    public void OnGUI()
    {
        if (isShowTip)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 60;
            style.normal.textColor = Color.blue;
            GUI.Label(new Rect(Input.mousePosition.x - 100, Screen.height - Input.mousePosition.y + 30, 100, 100), itemName, style);
        }
    }

    public void Boundary()
    {
        Transform thisShipTransform = transform;
        thisShipPosition = thisShipTransform.position;// ��ȡ��ǰƽ̨����ĵ�ǰλ�ã��������꣩
        if (thisShipPosition.x > 42.5f || thisShipPosition.z > 42.5f || thisShipPosition.x < -2.5f || thisShipPosition.z < -2.5f)//�жϱ߽�
        {
            Turnback();
            print("��������ս����");
        }
        else if (Mathf.Abs(thisShipPosition.x - gameManager.CurrentObjectX) > moverange || Mathf.Abs(thisShipPosition.z - gameManager.CurrentObjectZ) > moverange)//�жϳ����ƶ�����
        {
            Turnback();
            print("�ƶ����볬������");
        }

    }
}
