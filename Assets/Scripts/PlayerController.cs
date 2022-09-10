using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input keyCodes")]
    [SerializeField]
    private KeyCode keyCodeRun = KeyCode.LeftShift; // 달리기

    [SerializeField]
    private KeyCode keyCodeJump = KeyCode.Space; // 점프

    [Header("Audio Clips")]
    
    [SerializeField]
    private AudioClip audioClipWalk; // 걷기 사운드

    [SerializeField]
    private AudioClip audioClipRun; // 뛰기 사운드

    private RotateToMouse rotateToMouse; // 마우스 이동으로 카메라 회전
    private MovementCharactorController movement; // 키보드 입력으로 캐릭터 이동
    private Status status; // 이동속도 등의 캐릭터 정보
    private PlayerAnimatorController animator; // 애니메이션 재생 제어
    private AudioSource audioSource; // 사운드 재생 제어

    private void Awake()
    {
        // 마우스 커서를 보이지 않게 설정
        // 현재 위치에 고정
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotateToMouse = GetComponent<RotateToMouse>();
        movement = GetComponent<MovementCharactorController>();
        status= GetComponent<Status>();
        animator = GetComponent<PlayerAnimatorController>();
        audioSource= GetComponent<AudioSource>();
    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        UpdateJump();
    }

    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 이동중 일 경우 걷기 or 뛰기
        if (x !=0 || z!= 0)
        {
            bool isRun = false;

            // 옆 뒤로 이동할 경우 달릴 수 없음
            if (z > 0) isRun = Input.GetKey(keyCodeRun);
            
            movement.MoveSpeed= isRun == true ? status.RunSpeed : status.WalkSpeed;
            animator.MoveSpeed = isRun == true ? 1 : 0.5f;
            audioSource.clip = isRun == true ? audioClipRun : audioClipWalk;

            // 재생중엔 다시 재생하지 않도록 isPlaying으로 체크
            if (audioSource.isPlaying == false)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        // 멈춰있을 경우
        else
        {
            movement.MoveSpeed = 0;
            animator.MoveSpeed = 0;

            // 멈춘 경우 사운드가 재생중이면 정지
            if (audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }
        }

        movement.MoveTo(new Vector3(x, 0, z));
    }

    private void UpdateJump()
    {
        if (Input.GetKeyDown(keyCodeJump))
        {
            movement.Jump();
        }
    }
}
