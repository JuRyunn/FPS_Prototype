using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input keyCodes")]
    [SerializeField]
    private KeyCode keyCodeRun = KeyCode.LeftShift; // �޸���

    [SerializeField]
    private KeyCode keyCodeJump = KeyCode.Space; // ����

    [Header("Audio Clips")]
    
    [SerializeField]
    private AudioClip audioClipWalk; // �ȱ� ����

    [SerializeField]
    private AudioClip audioClipRun; // �ٱ� ����

    private RotateToMouse rotateToMouse; // ���콺 �̵����� ī�޶� ȸ��
    private MovementCharactorController movement; // Ű���� �Է����� ĳ���� �̵�
    private Status status; // �̵��ӵ� ���� ĳ���� ����
    private PlayerAnimatorController animator; // �ִϸ��̼� ��� ����
    private AudioSource audioSource; // ���� ��� ����

    private void Awake()
    {
        // ���콺 Ŀ���� ������ �ʰ� ����
        // ���� ��ġ�� ����
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

        // �̵��� �� ��� �ȱ� or �ٱ�
        if (x !=0 || z!= 0)
        {
            bool isRun = false;

            // �� �ڷ� �̵��� ��� �޸� �� ����
            if (z > 0) isRun = Input.GetKey(keyCodeRun);
            
            movement.MoveSpeed= isRun == true ? status.RunSpeed : status.WalkSpeed;
            animator.MoveSpeed = isRun == true ? 1 : 0.5f;
            audioSource.clip = isRun == true ? audioClipRun : audioClipWalk;

            // ����߿� �ٽ� ������� �ʵ��� isPlaying���� üũ
            if (audioSource.isPlaying == false)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        // �������� ���
        else
        {
            movement.MoveSpeed = 0;
            animator.MoveSpeed = 0;

            // ���� ��� ���尡 ������̸� ����
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
