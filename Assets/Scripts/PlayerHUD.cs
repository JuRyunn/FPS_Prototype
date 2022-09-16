using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        SetupWeapon();

        weapon.onAmmoEvent.AddListener(UpdateAmmoHUD);
    }

    private void SetupWeapon()
    {
        textWeaponName.text = weapon.WeaponName.ToString();
        imageWeaponIcon.sprite = spriteWeaponIcons[(int)weapon.WeaponName];
    }
    private  void UpdateAmmoHUD(int currentAmmo, int maxAmmo)
    {
        textAmmo.text = $"<size=40>{currentAmmo}/</size>{maxAmmo}";
    }
}
