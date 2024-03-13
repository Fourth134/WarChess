using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject CurrentObject;
    public GameObject CurrentSealand;
    public bool set = false;//判断是否触发棋盘中心吸附
    public bool ShowRangHighlight = false;//判断距离高光显示

    public bool RoundMove = true;//储存本回合是否完成移动
    public int TurnNum=1;//储存当前回合数
    public bool attackmode = false;//储存当前是否是攻击模式
    public bool scoutmode = false;//储存当前是否是侦察模式
    public bool Roundaction = true;//储存本回合是否完成一次动作
    public int Scoutaction = 0;//储存是否可以进行侦察
    public int ScoutTime;//储存侦察机进行准备的回合数

    public float CurrentObjectX;
    public float CurrentObjectZ; //当前控制物体记录的上一次正确坐标

    
    public string[] landProperties;// String数组，用于存储地块是否为海洋
    public List<string> landEnemyExist = new List<string>();// List，用于存储地块是否存在敌人 

    // Start is called before the first frame update

    public void NextTurn()
    {
        TurnNum++;
        RoundMove = true;
        print("现在是"+ TurnNum+"回合");
        attackmode = false;
        scoutmode = false;
        Roundaction = true;
        if (Scoutaction != 0)
        {
            Scoutaction++;
        }
        if (Scoutaction > ScoutTime)
        {
            Scoutaction = 0;
            print("侦察机已经就绪");
        }
        MoveEnemy();
    }

    public void AttackSwitch()
    {
        if (attackmode)
        {
            attackmode = false;
        }
        else
        {
            if (scoutmode)
            {
                ScoutSwitch();
            }
            attackmode = true;
        }
    }

    public void ScoutSwitch()
    {
        if (scoutmode)
        {
            scoutmode = false;
        }
        else
        {
            if(attackmode)
            {
                AttackSwitch();
            }
            scoutmode = true;
        }
    }

    public void EnemyInfo()
    {

    }

    public void MoveEnemy()
    {
        if (landEnemyExist.Count == 0)
        {
            print("地图上没有敌人");
            return;
        }


        // 随机选择一个敌人的当前位置
        int index = Random.Range(0, landEnemyExist.Count);
        string currentLocation = landEnemyExist[index];

        // 解析当前位置的x和y坐标
        string[] parts = currentLocation.Split(' ');
        int x = int.Parse(parts[1]);
        int y = int.Parse(parts[2]);

        // 生成随机移动方向
        int moveX = Random.Range(-1, 2); // 随机生成-1，0，1
        int moveY = Random.Range(-1, 2); // 随机生成-1，0，1

        // 计算新位置，确保不越界
        int newX = Mathf.Clamp(x + moveX, 0, 8);
        int newY = Mathf.Clamp(y + moveY, 0, 8);

        // 构建新位置的字符串表示
        string newLocation = $"Tile {newX} {newY}";

        // 更新敌人位置
        landEnemyExist[index] = newLocation;
    }
}

