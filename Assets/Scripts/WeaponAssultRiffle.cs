using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAssultRiffle : MonoBehaviour
{
    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect; // �ѱ� ����Ʈ 

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipTakeOutWeapon; // ��������

    [SerializeField]
    private AudioClip audioClipFire; // ���ݼҸ�
    

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting weaponSetting; // ���⼳��
    private float lastAttackTime = 0; // ������ �߻�ð� üũ

    private AudioSource audioSource;
    private PlayerAnimatorController animator;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInParent<PlayerAnimatorController>();
    }

    private void OnEnable()
    {
        // �������� ���� ���
        PlaySound(audioClipTakeOutWeapon);
        muzzleFlashEffect.SetActive(false);
    }

    public void StartWeaponAction(int type= 0)
    {
        // ���ݽ���: ���콺 ���ʹ�ư
        if(type == 0)
        {

            // ���Ӱ���
            if(weaponSetting.isAutomaticAttack == true)
            {
                StartCoroutine("OnAttackLoop");
            }

            // �ܹ߰���
            else
            {
                OnAttack();
            }
        }
    }

    public void StopWeaponAction(int type= 0)
    {

        // ��������: ���콺 ���� ��ư
        if(type == 0)
        {
            StopCoroutine("OnAttackLoop");
        }
    }

    private IEnumerator OnAttackLoop()
    {
        while (true)
        {
            OnAttack();
            yield return null;  

        }
    }

    public void OnAttack()
    {
        if(Time.time - lastAttackTime > weaponSetting.attackRate)
        {

            // �ٴ� ���� ���� x
            if(animator.MoveSpeed > 0.5f)
            {
                return;
            }

            lastAttackTime = Time.time;

            animator.Play("Fire", -1, 0); // ���� �ִϸ��̼� ���
            StartCoroutine("OnMuzzleFlashEffect"); // �ѱ� ����Ʈ ���
            PlaySound(audioClipFire); // ���ݼҸ� ���
        }
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop(); // ������ ������� ���� ����
        audioSource.clip = clip; // ���ο� clip ��ü
        audioSource.Play(); // ���
    }

    private IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);
        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);
        muzzleFlashEffect.SetActive(false);
    }
}
