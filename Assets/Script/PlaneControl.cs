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
    public List<string> attackInfos = new List<string>();//���汾�غϹ�����Ϣ
    public List<string> scoutInfos = new List<string>();//���汾�غ������Ϣ

    // ����ͨ��ĳ�ַ�ʽ���õ�ǰѡ�е�ChessTile������ͨ�������
    public ChessTile currentTile;

    void Start()
    {
        PatrolAircraftInfoText.text = " ";
        PatrolAircraftStateText.text = "On Board";
        AttackAircraftInfoText.text = " ";
        AttackAircraftStateText.text = "On Board";
    }
    public void PerformAction()//��������촥������
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

    private void PerformAttack(string tileName)//����&�˺��жϷ����ͷ�����Ϣ
    {
        bool enemyFound = enemyState.enemyPosition == tileName;
        string attackResultInfo;

        if (enemyFound)
        {
            Debug.Log($"�ɻ����ǰ�� {tileName} ���й���,���ɹ�����Ŀ�ꡣ");
            // 2/3 ���ʶԵн�����˺�
            if (Random.Range(0, 3) < 2)
            {
                UpdateEnemyDamage();
            }
            else
            {
                attackResultInfo = $"attck {tileName} Succeeded, but no harm was done.";
                attackInfos.Add(attackResultInfo);
            }
        }
        else
        {
            Debug.Log($"�ɻ����ǰ�� {tileName} ���й�����������δ���ֵ��ˡ�");
            attackResultInfo = $"attack {tileName} No enemy found.";
            attackInfos.Add(attackResultInfo);
        }

        Attackaction++;
        gameManager.attackmode = false; // �رչ���ģʽ�߹���ʾ

        int AttackcooldownTurnsLeft = AttackTime - Attackaction;
        AttackAircraftInfoText.text = $"Waiting {AttackcooldownTurnsLeft} rounds";
        AttackAircraftStateText.text = "On task";
        AttackAircraftStateText.color = new Color32(255, 0, 0, 255);
    }

    private void UpdateEnemyDamage()//�����˺�����
    {
        int damageType = Random.Range(0, 3); // ��������˺�����
        EnemyStatus newStatus = EnemyStatus.MinorDamage; // Ĭ����΢����

        switch (damageType)
        {
            case 0:
                newStatus = EnemyStatus.MinorDamage;
                break;
            case 1:
                newStatus = EnemyStatus.ModerateDamage;
                break;
            case 2:
                newStatus = EnemyStatus.SevereDamage;
                break;
        }

        enemyState.UpdateStatus(newStatus);
        string damageDescription = $"caused{newStatus}��";
        attackInfos.Add($"attack {enemyState.enemyPosition} Succeeded��" + damageDescription);
    }

    private void PerformScout(string tileName)//����жϷ���
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
        ShowEnemyInfo(enemyState.enemyPosition, enemyState.nextEnemyPosition, gameManager.TurnNum, tileName);
        Scoutaction++;
        gameManager.scoutmode = false; // �ر����ģʽ�߹���ʾ

        int ScoutcooldownTurnsLeft = ScoutTime - Scoutaction;
        PatrolAircraftInfoText.text = $"Waiting {ScoutcooldownTurnsLeft} rounds"; // �޸�UI�ɻ���Ϣ��ʾ
        PatrolAircraftStateText.text = "On task";
        PatrolAircraftStateText.color = new Color32(255, 0, 0, 255);
    }

    public void ShowEnemyInfo(string enemyPosition, string nextEnemyPosition, int turnNum, string tileScouted)//����ȫ��������Ϣ
    {
        // ���ݵ����Ƿ񱻷�����������ͬ����Ϣ�ַ���
        string scoutResult = enemyPosition == tileScouted ? "Enemy found" : "No Enemy found";
        // ����Ҫ��ʾ����Ϣ�ַ����������غ������������������
        string info = $"At{turnNum}Round��Send{tileScouted}a reconnaissance aircraft��{scoutResult}��";

        if (enemyPosition == tileScouted)
        {
            // ��������˵��ˣ�׷�ӵ����»غ�Ԥ�Ƶ��ƶ�Ŀ������͵�ǰ״̬
            string statusDescription = ConvertStatusToString(gameManager.enemyState.status);
            info += $"\nThe target area where the enemy will move next turn:{nextEnemyPosition}��\nCurrent status:{statusDescription}��";
        }

        // ���÷�������Ϣ��ӵ�������
        scoutInfos.Add(info);
    }

    private string ConvertStatusToString(EnemyStatus status)//���ݵн�״̬���ض�Ӧ�ַ�����ʽ����
    {
        switch (status)
        {
            case EnemyStatus.Normal:
                return "��������";
            case EnemyStatus.MinorDamage:
                return "��΢����";
            case EnemyStatus.ModerateDamage:
                return "�ж�����";
            case EnemyStatus.SevereDamage:
                return "��������";
            case EnemyStatus.Sunk:
                return "�ѳ�û";
            default:
                return "δ֪";
        }
    }

    public void UpdatePlaneActions()//���·ɻ���ʾ��UI�ϵ�״̬
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

    public void OpenPlayerNotification()//��ʾƮ��
    {
        PlayerNotification.SetActive(true);
    }
    public void ClosePlayerNotification()//�ر�Ʈ��
    {
        PlayerNotification.SetActive(false);
    }
}
