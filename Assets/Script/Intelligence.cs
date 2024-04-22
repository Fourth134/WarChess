using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引用UI命名空间

public class Intelligence : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject scrollViewContent; // ScrollView的Content对象
    public GameObject attackScrollViewContent; // 新的攻击信息ScrollView内容对象
    public GameObject intelligenceTextPrefab; // 侦察信息Text预制体

    public void AddIntelligenceInfo(string info)// 调用这个方法来添加侦察信息到ScrollView
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
    public void AddAttackInfo(string info)// 新方法：添加攻击信息到ScrollView
    {
        if (intelligenceTextPrefab != null && attackScrollViewContent != null)
        {
            GameObject newInfoText = Instantiate(intelligenceTextPrefab, attackScrollViewContent.transform);
            Text textComponent = newInfoText.GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.text = info;
            }
        }
    }

    // 示例：展示侦察信息和敌人的预计移动
    public void ShowEnemyInfo(string enemyPosition, string nextEnemyPosition, int turnNum, string tileScouted)
    {
        // 根据敌人是否被发现来构建不同的信息字符串
        string scoutResult = enemyPosition == tileScouted ? "发现了敌人" : "并未发现敌人";
        // 构建要显示的信息字符串，包括回合数、侦察区域和侦察结果
        string info = $"在第{turnNum}回合\n向{tileScouted}派出了侦察机，{scoutResult}。";

        // 如果发现了敌人，追加敌人下回合预计的移动目标区域
        if (enemyPosition == tileScouted)
        {
            info += $"\n敌人下回合移动的目标区域：{nextEnemyPosition}。";
        }

        // 调用方法将信息添加到ScrollView中
        AddIntelligenceInfo(info);
    }
}


