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

    public void AddScoutInfo(string info)// 调用这个方法来添加侦察信息到ScrollView
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
    public void AddAttackInfo(string info)// 调用这个方法来添加攻击信息到ScrollView
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

}


