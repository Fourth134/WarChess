using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneControl : MonoBehaviour
{
    public GameManager gameManager;
    public Intelligence intelligence;

    // 可以通过某种方式设置当前选中的ChessTile，例如通过鼠标点击
    public ChessTile currentTile;

    // 调用该方法以执行攻击或侦察逻辑
    public void PerformAction()
    {
        if (gameManager == null || intelligence == null || currentTile == null)
        {
            print("需保证PlaneControl对象gameManager == null || intelligence == null || currentTile == null");
            return;
        }


        string tileName = currentTile.gameObject.name;

        if (gameManager.attackmode && gameManager.Roundaction)
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
            gameManager.Roundaction = false;
            gameManager.attackmode = false; // 关闭攻击模式高光显示
        }
        else if (gameManager.scoutmode && gameManager.Scoutaction == 0)
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
            gameManager.Scoutaction++;
            gameManager.scoutmode = false; // 关闭侦察模式高光显示
        }
    }
}
