using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    [SerializeField]
    private float rotCamXAxisSpeed = 5; // x축 회전속도

    [SerializeField]
    private float rotCamYAxisSpeed = 3; // y축 회전속도

    private float limitMinX = -80; // x축 최소 회전 범위
    private float limitMaxX = 50; // x축 최대 회전 범위
    private float eulerAngleX;
    private float eulerAngleY;

    public void UpdateRotate(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed; // 좌우 이동으로 y축 회전
        eulerAngleX -= mouseY * rotCamXAxisSpeed; // 위아래 이동으로 x축 회전

        // x축 회전의 범위 설정
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
