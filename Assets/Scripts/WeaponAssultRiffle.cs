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
    private GameObject muzzleFlashEffect; // �ѱ� ����Ʈ 

    [Header("Spawn Points")]
    [SerializeField]
    private Transform casingSpawnPoint; // ź�� ���� ��ġ

    [SerializeField]
    private Transform bulletSpawnPoint; // �Ѿ� ���� ��ġ

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipTakeOutWeapon; // ��������

    [SerializeField]
    private AudioClip audioClipFire; // ���ݼҸ�

    [SerializeField]
    private AudioClip audioClipReload; // ������ ����

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting weaponSetting; // ���⼳��
    private float lastAttackTime = 0; // ������ �߻�ð� üũ
    private bool isReload = false; // ������ ���� üũ

    private AudioSource audioSource;
    private PlayerAnimatorController animator;
    private ChasingMemoryPool casingMemoryPool; // ź�� ���� �� Ȱ��,��Ȱ�� ����
    private ImpactMemoryPool impactMemoryPool; // ���� ȿ�� ���� �� Ȱ��, ��Ȱ�� ����
    private Camera mainCamera; // ���� �߻�

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

        // ù źâ�� max
        weaponSetting.currentMagazine = weaponSetting.maxMagazine;

        // ù ź ���� max
        weaponSetting.CurrentAmmo = weaponSetting.maxAmmo;
    }

    private void OnEnable()
    {
        // �������� ���� ���
        PlaySound(audioClipTakeOutWeapon);
        muzzleFlashEffect.SetActive(false);

        // ���� Ȱ��ȭ �Ǵ� ��� źâ ����
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);

        // ���Ⱑ Ȱ��ȭ �Ǵ� ��� ź �� ����
        onAmmoEvent.Invoke(weaponSetting.CurrentAmmo, weaponSetting.maxAmmo);
    }

    public void StartWeaponAction(int type= 0)
    {
        // ���� ������ ���̰ų� źâ �� 0�̸� ������ �Ұ���
        if (isReload == true || weaponSetting.currentMagazine <= 0) return;

        // ���ݽ���: ���콺 ���ʹ�ư
        if (type == 0)
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

    public void startReload()
    {
        // ������ �ÿ� ������ �Ұ���
        if (isReload == true) return;
        
        // "R" Ű�� ���� ������
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

            // �ٴ� ���� ���� x
            if(animator.MoveSpeed > 0.5f)
            {
                return;
            }

            lastAttackTime = Time.time;

            // ź���� ������ ���� x
            if (weaponSetting.CurrentAmmo <= 0)
            {
                return;
            }

            // ���ݽ� ź�� -1�� ����
            weaponSetting.CurrentAmmo--;
            onAmmoEvent.Invoke(weaponSetting.CurrentAmmo, weaponSetting.maxAmmo);

            animator.Play("Fire", -1, 0); // ���� �ִϸ��̼� ���
            StartCoroutine("OnMuzzleFlashEffect"); // �ѱ� ����Ʈ ���
            PlaySound(audioClipFire); // ���ݼҸ� ���
            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right);

            // ������ �߻��� ���ϴ� ��ġ ����
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
            // ���� ����� �ƴ� �ִϸ��̼��� movement�� ���
            // ������ �ִϸ��̼� ��� ����
            if(audioSource.isPlaying == false && animator.CurrentAnimatonIs("Movement"))
            {
                isReload = false;

                // ���� źâ �� 1�����԰� ���ÿ� �ٲ� źâ ���� ������Ʈ
                weaponSetting.currentMagazine--;
                onMagazineEvent.Invoke(weaponSetting.currentMagazine);

                // ���� ź �� �ִ� ����
                // �ٲ� ź �� ���� TextMeshPro�� ������Ʈ
                weaponSetting.CurrentAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.CurrentAmmo, weaponSetting.maxAmmo);

                yield break;
            }

            yield return null;
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
