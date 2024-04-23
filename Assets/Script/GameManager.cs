using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GridManager gridManager;//��ȡGridManager�ű�
    public PlaneControl planeControl;//��ȡPlaneControl�ű�
    public EnemyState enemyState;//��ȡEnemyState�ű�
    public Intelligence intelligence;//��ȡIntelligence�ű�

    public float CurrentObjectX;
    public float CurrentObjectZ; //��ǰ���������¼����һ����ȷ����

    public Text turnCounterText; // UI TextԪ�أ�������ʾ��ǰ�غ���
    public string[] landProperties;// String���飬���ڴ洢�ؿ��Ƿ�Ϊ����

    // Start is called before the first frame update

    void Start()
    {
        // ���е��˳�ʼ���㣬�ⲿ�ֿ�����EnemyState�ű��Լ�����Start�����д���
        enemyState.StartEnemyMove();
    }

    public void NextTurn()
    {
        TurnNum++;
        RoundMove = true;
        print("������" + TurnNum + "�غ�");
        if (turnCounterText != null)
        {
            turnCounterText.text = "��ǰ�غ�: " + TurnNum; // ����UI Text��ʾ��ǰ�غ���
        }
        // Ӧ�õ��˵��ƶ�
        enemyState.ApplyEnemyMoves();
        // ���������һ�غϵ��ƶ�
        enemyState.CalculateNextEnemyMove();
        // �ڿ���̨���������Ϣ
        enemyState.EnemyInfo();

        // ����������Ϸ״̬
        attackmode = false;
        scoutmode = false;
        planeControl.UpdatePlaneActions();//�����йطɻ��Ļغ��ж�
        DisplayPreviousRoundInfo();// ��ʾ��һ�غϵĹ����������Ϣ
    }

    public void DisplayPreviousRoundInfo()// ��ʾ��һ�غϵĹ����������Ϣ
    {
        
        foreach (string info in planeControl.attackInfos)
        {
            intelligence.AddAttackInfo(info);
        }
        planeControl.attackInfos.Clear();

        foreach (string info in planeControl.scoutInfos)
        {
            intelligence.AddScoutInfo(info);
        }
        planeControl.scoutInfos.Clear();
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

}




