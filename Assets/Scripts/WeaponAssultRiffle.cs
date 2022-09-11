using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAssultRiffle : MonoBehaviour
{
    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect; // 총구 이펙트 

    [Header("Spawn Points")]
    [SerializeField]
    private Transform casingSpawnPoint; // 탄피 생성 위치

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipTakeOutWeapon; // 무기장착

    [SerializeField]
    private AudioClip audioClipFire; // 공격소리
    

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting weaponSetting; // 무기설정
    private float lastAttackTime = 0; // 마지막 발사시간 체크

    private AudioSource audioSource;
    private PlayerAnimatorController animator;
    private ChasingMemoryPool casingMemoryPool; // 탄피 생성 후 활성,비활성 관리

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInParent<PlayerAnimatorController>();
        casingMemoryPool = GetComponent<ChasingMemoryPool>();
    }

    private void OnEnable()
    {
        // 무기장착 사운드 재생
        PlaySound(audioClipTakeOutWeapon);
        muzzleFlashEffect.SetActive(false);
    }

    public void StartWeaponAction(int type= 0)
    {
        // 공격시작: 마우스 왼쪽버튼
        if(type == 0)
        {

            // 연속공격
            if(weaponSetting.isAutomaticAttack == true)
            {
                StartCoroutine("OnAttackLoop");
            }

            // 단발공격
            else
            {
                OnAttack();
            }
        }
    }

    public void StopWeaponAction(int type= 0)
    {

        // 공격종료: 마우스 왼쪽 버튼
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

            // 뛰는 경우는 공격 x
            if(animator.MoveSpeed > 0.5f)
            {
                return;
            }

            lastAttackTime = Time.time;

            animator.Play("Fire", -1, 0); // 무기 애니메이션 재생
            StartCoroutine("OnMuzzleFlashEffect"); // 총구 이펙트 재생
            PlaySound(audioClipFire); // 공격소리 재생
            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop(); // 기존에 재생중인 사운드 중지
        audioSource.clip = clip; // 새로운 clip 교체
        audioSource.Play(); // 재생
    }

    private IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);
        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);
        muzzleFlashEffect.SetActive(false);
    }
}
