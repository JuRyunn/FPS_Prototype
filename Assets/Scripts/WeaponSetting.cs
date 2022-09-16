using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponName { AssultRifle= 0 }

[System.Serializable]
public struct WeaponSetting
{
    public WeaponName weaponName; // 무기의 이름

    public int currentMagazine; // 현재 탄창 수
    public int maxMagazine; // 최대 탄창 수 
    public int CurrentAmmo; // 현재 탄약수
    public int maxAmmo; // 최대 탄약수

    public float attackRate; // 공속
    public float attackDistance; // 사거리

    public bool isAutomaticAttack; // 연속공격
}
