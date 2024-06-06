using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EnemyStatus { Normal, MinorDamage, ModerateDamage, SevereDamage, Sunk }

public class EnemyState : MonoBehaviour
{
    public string enemyPosition; // ��ǰ����λ��
    public string nextEnemyPosition; // Ԥ����һ�غϵ��˵�λ��
    public int enemyMoveDistance; // �����ƶ�����
    public string targetTile;//Ŀ����
    public int tendency;//�ƶ�����0-9��
    public GridManager gridManager; // ����GridManager�ű�
    public GameManager gameManager;// ����GameManager�ű�
    public EnemyStatus status = EnemyStatus.Normal;
    public GameObject QuitMenu;//�˳��˵�

    public void StartEnemyMove()//����н��ƶ�
    {
        CalculateNextEnemyMove();
        EnemyInfo();
    }

    public void ApplyEnemyMoves()//�����ƶ�
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

        Debug.Log($"Each round, the enemy ship status is updated to��{status}");
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
        int attemptCount = 0; // ��������ѭ��
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

            // �����жϵ����ƶ������Ƿ񿿽�Ŀ�����߼�
            if (Random.Range(0, 11) <= tendency) // ��������С�ڵ�������ֵ�����˳�Ŀ���ƶ�
            {
                directionX = targetX - x;
                directionY = targetY - y;
                directionX = Mathf.Clamp(directionX, -enemyMoveDistance, enemyMoveDistance);
                directionY = Mathf.Clamp(directionY, -enemyMoveDistance, enemyMoveDistance);
            }

            newX = Mathf.Clamp(x + directionX, 0, gridManager._width - 1);
            newY = Mathf.Clamp(y + directionY, 0, gridManager._height - 1);

            string potentialNewLocation = $"Tile {newX} {newY}";

            // �����λ���Ƿ�����ƶ������������Ƿ�Ϊ����
            if (!gameManager.landProperties.Contains(potentialNewLocation))
            {
                isValidMove = true; // �ҵ�һ����Ч���ƶ�λ��
            }
        }

        if (isValidMove)
        {
            // �洢����Ԥ�Ƶ��ƶ�������������ִ��
            nextEnemyPosition = $"Tile {newX} {newY}";
        }
    }

    public void EnemyInfo()
    {
        Debug.Log($"���˴� {enemyPosition} Ԥ���ƶ��� {nextEnemyPosition}");
    }

    // �����ⲿ�޸ĵ���״̬�ķ���
    public void UpdateStatus(EnemyStatus newStatus)
    {
        if (status != EnemyStatus.Sunk) // ����н�δ��û���Ÿ���״̬
        {
            status = newStatus;
            Debug.Log($"�н�״̬����Ϊ��{status}");
        }
        else
        {
            Debug.Log("�н��Ѿ���û");
            QuitMenu.SetActive(true);
        }
    }

}

