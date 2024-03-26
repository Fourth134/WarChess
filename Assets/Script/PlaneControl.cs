using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneControl : MonoBehaviour
{
    public GameManager gameManager;
    public Intelligence intelligence;

    public bool Roundaction = true; // ���汾�غ��Ƿ���ɹ�������
    public int Scoutaction = 0; // �����Ƿ���Խ�����죨0������ԣ�
    public int ScoutTime; // ������������׼���Ļغ���

    // ����ͨ��ĳ�ַ�ʽ���õ�ǰѡ�е�ChessTile������ͨ�������
    public ChessTile currentTile;

    // ���ø÷�����ִ�й���������߼�
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
            gameManager.scoutmode = false; // �ر����ģʽ�߹���ʾ
        }
    }
    public void UpdatePlaneActions()
    {
        Roundaction = true;
        if (Scoutaction != 0)
        {
            Scoutaction++;
        }
        if (Scoutaction > ScoutTime)
        {
            Scoutaction = 0;
            Debug.Log("�����Ѿ�����");
        }
    }
}
