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
    public List<string> landEnemyExist = new List<string>();// List�����ڴ洢�ؿ��Ƿ���ڵ��� 
    private Dictionary<string, string> nextEnemyPositions = new Dictionary<string, string>(); // �洢������һ��Ԥ�Ƶ�λ��


    // Start is called before the first frame update

    void Start()
    {
        CalculateNextEnemyMove(); // ��Ϸ��ʼʱ������˵ĵ�һ��
    }

    public void NextTurn()
    {
        TurnNum++;
        RoundMove = true;
        print("������" + TurnNum + "�غ�");

        // �ڻغϿ�ʼǰ��ӡ������Ϣ
        EnemyInfo();

        // ����Ԥ�Ƶ��ƶ���Ϣʵ���ƶ�����
        ApplyEnemyMoves();

        // Ϊ��һ�غϼ����µ��ƶ�
        CalculateNextEnemyMove();

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
        nextEnemyPositions.Clear(); // �����һ�غϵ��ƶ���Ϣ

        foreach (var currentLocation in landEnemyExist)
        {
            string[] parts = currentLocation.Split(' ');
            int x = int.Parse(parts[1]);
            int y = int.Parse(parts[2]);

            bool isValidMove = false;
            int newX = x, newY = y;
            int attemptCount = 0; // ��������ѭ��

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
                string newLocation = $"Tile {newX} {newY}";
                nextEnemyPositions[currentLocation] = newLocation;
            }
        }
    }

    private void ApplyEnemyMoves()
    {
        foreach (var enemyMove in nextEnemyPositions)
        {
            if (landEnemyExist.Contains(enemyMove.Key))
            {
                landEnemyExist.Remove(enemyMove.Key);
                landEnemyExist.Add(enemyMove.Value);
            }
        }
    }

    public void EnemyInfo()
    {
        foreach (var enemyMove in nextEnemyPositions)
        {
            print($"���˴� {enemyMove.Key} Ԥ���ƶ��� {enemyMove.Value}");
        }
    }
}



