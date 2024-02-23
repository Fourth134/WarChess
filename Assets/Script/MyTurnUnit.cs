using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyTurnUnit : MonoBehaviour
{
    public Color focusColor;//高光材质
    private Color defaultColor;//默认材质
    private MeshRenderer meshRenderer;//材质设置
    private bool isShowTip = false;//物品信息提示
    public string itemName = "Object Information"; // Replace with your object's information text
    private float initialYPos;//平台接触点
    private Plane dragPlane;//平台
    public Vector3 thisShipPosition;
    public GameManager gameManager;
    public GameObject ThisObj;
    public float moverange;//移动距离
    public float attackrange;//攻击距离


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        meshRenderer = GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material.color;
        initialYPos = transform.position.y;

        // 创建平面，使用物体所在平面的法线和平面上一点的位置
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
        // 限制鼠标在平面内移动
        transform.position = newPos;
    }

    public void OnMouseUp()
    {
        Boundary();
        gameManager.set = true;
        gameManager.ShowRangHighlight = false;      
        //gameManager.CurrentObject = null;
    }

    public void Turnback()//返回原位置方法
    {
        gameManager.CurrentObject.transform.position = new Vector3(gameManager.CurrentObjectX, 0.25f, gameManager.CurrentObjectZ);
        gameManager.set = false;
    }

    Vector3 GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;

        // 投射射线到平面上
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
        thisShipPosition = thisShipTransform.position;// 获取当前平台物体的当前位置（世界坐标）
        if (thisShipPosition.x > 42.5f || thisShipPosition.z > 42.5f || thisShipPosition.x < -2.5f || thisShipPosition.z < -2.5f)//判断边界
        {
            Turnback();
            print("该区域不在战区内");
        }
        else if (Mathf.Abs(thisShipPosition.x - gameManager.CurrentObjectX) > moverange || Mathf.Abs(thisShipPosition.z - gameManager.CurrentObjectZ) > moverange)//判断超出移动距离
        {
            Turnback();
            print("移动距离超出限制");
        }

    }
}
