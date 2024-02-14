using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class view : MonoBehaviour
{
    public Transform cameraTransform;
    private Transform m_Transform;
    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        MoveControl();
        if (Input.GetMouseButton(1))
        {
            RotateView();
        }
    }

    private void RotateView()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");//Read mouse horizontal axis
        float angleX = mouseX * 2;
        transform.Rotate(Vector3.up, angleX);

        /*float mouseY = Input.GetAxisRaw("Mouse Y");//Read mouse vertical axis
        float angleY = -mouseY * 2;
        float targetAngle = cameraTransform.localEulerAngles.x + angleY;//Limit rotation angle

        if (targetAngle > 60 && targetAngle < 330)
            return;
        cameraTransform.Rotate(Vector3.right, angleY);*/
    }

    void MoveControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //键盘控制物品前后左右移动，调用函数Translate
            m_Transform.Translate(Vector3.forward * 0.05f, Space.Self);
        }

        if (Input.GetKey(KeyCode.S))
        {
            m_Transform.Translate(Vector3.back * 0.05f, Space.Self);
        }

        if (Input.GetKey(KeyCode.A))
        {
            m_Transform.Translate(Vector3.left * 0.05f, Space.Self);
        }

        if (Input.GetKey(KeyCode.D))
        {
            m_Transform.Translate(Vector3.right * 0.05f, Space.Self);
        }
        if (Input.GetAxis("Mouse ScrollWheel")!=0)
        {
            //键盘控制物品前后左右移动，调用函数Translate
            m_Transform.Translate(Vector3.up * -10 * Input.GetAxis("Mouse ScrollWheel"), Space.Self);
        }

        if (Input.GetKey(KeyCode.Q))
        {

            m_Transform.Rotate(Vector3.up, -0.2f);
        }

        if (Input.GetKey(KeyCode.E))
        {
            m_Transform.Rotate(Vector3.up, 0.2f);
        }
        /*
        if (Input.GetKey(KeyCode.Space))
        {
            //键盘控制物品上下移动，调用函数Translate
            m_Transform.Translate(Vector3.up * 0.1f, Space.Self);
        }

        if (Input.GetKey(KeyCode.X))
        {
            m_Transform.Translate(Vector3.down * 0.1f, Space.Self);
        }*/

    }
}
