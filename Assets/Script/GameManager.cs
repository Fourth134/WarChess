using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GridManager gridManager;//读取GridManager脚本
    public PlaneControl planeControl;//读取PlaneControl脚本
    public EnemyState enemyState;//读取EnemyState脚本
    public Intelligence intelligence;//读取Intelligence脚本

    public float CurrentObjectX;
    public float CurrentObjectZ; //当前控制物体记录的上一次正确坐标

    public Text turnCounterText; // UI Text元素，用于显示当前回合数
    public string[] landProperties;// String数组，用于存储地块是否为海洋

    // Start is called before the first frame update

    void Start()
    {
        // 进行敌人初始计算，这部分可能由EnemyState脚本自己在其Start方法中处理
        enemyState.StartEnemyMove();
    }

    public void NextTurn()
    {
        TurnNum++;
        RoundMove = true;
        print("现在是" + TurnNum + "回合");
        if (turnCounterText != null)
        {
            turnCounterText.text = "当前回合: " + TurnNum; // 更新UI Text显示当前回合数
        }
        // 应用敌人的移动
        enemyState.ApplyEnemyMoves();
        // 计算敌人下一回合的移动
        enemyState.CalculateNextEnemyMove();
        // 在控制台输出敌人信息
        enemyState.EnemyInfo();

        // 更新其他游戏状态
        attackmode = false;
        scoutmode = false;
        planeControl.UpdatePlaneActions();//进行有关飞机的回合判断
        DisplayPreviousRoundInfo();// 显示上一回合的攻击和侦察信息
    }

    public void DisplayPreviousRoundInfo()// 显示上一回合的攻击和侦察信息
    {
        
        foreach (string info in planeControl.attackInfos)
        {
            intelligence.AddAttackInfo(info);
        }
        planeControl.attackInfos.Clear();

        foreach (string info in planeControl.scoutInfos)
        {
            intelligence.AddScoutInfo(info);
        }
        planeControl.scoutInfos.Clear();
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

}




