using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 블록 생성 및 대기 영역 배치
/// </summary>
public class BlockSpawner : MonoBehaviour
{
    [Header("Prefabs & Materials")]
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject blockCellPrefab;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material selectedMaterial;

    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnParent;
    [SerializeField] private Vector3 spawnStartPosition = new Vector3(-3f, 0f, -8f);
    [SerializeField] private float blockSpacingX = 2.5f;
    [SerializeField] private float blockSpacingZ = 2.5f;
    [SerializeField] private int blocksPerRow = 4;

    private List<BlockObject> spawnedBlocks = new List<BlockObject>();

    public List<BlockObject> SpawnedBlocks => spawnedBlocks;

    /// <summary>
    /// 스테이지 데이터로부터 블록들 생성
    /// </summary>
    /// <param name="blockShapes">블록 목록</param>
    /// <param name="shuffle">true이면 스폰 순서를 무작위로 섞음 (difficulty 4+)</param>
    public void SpawnBlocks(List<BlockShapeData> blockShapes, bool shuffle = false)
    {
        ClearBlocks();

        if (blockShapes == null || blockShapes.Count == 0)
        {
            Debug.LogWarning("No block shapes to spawn!");
            return;
        }

        List<BlockShapeData> ordered = new List<BlockShapeData>(blockShapes);
        if (shuffle)
            Shuffle(ordered);

        for (int i = 0; i < ordered.Count; i++)
        {
            int row = i / blocksPerRow;
            int col = i % blocksPerRow;
            Vector3 spawnPos = spawnStartPosition +
                               new Vector3(col * blockSpacingX, 0f, -row * blockSpacingZ);
            SpawnBlock(ordered[i], spawnPos);
        }

        Debug.Log($"Spawned {spawnedBlocks.Count} blocks (shuffle={shuffle})");
    }

    private static void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    /// <summary>
    /// 개별 블록 생성
    /// </summary>
    private void SpawnBlock(BlockShapeData shapeData, Vector3 position)
    {
        if (shapeData == null)
        {
            Debug.LogError("ShapeData is null!");
            return;
        }

        if (shapeData.shape == null)
            shapeData.UnflattenShape();

        shapeData.LoadColor();

        if (shapeData.shape == null)
        {
            Debug.LogError($"Shape still null after unflatten! Width: {shapeData.width}, Height: {shapeData.height}");
            return;
        }

        GameObject blockObj = new GameObject($"Block_{spawnedBlocks.Count}");
        
        if (spawnParent != null)
        {
            blockObj.transform.SetParent(spawnParent);
        }
        else
        {
            blockObj.transform.SetParent(transform);
        }

        blockObj.transform.position = position;

        BlockObject block = blockObj.AddComponent<BlockObject>();
        
        if (block == null)
        {
            Debug.LogError("Failed to add BlockObject component!");
            return;
        }

        block.Initialize(shapeData.shape, blockCellPrefab, normalMaterial, selectedMaterial, shapeData.blockColor);

        spawnedBlocks.Add(block);
    }

    /// <summary>
    /// 특정 블록만 생성 (테스트용)
    /// </summary>
    public BlockObject SpawnSingleBlock(bool[,] shape, Vector3 position)
    {
        BlockShapeData shapeData = new BlockShapeData { shape = shape };
        SpawnBlock(shapeData, position);
        return spawnedBlocks[spawnedBlocks.Count - 1];
    }

    /// <summary>
    /// 모든 블록 제거
    /// </summary>
    public void ClearBlocks()
    {
        foreach (var block in spawnedBlocks)
        {
            if (block != null)
            {
                Destroy(block.gameObject);
            }
        }
        spawnedBlocks.Clear();
    }

    /// <summary>
    /// 배치되지 않은 블록 개수
    /// </summary>
    public int GetRemainingBlockCount()
    {
        int count = 0;
        foreach (var block in spawnedBlocks)
        {
            if (block != null && !block.IsPlaced)
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// 모든 블록이 배치되었는지 확인
    /// </summary>
    public bool AreAllBlocksPlaced()
    {
        return GetRemainingBlockCount() == 0;
    }

    /// <summary>
    /// 블록 재배치 (스테이지 재시작용)
    /// </summary>
    public void ResetAllBlocks()
    {
        foreach (var block in spawnedBlocks)
        {
            if (block != null)
            {
                block.Reset();
            }
        }
    }

    private void OnDestroy()
    {
        ClearBlocks();
    }

    // 디버그용: 스폰 위치 시각화
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        // 그리드 형태로 표시 (최대 12개, 3줄)
        for (int i = 0; i < 12; i++)
        {
            int row = i / blocksPerRow;
            int col = i % blocksPerRow;
            
            Vector3 pos = spawnStartPosition + 
                          new Vector3(col * blockSpacingX, 0f, -row * blockSpacingZ);
            
            Gizmos.DrawWireCube(pos, Vector3.one * 0.5f);
        }
    }
}