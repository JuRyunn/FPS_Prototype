using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject casingPrefabs; // 탄피
    private MemoryPool memoryPool; // 메모리풀

    private void Awake()
    {
        // casingPrefabs: 현재 생성, 관리하는 오브젝트
        memoryPool = new MemoryPool(casingPrefabs);
    }

    public void SpawnCasing(Vector3 position, Vector3 direction)
    {
        GameObject item = memoryPool.ActiveatePoolItem();

        item.transform.position = position;
        item.transform.rotation = Random.rotation;
        item.GetComponent<Casing>().Setup(memoryPool, direction);
    }
}
