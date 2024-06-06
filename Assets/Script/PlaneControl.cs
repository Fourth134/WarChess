using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlaneControl : MonoBehaviour
{
    public GameManager gameManager;
    public Intelligence intelligence;
    public EnemyState enemyState;//读取EnemyState脚本

    public GameObject PlayerNotification;//引用飘字物体组件

    public int Attackaction = 0; // 储存是否可以进行侦察（0代表可以）
    public int AttackTime; // 储存侦察机进行准备的回合数
    public int Scoutaction = 0; // 储存是否可以进行侦察（0代表可以）
    public int ScoutTime; // 储存侦察机进行准备的回合数

    public Text PlayerNotificationText;//引用飘字
    public Text PatrolAircraftStateText; // 引用侦察机状态UI文本
    public Text PatrolAircraftInfoText; // 引用侦察机信息UI文本
    public Text AttackAircraftStateText; // 引用侦察机状态UI文本
    public Text AttackAircraftInfoText; // 引用侦察机信息UI文本
    public List<string> attackInfos = new List<string>();//缓存本回合攻击信息
    public List<string> scoutInfos = new List<string>();//缓存本回合侦察信息

    // 可以通过某种方式设置当前选中的ChessTile，例如通过鼠标点击
    public ChessTile currentTile;

    void Start()
    {
        PatrolAircraftInfoText.text = " ";
        PatrolAircraftStateText.text = "On Board";
        AttackAircraftInfoText.text = " ";
        AttackAircraftStateText.text = "On Board";
    }
    public void PerformAction()//攻击和侦察触发方法
    {
        if (gameManager == null || intelligence == null || currentTile == null)
        {
            Debug.Log("需保证PlaneControl对象gameManager, intelligence, 或 currentTile 未初始化");
            return;
        }

        if (gameManager.attackmode && Attackaction == 0)
        {
            PerformAttack(currentTile.gameObject.name);
        }
        else if (gameManager.scoutmode && Scoutaction == 0)
        {
            PerformScout(currentTile.gameObject.name);
        }
        else if (Attackaction > 0)
        {
            OpenPlayerNotification();
            PlayerNotificationText.text = "Attack aircraft are currently unavailable";
            Invoke("ClosePlayerNotification", 2.0f);
        }
        else if (Scoutaction > 0)
        {
            OpenPlayerNotification();
            PlayerNotificationText.text = "Patrol aircraft are currently unavailable";
            Invoke("ClosePlayerNotification", 2.0f);
        }
    }

    private void PerformAttack(string tileName)//攻击&伤害判断方法和发送信息
    {
        bool enemyFound = enemyState.enemyPosition == tileName;
        string attackResultInfo;

        if (enemyFound)
        {
            Debug.Log($"飞机起飞前往 {tileName} 进行攻击,并成功发现目标。");
            // 2/3 几率对敌舰造成伤害
            if (Random.Range(0, 3) < 2)
            {
                UpdateEnemyDamage();
            }
            else
            {
                attackResultInfo = $"attck {tileName} Succeeded, but no harm was done.";
                attackInfos.Add(attackResultInfo);
            }
        }
        else
        {
            Debug.Log($"飞机起飞前往 {tileName} 进行攻击。该区域未发现敌人。");
            attackResultInfo = $"attack {tileName} No enemy found.";
            attackInfos.Add(attackResultInfo);
        }

        Attackaction++;
        gameManager.attackmode = false; // 关闭攻击模式高光显示

        int AttackcooldownTurnsLeft = AttackTime - Attackaction;
        AttackAircraftInfoText.text = $"Waiting {AttackcooldownTurnsLeft} rounds";
        AttackAircraftStateText.text = "On task";
        AttackAircraftStateText.color = new Color32(255, 0, 0, 255);
    }

    private void UpdateEnemyDamage()//更新伤害方法
    {
        int damageType = Random.Range(0, 3); // 随机决定伤害类型
        EnemyStatus newStatus = EnemyStatus.MinorDamage; // 默认轻微受损

        switch (damageType)
        {
            case 0:
                newStatus = EnemyStatus.MinorDamage;
                break;
            case 1:
                newStatus = EnemyStatus.ModerateDamage;
                break;
            case 2:
                newStatus = EnemyStatus.SevereDamage;
                break;
        }

        enemyState.UpdateStatus(newStatus);
        string damageDescription = $"caused{newStatus}。";
        attackInfos.Add($"attack {enemyState.enemyPosition} Succeeded，" + damageDescription);
    }

    private void PerformScout(string tileName)//侦察判断方法
    {
        bool enemyFound = enemyState.enemyPosition == tileName;
        if (enemyFound)
        {
            Debug.Log($"飞机起飞前往 {tileName} 进行侦察。侦察机发现敌人。");
        }
        else
        {
            Debug.Log($"飞机起飞前往 {tileName} 进行侦察。该区域未发现敌人。");
        }
        ShowEnemyInfo(enemyState.enemyPosition, enemyState.nextEnemyPosition, gameManager.TurnNum, tileName);
        Scoutaction++;
        gameManager.scoutmode = false; // 关闭侦察模式高光显示

        int ScoutcooldownTurnsLeft = ScoutTime - Scoutaction;
        PatrolAircraftInfoText.text = $"Waiting {ScoutcooldownTurnsLeft} rounds"; // 修改UI飞机信息显示
        PatrolAircraftStateText.text = "On task";
        PatrolAircraftStateText.color = new Color32(255, 0, 0, 255);
    }

    public void ShowEnemyInfo(string enemyPosition, string nextEnemyPosition, int turnNum, string tileScouted)//发送全面的侦察信息
    {
        // 根据敌人是否被发现来构建不同的信息字符串
        string scoutResult = enemyPosition == tileScouted ? "Enemy found" : "No Enemy found";
        // 构建要显示的信息字符串，包括回合数、侦察区域和侦察结果
        string info = $"At{turnNum}Round，Send{tileScouted}a reconnaissance aircraft，{scoutResult}。";

        if (enemyPosition == tileScouted)
        {
            // 如果发现了敌人，追加敌人下回合预计的移动目标区域和当前状态
            string statusDescription = ConvertStatusToString(gameManager.enemyState.status);
            info += $"\nThe target area where the enemy will move next turn:{nextEnemyPosition}。\nCurrent status:{statusDescription}。";
        }

        // 调用方法将信息添加到缓存中
        scoutInfos.Add(info);
    }

    private string ConvertStatusToString(EnemyStatus status)//根据敌舰状态返回对应字符串格式内容
    {
        switch (status)
        {
            case EnemyStatus.Normal:
                return "正常航行";
            case EnemyStatus.MinorDamage:
                return "轻微受损";
            case EnemyStatus.ModerateDamage:
                return "中度受损";
            case EnemyStatus.SevereDamage:
                return "严重受损";
            case EnemyStatus.Sunk:
                return "已沉没";
            default:
                return "未知";
        }
    }

    public void UpdatePlaneActions()//更新飞机显示在UI上的状态
    {
        if (Attackaction != 0)
        {
            Attackaction++;
            // 更新攻击机信息文本以显示冷却倒计时
            int AttackcooldownTurnsLeft = AttackTime - Attackaction;
            AttackAircraftInfoText.text = $"Waitting {AttackcooldownTurnsLeft} rounds";
        }
        else
        {
            AttackAircraftInfoText.text = " ";
        }
        if (Attackaction >= AttackTime)
        {
            Attackaction = 0;
            AttackAircraftStateText.text = "On Board";
            AttackAircraftStateText.color = new Color32(255, 255, 255, 255);
            // 当巡逻机准备好时，更新信息文本通知玩家
            AttackAircraftInfoText.text = " ";
        }
        if (Scoutaction != 0)
        {
            Scoutaction++;
            // 更新巡逻机信息文本以显示冷却倒计时
            int ScoutcooldownTurnsLeft = ScoutTime - Scoutaction;
            PatrolAircraftInfoText.text = $"Waitting {ScoutcooldownTurnsLeft} rounds";

        }
        else
        {
            PatrolAircraftInfoText.text = " ";
        }
        if (Scoutaction >= ScoutTime)
        {
            Scoutaction = 0;
            PatrolAircraftStateText.text = "On Board";
            PatrolAircraftStateText.color = new Color32(255, 255, 255, 255);
            // 当巡逻机准备好时，更新信息文本通知玩家
            PatrolAircraftInfoText.text = " ";
        }
    }

    public void OpenPlayerNotification()//显示飘字
    {
        PlayerNotification.SetActive(true);
    }
    public void ClosePlayerNotification()//关闭飘字
    {
        PlayerNotification.SetActive(false);
    }
}
