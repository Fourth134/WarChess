using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject CurrentObject;
    public GameObject CurrentSealand;
    public bool set = false;//判断是否触发棋盘中心吸附
    public bool ShowRangHighlight = false;//判断距离高光显示

    public bool RoundMove;

    public float CurrentObjectX;
    public float CurrentObjectZ; //当前控制物体记录的上一次正确坐标

    
    public string[] landProperties;// String数组，用于存储地块的属性

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
