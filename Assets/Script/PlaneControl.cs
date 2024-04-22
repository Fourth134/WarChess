using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneControl : MonoBehaviour
{
    public GameManager gameManager;
    public Intelligence intelligence;
    public EnemyState enemyState;//��ȡEnemyState�ű�

    public GameObject PlayerNotification;//����Ʈ���������

    public int Attackaction = 0; // �����Ƿ���Խ�����죨0������ԣ�
    public int AttackTime; // ������������׼���Ļغ���
    public int Scoutaction = 0; // �����Ƿ���Խ�����죨0������ԣ�
    public int ScoutTime; // ������������׼���Ļغ���

    public Text PlayerNotificationText;//����Ʈ��
    public Text PatrolAircraftStateText; // ��������״̬UI�ı�
    public Text PatrolAircraftInfoText; // ����������ϢUI�ı�
    public Text AttackAircraftStateText; // ��������״̬UI�ı�
    public Text AttackAircraftInfoText; // ����������ϢUI�ı�

    // ����ͨ��ĳ�ַ�ʽ���õ�ǰѡ�е�ChessTile������ͨ�������
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
            Debug.Log("�豣֤PlaneControl����gameManager, intelligence, �� currentTile δ��ʼ��");
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
            Debug.Log($"�ɻ����ǰ�� {tileName} ���й������������С�");
            attackResultInfo = $"���� {tileName} �ɹ���";
        }
        else
        {
            Debug.Log($"�ɻ����ǰ�� {tileName} ���й�����������δ���ֵ��ˡ�");
            attackResultInfo = $"���� {tileName} δ���ֵ��ˡ�";
        }
        intelligence.AddAttackInfo(attackResultInfo); // �����µķ�����ӹ�����Ϣ��ScrollView

        Attackaction++;
        gameManager.attackmode = false; // �رչ���ģʽ�߹���ʾ

        int AttackcooldownTurnsLeft = AttackTime - Attackaction;
        AttackAircraftInfoText.text = $"Waiting {AttackcooldownTurnsLeft} rounds"; // �޸�UI�ɻ���Ϣ��ʾ
        AttackAircraftStateText.text = "On task";
        AttackAircraftStateText.color = new Color32(255, 0, 0, 255);
    }

    private void PerformScout(string tileName)
    {
        bool enemyFound = enemyState.enemyPosition == tileName;
        if (enemyFound)
        {
            Debug.Log($"�ɻ����ǰ�� {tileName} ������졣�������ֵ��ˡ�");
        }
        else
        {
            Debug.Log($"�ɻ����ǰ�� {tileName} ������졣������δ���ֵ��ˡ�");
        }
        intelligence.ShowEnemyInfo(enemyState.enemyPosition, enemyState.nextEnemyPosition, gameManager.TurnNum, tileName);
        Scoutaction++;
        gameManager.scoutmode = false; // �ر����ģʽ�߹���ʾ

        int ScoutcooldownTurnsLeft = ScoutTime - Scoutaction;
        PatrolAircraftInfoText.text = $"Waiting {ScoutcooldownTurnsLeft} rounds"; // �޸�UI�ɻ���Ϣ��ʾ
        PatrolAircraftStateText.text = "On task";
        PatrolAircraftStateText.color = new Color32(255, 0, 0, 255);
    }
    public void UpdatePlaneActions()
    {
        if (Attackaction != 0)
        {
            Attackaction++;
            // ���¹�������Ϣ�ı�����ʾ��ȴ����ʱ
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
            // ��Ѳ�߻�׼����ʱ��������Ϣ�ı�֪ͨ���
            AttackAircraftInfoText.text = " ";
        }
        if (Scoutaction != 0)
        {
            Scoutaction++;
            // ����Ѳ�߻���Ϣ�ı�����ʾ��ȴ����ʱ
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
            // ��Ѳ�߻�׼����ʱ��������Ϣ�ı�֪ͨ���
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
