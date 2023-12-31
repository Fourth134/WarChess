using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane : MonoBehaviour
{
    public float DistanceX;
    public float DistanceZ;

    public bool sea;

    public Vector3 thisObjectPosition;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Transform thisObjectTransform = transform;
        thisObjectPosition = thisObjectTransform.position;// ��ȡ��ǰƽ̨����ĵ�ǰλ�ã��������꣩
    }

    // Update is called once per frame
    void Update()
    {
        if (sea)
        {
            if (gameManager.set)
            {
                Transform currentObjectTransform = gameManager.CurrentObject.transform;
                Vector3 currentPosition = currentObjectTransform.position;// ��ȡ��ǰ��λ����ĵ�ǰλ�ã��������꣩
                DistanceX = currentPosition.x - thisObjectPosition.x;
                DistanceZ = currentPosition.z - thisObjectPosition.z;
                if (DistanceX < 5 && DistanceZ < 5)
                {
                    if (DistanceX > -5 && DistanceZ > -5)
                    {
                        gameManager.CurrentObject.transform.position = thisObjectPosition;
                        gameManager.set = false;
                    }
                }
            }
        }    
    }
}
