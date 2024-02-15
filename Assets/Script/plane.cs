using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane : MonoBehaviour
{
    public float DistanceX;
    public float DistanceZ;

    public bool sea;

    public Vector3 thisObjectPosition;
    public GameManager gameManager;
    private string objectName; // 用于存储当前游戏对象的名称

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
            if (gameManager.set)
            {
                Transform currentObjectTransform = gameManager.CurrentObject.transform;
                Vector3 currentPosition = currentObjectTransform.position;// 获取当前单位物体的当前位置（世界坐标）
                DistanceX = currentPosition.x - thisObjectPosition.x;
                DistanceZ = currentPosition.z - thisObjectPosition.z;

                if (currentPosition.x > 42.5f || currentPosition.z > 42.5f || currentPosition.x < -2.5f || currentPosition.z < -2.5f)//超出地图边界
                {
                    Turnback();
                }
                else if (DistanceX < 2.5 && DistanceZ < 2.5 && DistanceX > -2.5 && DistanceZ > -2.5)
                {
                    gameManager.CurrentObject.transform.position = new Vector3(thisObjectPosition.x, gameManager.CurrentObject.transform.position.y + 0.025f, thisObjectPosition.z);
                    gameManager.set = false;
                    DistanceX = 0;
                    DistanceZ = 0;
                    gameManager.CurrentObjectX = thisObjectPosition.x;
                    gameManager.CurrentObjectZ = thisObjectPosition.z;
                }

            }
        }
        else
        {
            if (gameManager.set)
            {
                Turnback();
            }
        }
    }

    public void Turnback()
    {
        gameManager.CurrentObject.transform.position = new Vector3(gameManager.CurrentObjectX, 0.25f, gameManager.CurrentObjectX);
        gameManager.set = false;
    }
}