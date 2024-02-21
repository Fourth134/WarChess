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
    private float moveRange;//储存舰船的移动距离
    public GameObject MoverangeHighlight;

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
    }

    // Update is called once per frame
    void Update()
    {
        showHighlight();//拖动时高光显示判断
        ChessbordAdsorption();//棋盘吸附判断
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
            Vector3 currentPosition = currentObjectTransform.position;// 获取当前舰船的当前位置（世界坐标）
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
                    print("该区域不是海洋");
                }
            }
        }
    }
}
