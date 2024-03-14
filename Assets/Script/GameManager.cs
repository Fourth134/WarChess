using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject CurrentObject;
    public GameObject CurrentSealand;
    public bool set = false;//�ж��Ƿ񴥷�������������
    public bool ShowRangHighlight = false;//�жϾ���߹���ʾ

    public bool RoundMove = true;//���汾�غ��Ƿ�����ƶ�
    public int TurnNum=1;//���浱ǰ�غ���
    public bool attackmode = false;//���浱ǰ�Ƿ��ǹ���ģʽ
    public bool scoutmode = false;//���浱ǰ�Ƿ������ģʽ
    public bool Roundaction = true;//���汾�غ��Ƿ����һ�ζ���
    public int Scoutaction = 0;//�����Ƿ���Խ������
    public int ScoutTime;//������������׼���Ļغ���
    public int EnemymoveDistance;//�����ƶ�����
    public string targetTile;//����Ŀ���λ��
    public int tendency;//������Ŀ����ƶ�����
    public GridManager gridManager;//��ȡGridManager�ű�

    public float CurrentObjectX;
    public float CurrentObjectZ; //��ǰ���������¼����һ����ȷ����

    
    public string[] landProperties;// String���飬���ڴ洢�ؿ��Ƿ�Ϊ����
    public string enemyPosition; // �滻ԭ���� landEnemyExist �б�����ֻ׷��һ�����˵�λ��
    public string nextEnemyPosition; // Ԥ����һ�غϵ��˵�λ��


    // Start is called before the first frame update

    void Start()
    {
        CalculateNextEnemyMove(); // ��Ϸ��ʼʱ������˵ĵ�һ��
        EnemyInfo();
    }

    public void NextTurn()
    {
        TurnNum++;
        RoundMove = true;
        print("������" + TurnNum + "�غ�");

        // ����Ԥ�Ƶ��ƶ���Ϣʵ���ƶ�����
        ApplyEnemyMoves();
        // Ϊ��һ�غϼ����µ��ƶ�
        CalculateNextEnemyMove();
        // ���»غϿ�ʼǰ��ӡ������Ϣ
        EnemyInfo();

        // ����������Ϸ״̬
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
            print("�����Ѿ�����");
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
        // �����һ�غϵ��ƶ���Ϣ
        bool isValidMove = false;
        int attemptCount = 0; // ��������ѭ��
        int x, y, newX = 0, newY = 0;

        string[] parts = enemyPosition.Split(' ');
        x = int.Parse(parts[1]);
        y = int.Parse(parts[2]);

        while (!isValidMove && attemptCount < 10)
        {
            attemptCount++;
            int directionX = Random.Range(-EnemymoveDistance, EnemymoveDistance + 1);
            int directionY = Random.Range(-EnemymoveDistance, EnemymoveDistance + 1);

            newX = Mathf.Clamp(x + directionX, 0, gridManager._width - 1);
            newY = Mathf.Clamp(y + directionY, 0, gridManager._height - 1);

            string potentialNewLocation = $"Tile {newX} {newY}";

            // �����λ���Ƿ�����ƶ������������Ƿ�Ϊ����
            if (!landProperties.Contains(potentialNewLocation))
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

    private void ApplyEnemyMoves() // ʵ���ƶ����˵ķ���
    {
        enemyPosition = nextEnemyPosition;
    }

    public void EnemyInfo()
    {
        print($"���˴� {enemyPosition} Ԥ���ƶ��� {nextEnemyPosition}");
    }
}




