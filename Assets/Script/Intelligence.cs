using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ����UI�����ռ�

public class Intelligence : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject scrollViewContent; // ScrollView��Content����
    public GameObject intelligenceTextPrefab; // �����ϢTextԤ����

    public void AddIntelligenceInfo(string info)// ���������������������Ϣ��ScrollView
    {
        if (intelligenceTextPrefab != null && scrollViewContent != null)
        {
            GameObject newInfoText = Instantiate(intelligenceTextPrefab, scrollViewContent.transform);
            Text textComponent = newInfoText.GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.text = info;
            }
        }
    }

    // ʾ����չʾ�����Ϣ�͵��˵�Ԥ���ƶ�
    public void ShowEnemyInfo(string enemyPosition, string nextEnemyPosition, int turnNum, string tileScouted)
    {
        // ���ݵ����Ƿ񱻷�����������ͬ����Ϣ�ַ���
        string scoutResult = enemyPosition == tileScouted ? "�����˵���" : "��δ���ֵ���";
        // ����Ҫ��ʾ����Ϣ�ַ����������غ������������������
        string info = $"�ڵ�{turnNum}�غ�\n��{tileScouted}�ɳ���������{scoutResult}��";

        // ��������˵��ˣ�׷�ӵ����»غ�Ԥ�Ƶ��ƶ�Ŀ������
        if (enemyPosition == tileScouted)
        {
            info += $"\n�����»غ��ƶ���Ŀ������{nextEnemyPosition}��";
        }

        // ���÷�������Ϣ��ӵ�ScrollView��
        AddIntelligenceInfo(info);
    }
}


