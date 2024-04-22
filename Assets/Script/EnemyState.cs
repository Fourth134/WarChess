using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EnemyStatus { Normal, MinorDamage, ModerateDamage, SevereDamage, Sunk }

public class EnemyState : MonoBehaviour
{
    public string enemyPosition;
    public string nextEnemyPosition;
    public int enemyMoveDistance;
    public string targetTile;//目标格块
    public int tendency;//移动倾向（0-9）
    public GridManager gridManager;
    public GameManager gameManager;
    public EnemyStatus status = EnemyStatus.Normal;

    public void StartEnemyMove()
    {
        CalculateNextEnemyMove();
        EnemyInfo();
    }

    public void ApplyEnemyMoves()
    {
        if (status != EnemyStatus.Sunk)
        {
            enemyPosition = nextEnemyPosition;
        }
    }

    public void CalculateNextEnemyMove()
    {
        if (status == EnemyStatus.Sunk)
        {
            Debug.Log("敌舰已沉没，无法移动。");
            return;
        }

        bool canMove = true;
        if (status == EnemyStatus.ModerateDamage)
        {
            canMove = Random.Range(0, 3) != 0; // 1/3 几率不能移动
        }
        else if (status == EnemyStatus.SevereDamage)
        {
            canMove = Random.Range(0, 3) == 0; // 2/3 几率不能移动
        }

        if (!canMove)
        {
            Debug.Log("由于受损，敌舰本回合无法移动。");
            return;
        }
        else
        {
            AttemptToMoveEnemy();
        }
    }

    void AttemptToMoveEnemy()
    {
        bool isValidMove = false;
        int attemptCount = 0; // 避免无限循环
        int x, y, newX = 0, newY = 0;

        string[] parts = enemyPosition.Split(' ');
        x = int.Parse(parts[1]);
        y = int.Parse(parts[2]);

        string[] targetParts = targetTile.Split(' ');
        int targetX = int.Parse(targetParts[1]);
        int targetY = int.Parse(targetParts[2]);

        while (!isValidMove && attemptCount < 20)
        {
            attemptCount++;
            int directionX = Random.Range(-enemyMoveDistance, enemyMoveDistance + 1);
            int directionY = Random.Range(-enemyMoveDistance, enemyMoveDistance + 1);

            // 增加判断敌人移动方向是否靠近目标点的逻辑
            if (Random.Range(0, 11) <= tendency) // 如果随机数小于等于倾向值，敌人朝目标移动
            {
                directionX = targetX - x;
                directionY = targetY - y;
                directionX = Mathf.Clamp(directionX, -enemyMoveDistance, enemyMoveDistance);
                directionY = Mathf.Clamp(directionY, -enemyMoveDistance, enemyMoveDistance);
            }

            newX = Mathf.Clamp(x + directionX, 0, gridManager._width - 1);
            newY = Mathf.Clamp(y + directionY, 0, gridManager._height - 1);

            string potentialNewLocation = $"Tile {newX} {newY}";

            // 检查新位置是否符合移动条件（例如是否为海洋）
            if (!gameManager.landProperties.Contains(potentialNewLocation))
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

    public void EnemyInfo()
    {
        Debug.Log($"敌人从 {enemyPosition} 预计移动到 {nextEnemyPosition}");
    }

    // 用于外部修改敌人状态的方法
    public void UpdateStatus(EnemyStatus newStatus)
    {
        status = newStatus;
        Debug.Log($"敌舰状态更新为：{status}");
    }
}

