using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponSetting
{
    public float attackRate; // 공속
    public float attackDistance; // 사거리
    public bool isAutomaticAttack; // 연속공격
}
