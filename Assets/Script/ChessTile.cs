using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChessTile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] public bool sea;

    public Vector3 thisObjectPosition;//当前棋盘区域坐标

    public GameManager gameManager;//引用游戏管理器脚本
    public Intelligence intelligence;//引用情报脚本
    public PlaneControl planeControl;//引用飞机控制脚本

    private string objectName; // 用于存储当前游戏对象的名称
    public GameObject MoverangeHighlight;//储存绿色高光
    public GameObject AttackrangeHighlight;//储存红色高光
    public GameObject ScoutrangHighlight;//储存黄色高光
    public GameObject TileText;//储存文字组件游戏对象
    public TextMesh tileTextMesh; // 储存文字组件


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        intelligence = GameObject.Find("IntellManager").GetComponent<Intelligence>();
        planeControl = GameObject.Find("PlaneController").GetComponent<PlaneControl>();

        Transform thisObjectTransform = transform;
        thisObjectPosition = thisObjectTransform.position; // 获取当前平台的当前位置（世界坐标）

        // 获取当前游戏对象的名称
        objectName = gameObject.name;
        if (tileTextMesh != null)
        {
            // 直接使用gameObject的name作为TextMesh的文本
            tileTextMesh.text = gameObject.name;
        }

        // 检查当前游戏对象的名称是否在 GameManager 的 landProperties 数组中，如果是，则将 sea 设置为 false
        foreach (string landProperty in gameManager.landProperties)
        {
            if (landProperty == objectName)
            {
                sea = false;
                break;
            }
        }

    }

    void Update()
    {
        showHighlight();//拖动时高光显示判断
        ChessbordAdsorption();//棋盘吸附判断
        AttackMode();//打开地图攻击模式判断
        ScoutMode();//打开地图侦察模式判断
        TextAdjust();//文字方向判断

    }
    public void Init(bool isOffset)
    {
        _renderer.material.color = isOffset ? _offsetColor : _baseColor;
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            _highlight.SetActive(true);
            TileText.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            _highlight.SetActive(false);
            TileText.SetActive(false);
        }
    }

    private void showHighlight()
    {
        if (sea)
        {
            if (gameManager.ShowRangHighlight)//显示移动距离高光
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
            Vector3 currentPosition = currentObjectTransform.position;// 获取当前舰船的当前位置（世界坐标）
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
                        gameManager.RoundMove = false;//每回合移动限制
                    }
                    else
                    {
                        MyTurnUnit turnUnit = gameManager.CurrentObject.GetComponent<MyTurnUnit>();
                        turnUnit.Turnback();
                        print("该区域不是海洋");
                    }
                }
                else
                {
                    MyTurnUnit turnUnit = gameManager.CurrentObject.GetComponent<MyTurnUnit>();
                    turnUnit.Turnback();
                    print("本回合已经移动过");
                }
            }
        }
    }
    public void AttackMode()
    {
        if (gameManager.attackmode)
        {
            Transform currentObjectTransform = gameManager.CurrentObject.transform;
            Vector3 currentPosition = currentObjectTransform.position;// 获取当前舰船的当前位置（世界坐标）
            float AttackDistanceX = currentPosition.x - thisObjectPosition.x;
            float AttackDistanceZ = currentPosition.z - thisObjectPosition.z;//获取距离

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
            Vector3 currentPosition = currentObjectTransform.position;// 获取当前舰船的当前位置（世界坐标）
            float AttackDistanceX = currentPosition.x - thisObjectPosition.x;
            float AttackDistanceZ = currentPosition.z - thisObjectPosition.z;//获取距离

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
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // 设置当前选中的ChessTile
            if (planeControl != null)
            {
                planeControl.currentTile = this;
                planeControl.PerformAction();
            }
        }
    }
    private void TextAdjust()
    {
        if (tileTextMesh != null)
        {
            tileTextMesh.transform.LookAt(Camera.main.transform.position);

            // 由于LookAt使得文本直接面向摄像机，可能导致文本倒置，所以下面的代码是调整文本使其正面朝向摄像机
            // 这通过沿y轴旋转180度来实现，因为LookAt会使文本的背面朝向摄像机
            tileTextMesh.transform.Rotate(0, 180, 0);
        }
    }
}
