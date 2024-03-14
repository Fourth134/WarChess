using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引用UI命名空间

public class Intelligence : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject scrollViewContent; // ScrollView的Content对象
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

    // 示例：展示侦察信息和敌人的预计移动
    public void ShowEnemyInfo(string enemyPosition, string nextEnemyPosition, int turnNum, string tileScouted)
    {
        string scoutResult = enemyPosition == tileScouted ? "发现了敌人" : "并未发现敌人";
        string info = $"在第{turnNum}回合，向{tileScouted}派出了侦察机，{scoutResult}。\n敌人下回合移动的目标区域：{nextEnemyPosition}。";
        AddIntelligenceInfo(info);
    }
}


