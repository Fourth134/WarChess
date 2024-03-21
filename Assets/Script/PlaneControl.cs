using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneControl : MonoBehaviour
{
    public GameManager gameManager;
    public Intelligence intelligence;

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

        if (gameManager.attackmode && gameManager.Roundaction)
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
            gameManager.Roundaction = false;
            gameManager.attackmode = false; // �رչ���ģʽ�߹���ʾ
        }
        else if (gameManager.scoutmode && gameManager.Scoutaction == 0)
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
            gameManager.Scoutaction++;
            gameManager.scoutmode = false; // �ر����ģʽ�߹���ʾ
        }
    }
}
