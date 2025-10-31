using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Header("Player Setup")]
    public Transform player;
    [Header("Spawner References")]
    public EnemySpawner enemySpawner;
    public CollectibleSpawner collectibleSpawner;
    [Header("Tilemap Setup")]
    public GameObject gridPrefab;
    public TileBase floorTile;
    public TileBase wallTile;

    private Tilemap floorTilemap;
    private Tilemap wallTilemap;

    [Header("Generation Settings")]
    [Range(50, 500)]
    public int walkSteps = 200;
    public Vector2Int startPosition = Vector2Int.zero;
    [Range(50, 500)]
    public int minFloorTiles = 100;
    [Range(0, 3)]
    public int stampSize = 1;
    [Range(10, 200)]
    public int maxGenerationAttempts = 100;

    void Start()
    {
        if (!ValidateSetup()) return;
        GenerateLevelWithRetries();
    }

    private bool ValidateSetup()
    {
        bool isValid = true;
        if (gridPrefab == null) { Debug.LogError("Grid Prefab not assigned!"); isValid = false; }
        if (floorTile == null) { Debug.LogError("Floor Tile not assigned!"); isValid = false; }
        if (wallTile == null) { Debug.LogError("Wall Tile not assigned!"); isValid = false; }
        if (player == null) { Debug.LogWarning("Player not assigned in hierarchy!"); }
        return isValid;
    }

    void GenerateLevelWithRetries()
    {
        int attempts = 0;
        while (attempts < maxGenerationAttempts)
        {
            if (floorTilemap != null) Destroy(floorTilemap.transform.parent.gameObject);

            GameObject gridInstance = Instantiate(gridPrefab, Vector3.zero, Quaternion.identity);
            floorTilemap = gridInstance.transform.Find("Floor").GetComponent<Tilemap>();
            wallTilemap = gridInstance.transform.Find("Wall").GetComponent<Tilemap>();

            if (floorTilemap == null || wallTilemap == null)
            {
                Debug.LogError("Could not find 'Floor' or 'Wall' Tilemaps!");
                return;
            }

            HashSet<Vector2Int> floorPositions = GenerateFloor();
            GenerateWalls(floorPositions);

            if (floorPositions.Count >= minFloorTiles)
            {
                Debug.Log($"Level generated! Floor tiles: {floorPositions.Count}");

                // Pindah player di hierarchy ke posisi random floor
                if (player != null)
                {
                    Vector2Int spawnPos = GetRandomFloorPosition(floorPositions);
                    player.position = new Vector3(spawnPos.x, spawnPos.y, player.position.z);
                }

                // Spawn enemies & collectibles
                if (enemySpawner != null)
                    enemySpawner.SpawnEnemies(floorPositions, Vector2Int.zero);
                if (collectibleSpawner != null)
                    collectibleSpawner.SpawnCollectibles(floorPositions, Vector2Int.zero);

                return;
            }
            else
            {
                Debug.Log($"Generated level too small ({floorPositions.Count} tiles). Retrying...");
                attempts++;
            }
        }

        Debug.LogError($"Failed to generate a valid level after {maxGenerationAttempts} attempts.");
    }

    private HashSet<Vector2Int> GenerateFloor()
    {
        Vector2Int currentPos = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < walkSteps; i++)
        {
            for (int x = -stampSize; x <= stampSize; x++)
            {
                for (int y = -stampSize; y <= stampSize; y++)
                {
                    Vector2Int stampedPos = currentPos + new Vector2Int(x, y);
                    floorPositions.Add(stampedPos);
                    floorTilemap.SetTile((Vector3Int)stampedPos, floorTile);
                }
            }
            currentPos += GetRandomDirection();
        }

        return floorPositions;
    }

    private void GenerateWalls(HashSet<Vector2Int> floorPositions)
    {
        wallTilemap.ClearAllTiles();
        HashSet<Vector2Int> wallCandidatePositions = new HashSet<Vector2Int>();

        foreach (var pos in floorPositions)
            foreach (var dir in GetCardinalAndDiagonalDirections())
                if (!floorPositions.Contains(pos + dir))
                    wallCandidatePositions.Add(pos + dir);

        foreach (var wallPos in wallCandidatePositions)
            wallTilemap.SetTile((Vector3Int)wallPos, wallTile);

        Debug.Log($"Walls placed: {wallCandidatePositions.Count}");
    }

    private Vector2Int GetRandomDirection()
    {
        switch (Random.Range(0, 4))
        {
            case 0: return Vector2Int.up;
            case 1: return Vector2Int.down;
            case 2: return Vector2Int.left;
            case 3: return Vector2Int.right;
        }
        return Vector2Int.zero;
    }

    private List<Vector2Int> GetCardinalAndDiagonalDirections()
    {
        return new List<Vector2Int>
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
            new Vector2Int(1,1), new Vector2Int(1,-1), new Vector2Int(-1,1), new Vector2Int(-1,-1)
        };
    }

    private Vector2Int GetRandomFloorPosition(HashSet<Vector2Int> floorPositions)
    {
        int index = Random.Range(0, floorPositions.Count);
        foreach (var pos in floorPositions)
            if (index-- == 0) return pos;
        return Vector2Int.zero;
    }
}
