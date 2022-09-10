using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 해당 명령이 포함된 스크립트를 게임 오브젝트에 컴포넌트로 적용하면
// 해당 컴포넌트도 같이 추가된다.
[RequireComponent(typeof (CharacterController))]
public class MovementCharactorController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed; // 이동속도
    private Vector3 moveForce; // 이동 힘

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
        // 초당 moveForce 속력으로 이동
        characterController.Move(moveForce * Time.deltaTime);
    }

    public void MoveTo(Vector3 direction)
    {
        // 이동 방향= 캐릭터 회전값 * 방향값
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);

        // 이동 힘= 이동방향 * 속도
        moveForce = new Vector3(direction.x * moveSpeed, moveForce.y, direction.z * moveSpeed);

    }
}
