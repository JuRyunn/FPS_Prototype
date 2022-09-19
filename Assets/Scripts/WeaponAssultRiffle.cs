using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }

[System.Serializable]
public class MagazineEvent : UnityEngine.Events.UnityEvent<int> { }

public class WeaponAssultRiffle : MonoBehaviour
{
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();

    [HideInInspector]
    public MagazineEvent onMagazineEvent = new MagazineEvent();

    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect; // 총구 이펙트 

    [Header("Spawn Points")]
    [SerializeField]
    private Transform casingSpawnPoint; // 탄피 생성 위치

    [SerializeField]
    private Transform bulletSpawnPoint; // 총알 생성 위치

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipTakeOutWeapon; // 무기장착

    [SerializeField]
    private AudioClip audioClipFire; // 공격소리

    [SerializeField]
    private AudioClip audioClipReload; // 재장전 사운드

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting weaponSetting; // 무기설정
    private float lastAttackTime = 0; // 마지막 발사시간 체크
    private bool isReload = false; // 재장전 여부 체크

    private AudioSource audioSource;
    private PlayerAnimatorController animator;
    private ChasingMemoryPool casingMemoryPool; // 탄피 생성 후 활성,비활성 관리
    private ImpactMemoryPool impactMemoryPool; // 공격 효과 생성 후 활성, 비활성 관리
    private Camera mainCamera; // 광선 발사

    public WeaponName WeaponName => weaponSetting.weaponName;
    public int CurrentMagazine => weaponSetting.currentMagazine;
    public int MaxMagazine => weaponSetting.maxMagazine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInParent<PlayerAnimatorController>();
        casingMemoryPool = GetComponent<ChasingMemoryPool>();
        impactMemoryPool= GetComponent<ImpactMemoryPool>(); 
        
        mainCamera= Camera.main;

        // 첫 탄창은 max
        weaponSetting.currentMagazine = weaponSetting.maxMagazine;

        // 첫 탄 수는 max
        weaponSetting.CurrentAmmo = weaponSetting.maxAmmo;
    }

    private void OnEnable()
    {
        // 무기장착 사운드 재생
        PlaySound(audioClipTakeOutWeapon);
        muzzleFlashEffect.SetActive(false);

        // 무기 활성화 되는 경우 탄창 갱신
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);

        // 무기가 활성화 되는 경우 탄 수 갱신
        onAmmoEvent.Invoke(weaponSetting.CurrentAmmo, weaponSetting.maxAmmo);
    }

    public void StartWeaponAction(int type= 0)
    {
        // 현재 재장전 중이거나 탄창 수 0이면 재장전 불가능
        if (isReload == true || weaponSetting.currentMagazine <= 0) return;

        // 공격시작: 마우스 왼쪽버튼
        if (type == 0)
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

    public void startReload()
    {
        // 재장전 시엔 재장전 불가능
        if (isReload == true) return;
        
        // "R" 키를 통해 재장전
        StopWeaponAction();
        StartCoroutine("OnReload");
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

            // 탄수가 없으면 공격 x
            if (weaponSetting.CurrentAmmo <= 0)
            {
                return;
            }

            // 공격시 탄수 -1씩 감소
            weaponSetting.CurrentAmmo--;
            onAmmoEvent.Invoke(weaponSetting.CurrentAmmo, weaponSetting.maxAmmo);

            animator.Play("Fire", -1, 0); // 무기 애니메이션 재생
            StartCoroutine("OnMuzzleFlashEffect"); // 총구 이펙트 재생
            PlaySound(audioClipFire); // 공격소리 재생
            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right);

            // 광선을 발사해 원하는 위치 공격
            TwoStepRayCast();
        }
    }

    private IEnumerator OnReload()
    {
        isReload = true;

        animator.onReload();
        PlaySound(audioClipReload);

        while (true)
        {
            // 사운드 재생이 아닌 애니메이션이 movement일 경우
            // 재장전 애니메이션 재생 종료
            if(audioSource.isPlaying == false && animator.CurrentAnimatonIs("Movement"))
            {
                isReload = false;

                // 현재 탄창 수 1감소함과 동시에 바뀐 탄창 정보 업데이트
                weaponSetting.currentMagazine--;
                onMagazineEvent.Invoke(weaponSetting.currentMagazine);

                // 현재 탄 수 최대 설정
                // 바뀐 탄 수 정보 TextMeshPro에 업데이트
                weaponSetting.CurrentAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.CurrentAmmo, weaponSetting.maxAmmo);

                yield break;
            }

            yield return null;
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

    private void TwoStepRayCast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);

        if (Physics.Raycast(ray, out hit, weaponSetting.attackDistance))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint= ray.origin + ray.direction * weaponSetting.attackDistance;
        }
        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.attackDistance, Color.red);

        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;

        if (Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponSetting.attackDistance))
        {
            impactMemoryPool.SpawnImpact(hit);
        }
        Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponSetting.attackDistance, Color.blue);
    }
}
