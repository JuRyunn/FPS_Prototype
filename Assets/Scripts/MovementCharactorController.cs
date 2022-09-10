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
}
