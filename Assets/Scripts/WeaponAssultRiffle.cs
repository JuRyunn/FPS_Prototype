using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAssultRiffle : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipTakeOutWeapon; // 무기장착
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        // 무기장착 사운드 재생
        PlaySound(audioClipTakeOutWeapon);
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop(); // 기존에 재생중인 사운드 중지
        audioSource.clip = clip; // 새로운 clip 교체
        audioSource.Play(); // 재생
    }
}
