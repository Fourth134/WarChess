using System.Collections;
using System.Collections.Generic;
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

    public float CurrentObjectX;
    public float CurrentObjectZ; //��ǰ���������¼����һ����ȷ����

    
    public string[] landProperties;// String���飬���ڴ洢�ؿ��Ƿ�Ϊ����
    public List<string> landEnemyExist = new List<string>();// List�����ڴ洢�ؿ��Ƿ���ڵ��� 

    // Start is called before the first frame update

    public void NextTurn()
    {
        TurnNum++;
        RoundMove = true;
        print("������"+ TurnNum+"�غ�");
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
        MoveEnemy();
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

    public void EnemyInfo()
    {

    }

    public void MoveEnemy()
    {
        if (landEnemyExist.Count == 0)
        {
            print("��ͼ��û�е���");
            return;
        }


        // ���ѡ��һ�����˵ĵ�ǰλ��
        int index = Random.Range(0, landEnemyExist.Count);
        string currentLocation = landEnemyExist[index];

        // ������ǰλ�õ�x��y����
        string[] parts = currentLocation.Split(' ');
        int x = int.Parse(parts[1]);
        int y = int.Parse(parts[2]);

        // ��������ƶ�����
        int moveX = Random.Range(-1, 2); // �������-1��0��1
        int moveY = Random.Range(-1, 2); // �������-1��0��1

        // ������λ�ã�ȷ����Խ��
        int newX = Mathf.Clamp(x + moveX, 0, 8);
        int newY = Mathf.Clamp(y + moveY, 0, 8);

        // ������λ�õ��ַ�����ʾ
        string newLocation = $"Tile {newX} {newY}";

        // ���µ���λ��
        landEnemyExist[index] = newLocation;
    }
}

