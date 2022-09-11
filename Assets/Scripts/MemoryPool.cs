using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool : MonoBehaviour
{
    private class PoolItem
    {
        public bool isActive; // 게임오브젝트 활성화/비활성화
        public GameObject gameObject; 
    }

    private int increaseCount = 5;
    private int maxCount; // 리스트에 등록된 오브젝트 갯수
    private int activeCount; // 게임에 활성화된 오브젝트 갯수

    private GameObject poolObject; // 오브젝트 폴링에서 관리하는 게임 오브젝트 프리팹
    private List<PoolItem> poolItemList; // 모든 오브젝트를 저장하는 리스트

    public int MaxCOunt => maxCount; // 외부에서 리스트에 등록된 오브젝트 갯수 확인
    public int ActiveCount => activeCount; // 외부에서 활성화된 오브젝트 갯수 확인

    public MemoryPool(GameObject poolObject)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();

        InstantiateObjects();
    }

    public void InstantiateObjects()
    {
        maxCount += increaseCount;

        for(int i=0; i<increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false;
            poolItem.gameObject = GameObject.Instantiate(poolObject);
            poolItem.gameObject.SetActive(false);

            poolItemList.Add(poolItem);
        }
    }

    public void DestroyObjects()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;

        for(int i= 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }

        poolItemList.Clear();   
    }

    public GameObject ActiveatePoolItem()
    {
        if (poolItemList == null) return null;

        if(maxCount == activeCount)
        {
            InstantiateObjects();
        }

        int count = poolItemList.Count;

        for(int i= 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.isActive == false)
            {
                activeCount++;

                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }
        }

        return null;
    }

    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null) return;

        int count = poolItemList.Count;

        for(int i=0; i < count; ++i)
        {
            PoolItem poolItem= poolItemList[i];

            if(poolItem.gameObject == removeObject)
            {
                activeCount--;

                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

                return;
            }
        }
    }

    public void DeactiveateAllPoolItems()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;

        for(int i=0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }

        activeCount = 0;
    }


}
