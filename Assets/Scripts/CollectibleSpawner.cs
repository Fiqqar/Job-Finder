using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CollectibleSpawner : MonoBehaviour
{
    [System.Serializable]
    public class CollectibleEntry
    {
        public string collectibleType;
        public GameObject prefab;
    }

    [Header("Collectible Settings")]
    public List<CollectibleEntry> collectibles = new List<CollectibleEntry>();

    public void SpawnCollectibles(HashSet<Vector2Int> floorPositions, Vector2Int playerStartPosition)
    {
        if (GameManager.instance == null) return;

        List<Vector2Int> spawnPoints = floorPositions.ToList();
        spawnPoints.Remove(playerStartPosition);

        int coffeeTarget = GameManager.instance.totalCoffee;
        int jobTarget = GameManager.instance.totalJob;

        foreach (var collectible in collectibles)
        {
            Debug.Log($"SpawnCollectibles called: job={jobTarget}, coffee={coffeeTarget}, floorPositions={floorPositions.Count}");
            Debug.Log($"coffeeTarget={coffeeTarget}, jobTarget={jobTarget}, spawnPoints={spawnPoints.Count}");

            if (collectible.prefab == null) continue;

            int spawnCount = 0;

            if (collectible.collectibleType.ToLower() == "coffee")
                spawnCount = coffeeTarget;
            else if (collectible.collectibleType.ToLower() == "job")
                spawnCount = jobTarget;

            spawnCount = Mathf.Min(spawnCount, spawnPoints.Count);

            for (int i = 0; i < spawnCount; i++)
            {
                int randomIndex = Random.Range(0, spawnPoints.Count);
                Vector2Int spawnTile = spawnPoints[randomIndex];
                Vector3 spawnWorldPos = new Vector3(spawnTile.x + 0.5f, spawnTile.y + 0.5f, 0);

                Instantiate(collectible.prefab, spawnWorldPos, Quaternion.identity, transform);
                spawnPoints.RemoveAt(randomIndex);
                if (spawnPoints.Count == 0) break;
            }
        }
    }

    public void ClearAllCollectibles()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }
}
