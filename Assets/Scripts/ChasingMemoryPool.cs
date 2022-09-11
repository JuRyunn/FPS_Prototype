using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject casingPrefabs; // ź��
    private MemoryPool memoryPool; // �޸�Ǯ

    private void Awake()
    {
        // casingPrefabs: ���� ����, �����ϴ� ������Ʈ
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
