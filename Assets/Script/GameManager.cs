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
    public List<string> landEnemyExist = new List<string>();// List，用于存储地块是否存在敌人 
    private Dictionary<string, string> nextEnemyPositions = new Dictionary<string, string>(); // 存储敌人下一步预计的位置


    // Start is called before the first frame update

    void Start()
    {
        CalculateNextEnemyMove(); // 游戏开始时计算敌人的第一步
    }

    public void NextTurn()
    {
        TurnNum++;
        RoundMove = true;
        print("现在是" + TurnNum + "回合");

        // 在回合开始前打印敌人信息
        EnemyInfo();

        // 根据预计的移动信息实际移动敌人
        ApplyEnemyMoves();

        // 为下一回合计算新的移动
        CalculateNextEnemyMove();

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
        nextEnemyPositions.Clear(); // 清除上一回合的移动信息

        foreach (var currentLocation in landEnemyExist)
        {
            string[] parts = currentLocation.Split(' ');
            int x = int.Parse(parts[1]);
            int y = int.Parse(parts[2]);

            bool isValidMove = false;
            int newX = x, newY = y;
            int attemptCount = 0; // 避免无限循环

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
                string newLocation = $"Tile {newX} {newY}";
                nextEnemyPositions[currentLocation] = newLocation;
            }
        }
    }

    private void ApplyEnemyMoves()
    {
        foreach (var enemyMove in nextEnemyPositions)
        {
            if (landEnemyExist.Contains(enemyMove.Key))
            {
                landEnemyExist.Remove(enemyMove.Key);
                landEnemyExist.Add(enemyMove.Value);
            }
        }
    }

    public void EnemyInfo()
    {
        foreach (var enemyMove in nextEnemyPositions)
        {
            print($"敌人从 {enemyMove.Key} 预计移动到 {enemyMove.Value}");
        }
    }
}



