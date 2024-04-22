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

    // 可以通过某种方式设置当前选中的ChessTile，例如通过鼠标点击
    public ChessTile currentTile;

    void Start()
    {
        PatrolAircraftInfoText.text = " ";
        PatrolAircraftStateText.text = "On Board";
        AttackAircraftInfoText.text = " ";
        AttackAircraftStateText.text = "On Board";
    }
    public void PerformAction()
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

    private void PerformAttack(string tileName)
    {
        bool enemyFound = enemyState.enemyPosition == tileName;
        string attackResultInfo;

        if (enemyFound)
        {
            Debug.Log($"飞机起飞前往 {tileName} 进行攻击。攻击命中。");
            attackResultInfo = $"攻击 {tileName} 成功。";
        }
        else
        {
            Debug.Log($"飞机起飞前往 {tileName} 进行攻击。该区域未发现敌人。");
            attackResultInfo = $"攻击 {tileName} 未发现敌人。";
        }
        intelligence.AddAttackInfo(attackResultInfo); // 调用新的方法添加攻击信息到ScrollView

        Attackaction++;
        gameManager.attackmode = false; // 关闭攻击模式高光显示

        int AttackcooldownTurnsLeft = AttackTime - Attackaction;
        AttackAircraftInfoText.text = $"Waiting {AttackcooldownTurnsLeft} rounds"; // 修改UI飞机信息显示
        AttackAircraftStateText.text = "On task";
        AttackAircraftStateText.color = new Color32(255, 0, 0, 255);
    }

    private void PerformScout(string tileName)
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
        intelligence.ShowEnemyInfo(enemyState.enemyPosition, enemyState.nextEnemyPosition, gameManager.TurnNum, tileName);
        Scoutaction++;
        gameManager.scoutmode = false; // 关闭侦察模式高光显示

        int ScoutcooldownTurnsLeft = ScoutTime - Scoutaction;
        PatrolAircraftInfoText.text = $"Waiting {ScoutcooldownTurnsLeft} rounds"; // 修改UI飞机信息显示
        PatrolAircraftStateText.text = "On task";
        PatrolAircraftStateText.color = new Color32(255, 0, 0, 255);
    }
    public void UpdatePlaneActions()
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
    public void OpenPlayerNotification()
    {
        PlayerNotification.SetActive(true);
    }
    public void ClosePlayerNotification()
    {
        PlayerNotification.SetActive(false);
    }
}
