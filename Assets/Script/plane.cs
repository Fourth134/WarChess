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
    private string objectName; // 用于存储当前游戏对象的名称
    private float moveRange;//储存舰船的移动距离
    public GameObject MoverangeHighlight;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Transform thisObjectTransform = transform;
        thisObjectPosition = thisObjectTransform.position;// 获取当前平台物体的当前位置（世界坐标）

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
        if (sea)
        {
            if (gameManager.ShowRangHighlight)//显示移动距离高光
            {
                ReadMoveRangeFromCurrentObject();//获取移动距离
 
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
                Vector3 currentPosition = currentObjectTransform.position;// 获取当前舰船的当前位置（世界坐标）
                DistanceX = currentPosition.x - thisObjectPosition.x;
                DistanceZ = currentPosition.z - thisObjectPosition.z;

                if (currentPosition.x > 42.5f || currentPosition.z > 42.5f || currentPosition.x < -2.5f || currentPosition.z < -2.5f)//超出地图边界
                {
                    Turnback();
                    print("超出地图边界");
                }
                else if (DistanceX < 2.5 && DistanceZ < 2.5 && DistanceX > -2.5 && DistanceZ > -2.5)
                {
                    ReadMoveRangeFromCurrentObject();
                    if (Mathf.Abs(thisObjectPosition.x - gameManager.CurrentObjectX) > moveRange || Mathf.Abs(thisObjectPosition.z - gameManager.CurrentObjectZ) > moveRange)
                    {
                        Turnback();
                        print("超出移动距离限制");
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
                print("该位置不是海面，不可移动到此位置");
            }
        }
    }

    public void Turnback()//返回原位置方法
    {
        gameManager.CurrentObject.transform.position = new Vector3(gameManager.CurrentObjectX, 0.25f, gameManager.CurrentObjectX);
        gameManager.set = false;
    }
    public void ReadMoveRangeFromCurrentObject()//获取当前
    {
        // 检查是否有当前选中的游戏对象
        if (gameManager.CurrentObject != null)
        {
            // 尝试获取 MyTurnUnit 脚本
            MyTurnUnit turnUnit = gameManager.CurrentObject.GetComponent<MyTurnUnit>();

            // 检查 MyTurnUnit 脚本是否存在
            if (turnUnit != null)
            {
                // 获取 moverange 值
                moveRange = turnUnit.moverange;

            }
            else
            {
                // 如果 MyTurnUnit 脚本不存在，可在这里处理错误或异常情况
                Debug.LogError("MyTurnUnit script not found on the current object.");
            }
        }
        else
        {
            // 如果没有当前选中的游戏对象，可在这里处理错误或异常情况
            Debug.LogError("No current object selected in GameManager.");
        }
    }
}