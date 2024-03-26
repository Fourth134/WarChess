using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneControl : MonoBehaviour
{
    public GameManager gameManager;
    public Intelligence intelligence;

    public GameObject PlayerNotification;//引用飘字物体组件

    public bool Roundaction = true; // 储存本回合是否完成攻击动作
    public int Scoutaction = 0; // 储存是否可以进行侦察（0代表可以）
    public int ScoutTime; // 储存侦察机进行准备的回合数

    public Text PlayerNotificationText;//引用飘字
    public Text PatrolAircraftStateText; // 引用侦察机状态UI文本
    public Text PatrolAircraftInfoText; // 引用侦察机信息UI文本

    // 可以通过某种方式设置当前选中的ChessTile，例如通过鼠标点击
    public ChessTile currentTile;

    void Start()
    {
        PatrolAircraftInfoText.text = " ";
        PatrolAircraftStateText.text = "On Board";
    }
    public void PerformAction()
    {
        if (gameManager == null || intelligence == null || currentTile == null)
        {
            print("需保证PlaneControl对象gameManager == null || intelligence == null || currentTile == null");
            return;
        }


        string tileName = currentTile.gameObject.name;

        if (gameManager.attackmode && Roundaction)
        {
            bool enemyFound = gameManager.enemyPosition == tileName;
            if (enemyFound)
            {
                Debug.Log($"飞机起飞前往 {tileName} 进行攻击。攻击命中。");
            }
            else
            {
                Debug.Log($"飞机起飞前往 {tileName} 进行攻击。该区域未发现敌人。");
            }
            Roundaction = false;
            gameManager.attackmode = false; // 关闭攻击模式高光显示
        }
        else if (gameManager.scoutmode && Scoutaction == 0)
        {
            bool enemyFound = gameManager.enemyPosition == tileName;
            if (enemyFound)
            {
                Debug.Log($"飞机起飞前往 {tileName} 进行侦察。侦察机发现敌人。");
            }
            else
            {
                Debug.Log($"飞机起飞前往 {tileName} 进行侦察。该区域未发现敌人。");
            }
            intelligence.ShowEnemyInfo(gameManager.enemyPosition, gameManager.nextEnemyPosition, gameManager.TurnNum, tileName);

            Scoutaction++;
            int cooldownTurnsLeft = ScoutTime - Scoutaction;
            PatrolAircraftInfoText.text = $"Waitting {cooldownTurnsLeft} rounds";//修改UI飞机信息显示
            PatrolAircraftStateText.text = "On task";
            PatrolAircraftStateText.color = new Color32(255, 0, 0, 255);
            gameManager.scoutmode = false; // 关闭侦察模式高光显示
        }
        else if (!Roundaction)
        {
            OpenPlayerNotification();
            PlayerNotificationText.text = "Attack aircraft are currently unavailable";
            gameManager.AttackSwitch();
            Invoke("ClosePlayerNotification", 1.0f);
        }
        else if (Scoutaction>0)
        {
            OpenPlayerNotification();
            gameManager.ScoutSwitch();
            PlayerNotificationText.text = "Patrol aircraft are currently unavailable";
            Invoke("ClosePlayerNotification", 1.0f);
        }
    }
    public void UpdatePlaneActions()
    {
        Roundaction = true;
        if (Scoutaction != 0)
        {
            Scoutaction++;
            // 更新巡逻机信息文本以显示冷却倒计时
            int cooldownTurnsLeft = ScoutTime - Scoutaction;
            PatrolAircraftInfoText.text = $"Waitting {cooldownTurnsLeft} rounds";
 
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
