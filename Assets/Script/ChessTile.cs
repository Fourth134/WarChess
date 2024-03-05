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

    public Vector3 thisObjectPosition;//当前棋盘区域坐标
    public GameManager gameManager;
    private string objectName; // 用于存储当前游戏对象的名称
    public GameObject MoverangeHighlight;//储存绿色高光
    public GameObject AttackrangeHighlight;//储存红色高光
    public GameObject ScoutrangHighlight;//储存黄色高光
    public bool EnemyExist;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Transform thisObjectTransform = transform;
        thisObjectPosition = thisObjectTransform.position;// 获取当前平台的当前位置（世界坐标）

        // 获取当前游戏对象的名称
        objectName = gameObject.name;

        // 检查当前游戏对象的名称是否在 GameManager 的 landProperties 数组中，如果是，则将 sea 设置为 false
        foreach (string landProperty in gameManager.landProperties)
        {
            if (landProperty == objectName)
            {
                sea = false;
                break;
            }
        }
        //检查当前游戏对象是否有敌人
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
        showHighlight();//拖动时高光显示判断
        ChessbordAdsorption();//棋盘吸附判断
        AttackMode();//打开地图攻击模式判断
        ScoutMode();//打开地图侦察模式判断
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
        if (gameManager.attackmode)
        {
            if (gameManager.Roundaction)
            {
                print("飞机起飞前往"+ objectName+"进行攻击");
                if (EnemyExist)
                {
                    print("攻击命中");
                }
                else
                {
                    print("该区域未发现敌人");
                }
                gameManager.Roundaction = false;
                gameManager.attackmode = false;//关闭攻击模式高光显示
            }
            else
            {
                print("本回合已经进行动作");
            }
        }
    }
}
