using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool : MonoBehaviour
{
    private class PoolItem
    {
        public bool isActive; // ���ӿ�����Ʈ Ȱ��ȭ/��Ȱ��ȭ
        public GameObject gameObject; 
    }

    private int increaseCount = 5;
    private int maxCount; // ����Ʈ�� ��ϵ� ������Ʈ ����
    private int activeCount; // ���ӿ� Ȱ��ȭ�� ������Ʈ ����

    private GameObject poolObject; // ������Ʈ �������� �����ϴ� ���� ������Ʈ ������
    private List<PoolItem> poolItemList; // ��� ������Ʈ�� �����ϴ� ����Ʈ

    public int MaxCOunt => maxCount; // �ܺο��� ����Ʈ�� ��ϵ� ������Ʈ ���� Ȯ��
    public int ActiveCount => activeCount; // �ܺο��� Ȱ��ȭ�� ������Ʈ ���� Ȯ��

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
