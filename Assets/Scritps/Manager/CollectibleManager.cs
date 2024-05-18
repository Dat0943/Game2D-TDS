using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollectibleItem
{
    [Range(0f, 1f)] public float spawnRate;
    public int amount;
    public Collectible collectiblePrefab;
}

public class CollectibleManager : Singleton<CollectibleManager>
{
    [SerializeField] private CollectibleItem[] items;
    
    public void Spawn(Vector3 position)
    {
        if (items == null || items.Length <= 0) return;

        float spawnRateCheck = Random.value;

        for(int i = 0; i < items.Length; i++)
        {
            var item = items[i];

            if (item == null || item.spawnRate < spawnRateCheck) continue;

            CreateCollectible(position, item);
        }
    }

    void CreateCollectible(Vector3 spawnPosition, CollectibleItem collectibleItem)
    {
        if (collectibleItem.collectiblePrefab == null) return;

        for (int i = 0; i < collectibleItem.amount; i++)
        {
            Instantiate(collectibleItem.collectiblePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
