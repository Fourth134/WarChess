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


    public float CurrentObjectX;
    public float CurrentObjectZ; //当前控制物体记录的上一次正确坐标

    
    public string[] landProperties;// String数组，用于存储地块是否为海洋
    public string[] landEnemyExist;// String数组，用于存储地块是否存在敌人 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void NextTurn()
    {
        TurnNum++;
        RoundMove = true;
        print("现在是"+ TurnNum+"回合");
        attackmode = false;
        scoutmode = false;
        Roundaction = true;
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
