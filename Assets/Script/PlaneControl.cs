using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneControl : MonoBehaviour
{
    public GameManager gameManager;
    public Intelligence intelligence;

    public GameObject PlayerNotification;//����Ʈ���������

    public bool Roundaction = true; // ���汾�غ��Ƿ���ɹ�������
    public int Scoutaction = 0; // �����Ƿ���Խ�����죨0������ԣ�
    public int ScoutTime; // ������������׼���Ļغ���

    public Text PlayerNotificationText;//����Ʈ��
    public Text PatrolAircraftStateText; // ��������״̬UI�ı�
    public Text PatrolAircraftInfoText; // ����������ϢUI�ı�

    // ����ͨ��ĳ�ַ�ʽ���õ�ǰѡ�е�ChessTile������ͨ�������
    public ChessTile currentTile;

    void Start()
    {
        PatrolAircraftInfoText.text = " ";
        PatrolAircraftStateText.text = "On Board";
    }
    public void PerformAction()
    {
        if (gameManager == null || intelligence == null || currentTile == null)
        {
            print("�豣֤PlaneControl����gameManager == null || intelligence == null || currentTile == null");
            return;
        }


        string tileName = currentTile.gameObject.name;

        if (gameManager.attackmode && Roundaction)
        {
            bool enemyFound = gameManager.enemyPosition == tileName;
            if (enemyFound)
            {
                Debug.Log($"�ɻ����ǰ�� {tileName} ���й������������С�");
            }
            else
            {
                Debug.Log($"�ɻ����ǰ�� {tileName} ���й�����������δ���ֵ��ˡ�");
            }
            Roundaction = false;
            gameManager.attackmode = false; // �رչ���ģʽ�߹���ʾ
        }
        else if (gameManager.scoutmode && Scoutaction == 0)
        {
            bool enemyFound = gameManager.enemyPosition == tileName;
            if (enemyFound)
            {
                Debug.Log($"�ɻ����ǰ�� {tileName} ������졣�������ֵ��ˡ�");
            }
            else
            {
                Debug.Log($"�ɻ����ǰ�� {tileName} ������졣������δ���ֵ��ˡ�");
            }
            intelligence.ShowEnemyInfo(gameManager.enemyPosition, gameManager.nextEnemyPosition, gameManager.TurnNum, tileName);

            Scoutaction++;
            int cooldownTurnsLeft = ScoutTime - Scoutaction;
            PatrolAircraftInfoText.text = $"Waitting {cooldownTurnsLeft} rounds";//�޸�UI�ɻ���Ϣ��ʾ
            PatrolAircraftStateText.text = "On task";
            PatrolAircraftStateText.color = new Color32(255, 0, 0, 255);
            gameManager.scoutmode = false; // �ر����ģʽ�߹���ʾ
        }
        else if (!Roundaction)
        {
            OpenPlayerNotification();
            PlayerNotificationText.text = "Attack aircraft are currently unavailable";
            gameManager.AttackSwitch();
            Invoke("ClosePlayerNotification", 1.0f);
        }
        else if (Scoutaction>0)
        {
            OpenPlayerNotification();
            gameManager.ScoutSwitch();
            PlayerNotificationText.text = "Patrol aircraft are currently unavailable";
            Invoke("ClosePlayerNotification", 1.0f);
        }
    }
    public void UpdatePlaneActions()
    {
        Roundaction = true;
        if (Scoutaction != 0)
        {
            Scoutaction++;
            // ����Ѳ�߻���Ϣ�ı�����ʾ��ȴ����ʱ
            int cooldownTurnsLeft = ScoutTime - Scoutaction;
            PatrolAircraftInfoText.text = $"Waitting {cooldownTurnsLeft} rounds";
 
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
