using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTurnUnit : MonoBehaviour
{
    public Color focusColor;
    private Color defaultColor;
    private MeshRenderer meshRenderer;//材质
    private bool isShowTip = false;
    private string itemName = "Object Information"; // Replace with your object's information text
    private float initialYPos;//平台接触点
    private Plane dragPlane;//平台

    public GameManager gameManager;
    public GameObject ThisObj;

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
        gameManager.CurrentObject = null;
    }

    public void OnMouseDrag()
    {
        gameManager.CurrentObject = ThisObj;
        gameManager.set = false;
        Vector3 newPos = GetMousePos();
        // 限制鼠标在平面内移动
        transform.position = newPos;
    }

    public void OnMouseUp()
    {
        gameManager.set = true;
        //gameManager.CurrentObject = null;
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
            style.fontSize = 20;
            style.normal.textColor = Color.blue;
            GUI.Label(new Rect(Input.mousePosition.x - 100, Screen.height - Input.mousePosition.y + 30, 100, 100), itemName, style);
        }
    }
}
