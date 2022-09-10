using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    [SerializeField]
    private float rotCamXAxisSpeed = 5; // x�� ȸ���ӵ�

    [SerializeField]
    private float rotCamYAxisSpeed = 3; // y�� ȸ���ӵ�

    private float limitMinX = -80; // x�� �ּ� ȸ�� ����
    private float limitMaxX = 50; // x�� �ִ� ȸ�� ����
    private float eulerAngleX;
    private float eulerAngleY;

    public void UpdateRotate(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed; // �¿� �̵����� y�� ȸ��
        eulerAngleX -= mouseY * rotCamXAxisSpeed; // ���Ʒ� �̵����� x�� ȸ��

        // x�� ȸ���� ���� ����
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);
        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}
