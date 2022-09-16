using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerHUD : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private WeaponAssultRiffle weapon;

    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI textWeaponName; // 무기의 이름

    [SerializeField]
    private Image imageWeaponIcon; // 무기 아이콘

    [SerializeField]
    private Sprite[] spriteWeaponIcons; // 무기에 사용되는 sprite

    [Header("Ammo")]
    [SerializeField]
    private TextMeshProUGUI textAmmo;

    [Header("Magazine")]
    [SerializeField]
    private GameObject magazineUIPrefab; // 탄창 Prefab

    [SerializeField]
    private Transform magazineParent; // 탄창 UI 배치되는 Panel

    private List<GameObject> magazineList; // 탄창 UI 리스트

    private void Awake()
    {
        SetupWeapon();
        SetupMagazine();

        weapon.onAmmoEvent.AddListener(UpdateAmmoHUD);
        weapon.onMagazineEvent.AddListener(UpdateMagazineHUD);
    }

    private void SetupWeapon()
    {
        textWeaponName.text = weapon.WeaponName.ToString();
        imageWeaponIcon.sprite = spriteWeaponIcons[(int)weapon.WeaponName];
    }

    private void SetupMagazine()
    {
        // weapon에 등록되어 있는 최대 탄창 갯수만큼 image icon 생성
        // magazineParent 오브젝트의 자식 등록 후 모두 비활성화 / 리스트에 저장
        magazineList= new List<GameObject>();

        for(int i = 0; i < weapon.MaxMagazine; ++i)
        {
            GameObject clone = Instantiate(magazineUIPrefab);
            clone.transform.SetParent(magazineParent);
            clone.SetActive(false);

            magazineList.Add(clone);    
        }

        // weapon에 등록되어 있는 현재 탄창 갯수만큼 오브젝트 활성화
        for (int i = 0; i < weapon.CurrentMagazine; ++i)
        {
            magazineList[i].SetActive(true);
        }
    }

    private  void UpdateAmmoHUD(int currentAmmo, int maxAmmo)
    {
        textAmmo.text = $"<size=40>{currentAmmo}/</size>{maxAmmo}";
    }

    private void UpdateMagazineHUD(int currentMagazine)
    {
        // 전부 비활성화하고 currentMagazine 갯수만큼 활성화
        for (int i = 0; i < magazineList.Count; ++i)
        {
            magazineList[i].SetActive(false);
        }
        for (int i = 0; i < currentMagazine; ++i)
        {
            magazineList[i].SetActive(true);
        }
    }
}
