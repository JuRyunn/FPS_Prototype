using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casing : MonoBehaviour
{
    [SerializeField]
    private float deactiveateTime = 5.0f; // ź�� ��Ȱ��ȭ �ð�

    [SerializeField]
    private float casingSpin = 1.0f; // ź�� ȸ�� �ӷ°��

    [SerializeField]
    private AudioClip[] audioClips;

    private Rigidbody rigidbody3D;
    private AudioSource audioSource;
    private MemoryPool memoryPool;

    public void Setup(MemoryPool pool, Vector3 direction)
    {
        rigidbody3D = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        memoryPool = pool;

        // ź�� �̵� �� ȸ�� �ӷ� ����
        rigidbody3D.velocity = new Vector3(direction.x, 1.0f, direction.z);
        rigidbody3D.angularVelocity = new Vector3(Random.Range(-casingSpin, casingSpin),
                                                 Random.Range(-casingSpin, casingSpin),
                                                 Random.Range(-casingSpin, casingSpin));

        // ź�� �ڵ� ��Ȱ��ȭ
        StartCoroutine("DeactiveAfterTime");
    }

    private void OnCollisionEnter(Collision collision)
    {
        int index = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    private IEnumerator DeactiveAfterTime()
    {
        yield return new WaitForSeconds(deactiveateTime);

        memoryPool.DeactivatePoolItem(this.gameObject);
    }

}