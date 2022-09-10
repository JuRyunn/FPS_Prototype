using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ش� ����� ���Ե� ��ũ��Ʈ�� ���� ������Ʈ�� ������Ʈ�� �����ϸ�
// �ش� ������Ʈ�� ���� �߰��ȴ�.
[RequireComponent(typeof (CharacterController))]
public class MovementCharactorController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed; // �̵��ӵ�
    private Vector3 moveForce; // �̵� ��

    [SerializeField]
    private float jumpForce; // ������

    [SerializeField]
    private float gravity; // �߷� ���

    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;
    }

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // ����� ��� �߷¸�ŭ y�� �̵��ӵ� ����
        if (!characterController.isGrounded)
        {
            moveForce.y += gravity * Time.deltaTime;
        }

        // �ʴ� moveForce �ӷ����� �̵�
        characterController.Move(moveForce * Time.deltaTime);
    }

    public void MoveTo(Vector3 direction)
    {
        // �̵� ����= ĳ���� ȸ���� * ���Ⱚ
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);

        // �̵� ��= �̵����� * �ӵ�
        moveForce = new Vector3(direction.x * moveSpeed, moveForce.y, direction.z * moveSpeed);

    }

    public void Jump()
    {
        // ĳ���Ͱ� �ٴڿ� ���� ��츸 ����
        if (characterController.isGrounded)
        {
            moveForce.y = jumpForce;
        }
    }
}
