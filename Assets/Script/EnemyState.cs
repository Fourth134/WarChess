using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EnemyStatus { Normal, MinorDamage, ModerateDamage, SevereDamage, Sunk }

public class EnemyState : MonoBehaviour
{
    public string enemyPosition; // 当前敌人位置
    public string nextEnemyPosition; // 预计下一回合敌人的位置
    public int enemyMoveDistance; // 敌人移动距离
    public string targetTile;//目标格块
    public int tendency;//移动倾向（0-9）
    public GridManager gridManager; // 引用GridManager脚本
    public GameManager gameManager;// 引用GameManager脚本
    public EnemyStatus status = EnemyStatus.Normal;
    public GameObject QuitMenu;//退出菜单

    public void StartEnemyMove()//计算敌舰移动
    {
        CalculateNextEnemyMove();
        EnemyInfo();
    }

    public void ApplyEnemyMoves()//进行移动
    {
        if (status != EnemyStatus.Sunk)
        {
            enemyPosition = nextEnemyPosition;
        }
    }
    public void UpdateStatusBasedOnDamage()
    {
        if (status == EnemyStatus.Normal || status == EnemyStatus.Sunk) return;

        int chance = Random.Range(0, 3); // 0, 1, or 2

        if (chance == 0) // 1/3 probability to change state
        {
            if (status > EnemyStatus.Normal && status < EnemyStatus.Sunk)
            {
                status--; // better
            }
        }
        else if (chance == 1)
        {
            if (status < EnemyStatus.SevereDamage)
            {
                status++; // worse
            }
        }

        Debug.Log($"Each round, the enemy ship status is updated to：{status}");
    }

    public void CalculateNextEnemyMove()
    {
        if (status == EnemyStatus.Sunk)
        {
            Debug.Log("The enemy ship has sunk and cannot move.");
            QuitMenu.SetActive(true);
            return;
        }

        bool canMove = true;
        if (status == EnemyStatus.ModerateDamage)
        {
            canMove = Random.Range(0, 3) != 0; // 1/3 chance of being unable to move
        }
        else if (status == EnemyStatus.SevereDamage)
        {
            canMove = Random.Range(0, 3) == 0; // 2/3 chance of being unable to move
        }

        if (!canMove)
        {
            Debug.Log("Due to damage, the enemy ship cannot move this turn.");
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

        while (!isValidMove && attemptCount < 100)
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
        if (status != EnemyStatus.Sunk) // 如果敌舰未沉没，才更新状态
        {
            status = newStatus;
            Debug.Log($"敌舰状态更新为：{status}");
        }
        else
        {
            Debug.Log("敌舰已经沉没");
            QuitMenu.SetActive(true);
        }
    }

}

