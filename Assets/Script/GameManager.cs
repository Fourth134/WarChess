using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int EnemymoveDistance;//敌人移动距离
    public string targetTile;//敌人目标点位置
    public int tendency;//敌人向目标点移动倾向
    public GridManager gridManager;//读取GridManager脚本

    public float CurrentObjectX;
    public float CurrentObjectZ; //当前控制物体记录的上一次正确坐标

    
    public string[] landProperties;// String数组，用于存储地块是否为海洋
    public string enemyPosition; // 替换原来的 landEnemyExist 列表，现在只追踪一个敌人的位置
    public string nextEnemyPosition; // 预计下一回合敌人的位置


    // Start is called before the first frame update

    void Start()
    {
        CalculateNextEnemyMove(); // 游戏开始时计算敌人的第一步
        EnemyInfo();
    }

    public void NextTurn()
    {
        TurnNum++;
        RoundMove = true;
        print("现在是" + TurnNum + "回合");

        // 根据预计的移动信息实际移动敌人
        ApplyEnemyMoves();
        // 为下一回合计算新的移动
        CalculateNextEnemyMove();
        // 在下回合开始前打印敌人信息
        EnemyInfo();

        // 更新其他游戏状态
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

    private void CalculateNextEnemyMove()
    {
        // 清除上一回合的移动信息
        bool isValidMove = false;
        int attemptCount = 0; // 避免无限循环
        int x, y, newX = 0, newY = 0;

        string[] parts = enemyPosition.Split(' ');
        x = int.Parse(parts[1]);
        y = int.Parse(parts[2]);

        while (!isValidMove && attemptCount < 10)
        {
            attemptCount++;
            int directionX = Random.Range(-EnemymoveDistance, EnemymoveDistance + 1);
            int directionY = Random.Range(-EnemymoveDistance, EnemymoveDistance + 1);

            newX = Mathf.Clamp(x + directionX, 0, gridManager._width - 1);
            newY = Mathf.Clamp(y + directionY, 0, gridManager._height - 1);

            string potentialNewLocation = $"Tile {newX} {newY}";

            // 检查新位置是否符合移动条件（例如是否为海洋）
            if (!landProperties.Contains(potentialNewLocation))
            {
                isValidMove = true; // 找到一个有效的移动位置
            }
        }

        if (isValidMove)
        {
            // 存储敌人预计的移动，而不是立即执行
            nextEnemyPosition = $"Tile {newX} {newY}";
        }
    }

    private void ApplyEnemyMoves() // 实际移动敌人的方法
    {
        enemyPosition = nextEnemyPosition;
    }

    public void EnemyInfo()
    {
        print($"敌人从 {enemyPosition} 预计移动到 {nextEnemyPosition}");
    }
}




