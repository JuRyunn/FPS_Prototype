using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ImpactType { Normal= 0, Obstacle, }
public class ImpactMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] impactPrefab; // ««∞› ¿Ã∆—∆Æ
    private MemoryPool[] memorypool;

    private void Awake()
    {
        memorypool= new MemoryPool[impactPrefab.Length];

        for (int i=0; i < impactPrefab.Length; ++i)
        {
            memorypool[i]= new MemoryPool(impactPrefab[i]);
        }
    }

    public void SpawnImpact(RaycastHit hit)
    {
        if (hit.transform.CompareTag("ImpactNormal"))
        {
            OnSpawnImpact(ImpactType.Normal, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if (hit.transform.CompareTag("ImpactObstacle"))
        {
            OnSpawnImpact(ImpactType.Obstacle, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    public void OnSpawnImpact(ImpactType type, Vector3 position, Quaternion rotation)
    {
        GameObject item = memorypool[(int)type].ActiveatePoolItem();

        item.transform.position = position;
        item.transform.rotation= rotation;
        item.GetComponent<Impact>().Setup(memorypool[(int)type]);
    }


}
