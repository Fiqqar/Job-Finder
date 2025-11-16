using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{

    [Header("Spawner")]
    public CollectibleSpawner collectibleSpawner;
    public EnemySpawner enemySpawner;

    [Header("Player Setup")]
    public Transform player;

    [Header("Tilemap Setup")]
    public GameObject gridPrefab;
    public TileBase floorTile;
    public TileBase wallTile;

    private Tilemap floorTilemap;
    private Tilemap wallTilemap;

    [Header("Generation Settings")]
    public int walkSteps = 200;
    public Vector2Int startPosition = Vector2Int.zero;
    public int minFloorTiles = 100;
    public int stampSize = 1;
    public int maxGenerationAttempts = 100;

    void Start()
    {
        if (!ValidateSetup()) return;
        int baseJob = 5;
        int baseCoffee = 5;
        GameManager.instance.RegisterCollectibleTargets(baseJob, baseCoffee);
        GenerateNewStage();
    }

    private bool ValidateSetup()
    {
        bool valid = true;
        if (gridPrefab == null) { Debug.LogError("Grid Prefab not assigned!"); valid = false; }
        if (floorTile == null) { Debug.LogError("Floor Tile not assigned!"); valid = false; }
        if (wallTile == null) { Debug.LogError("Wall Tile not assigned!"); valid = false; }
        if (player == null) Debug.LogWarning("Player not assigned!");
        return valid;
    }

    public void GenerateNewStage()
    {
        if (!ValidateSetup()) return;

        if (floorTilemap != null)
        {
            Destroy(floorTilemap.transform.parent.gameObject);
        }

        GameObject gridInstance = Instantiate(gridPrefab, Vector3.zero, Quaternion.identity);
        floorTilemap = gridInstance.transform.Find("Floor").GetComponent<Tilemap>();
        wallTilemap = gridInstance.transform.Find("Wall").GetComponent<Tilemap>();

        HashSet<Vector2Int> floorPositions = GenerateFloor();
        GenerateWalls(floorPositions);

        if (player != null)
        {
            Vector2Int spawnPos = GetRandomFloorPosition(floorPositions);
            player.position = new Vector3(spawnPos.x, spawnPos.y, player.position.z);
        }

        if (enemySpawner != null)
            enemySpawner.SpawnEnemies(floorPositions, Vector2Int.zero);

        if (collectibleSpawner != null)
            collectibleSpawner.SpawnCollectibles(floorPositions, Vector2Int.zero);
    }

    private HashSet<Vector2Int> GenerateFloor()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        Vector2Int pos = startPosition;

        for (int i = 0; i < walkSteps; i++)
        {
            for (int x = -stampSize; x <= stampSize; x++)
                for (int y = -stampSize; y <= stampSize; y++)
                {
                    Vector2Int stampedPos = pos + new Vector2Int(x, y);
                    floorPositions.Add(stampedPos);
                    floorTilemap.SetTile((Vector3Int)stampedPos, floorTile);
                }
            pos += GetRandomDirection();
        }
        return floorPositions;
    }

    private void GenerateWalls(HashSet<Vector2Int> floorPositions)
    {
        wallTilemap.ClearAllTiles();
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

        foreach (var pos in floorPositions)
            foreach (var dir in GetCardinalAndDiagonalDirections())
                if (!floorPositions.Contains(pos + dir))
                    wallPositions.Add(pos + dir);

        foreach (var wp in wallPositions)
            wallTilemap.SetTile((Vector3Int)wp, wallTile);
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

    public HashSet<Vector2Int> GetFloorPositions()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        foreach (var pos in floorTilemap.cellBounds.allPositionsWithin)
            if (floorTilemap.HasTile(pos))
                floorPositions.Add((Vector2Int)pos);
        return floorPositions;
    }
    public void ResetLevel()
    {
        if (floorTilemap != null)
            floorTilemap.ClearAllTiles();
        if (wallTilemap != null)
            wallTilemap.ClearAllTiles();
    }

}
