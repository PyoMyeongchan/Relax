using UnityEngine;

/// <summary>
/// 격자 생성 및 관리, 셀 접근, 좌표 변환
/// </summary>
public class GridSystem : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int gridWidth = 5;
    [SerializeField] private int gridHeight = 5;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private float cellSpacing = 0.1f;

    [Header("Prefab")]
    [SerializeField] private GameObject gridCellPrefab;

    private GridCell[,] grid;

    public int GridWidth => gridWidth;
    public int GridHeight => gridHeight;
    public float CellSize => cellSize;
    public float CellSpacing => cellSpacing;

    /// <summary>
    /// 격자 생성
    /// </summary>
    public void CreateGrid(int width, int height)
    {
        gridWidth = width;
        gridHeight = height;

        ClearGrid();

        grid = new GridCell[gridWidth, gridHeight];

        float offsetX = -(gridWidth * (cellSize + cellSpacing)) / 2f + (cellSize + cellSpacing) / 2f;
        float offsetZ = -(gridHeight * (cellSize + cellSpacing)) / 2f + (cellSize + cellSpacing) / 2f;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector3 position = new Vector3(
                    offsetX + x * (cellSize + cellSpacing),
                    0f,
                    offsetZ + z * (cellSize + cellSpacing)
                );

                GameObject cellObj = Instantiate(gridCellPrefab, position, Quaternion.identity, transform);
                cellObj.name = $"Cell_{x}_{z}";

                GridCell cell = cellObj.GetComponent<GridCell>();
                if (cell != null)
                {
                    cell.Initialize(x, z, cellSize);
                    grid[x, z] = cell;
                }
                else
                {
                    Debug.LogError("GridCell component not found on prefab!");
                }
            }
        }

        Debug.Log($"Grid created: {gridWidth}x{gridHeight}");
    }

    /// <summary>
    /// 특정 위치의 셀 가져오기
    /// </summary>
    public GridCell GetCell(int x, int z)
    {
        if (IsValidPosition(x, z))
        {
            return grid[x, z];
        }
        return null;
    }

    /// <summary>
    /// 격자 좌표가 유효한지 확인
    /// </summary>
    public bool IsValidPosition(int x, int z)
    {
        return x >= 0 && x < gridWidth && z >= 0 && z < gridHeight;
    }

    /// <summary>
    /// 월드 좌표를 격자 좌표로 변환
    /// </summary>
    public Vector2Int WorldToGridPosition(Vector3 worldPos)
    {
        float offsetX = -(gridWidth * (cellSize + cellSpacing)) / 2f + (cellSize + cellSpacing) / 2f;
        float offsetZ = -(gridHeight * (cellSize + cellSpacing)) / 2f + (cellSize + cellSpacing) / 2f;

        int x = Mathf.RoundToInt((worldPos.x - offsetX) / (cellSize + cellSpacing));
        int z = Mathf.RoundToInt((worldPos.z - offsetZ) / (cellSize + cellSpacing));

        return new Vector2Int(x, z);
    }

    /// <summary>
    /// 격자 좌표를 월드 좌표로 변환
    /// </summary>
    public Vector3 GridToWorldPosition(int x, int z)
    {
        if (IsValidPosition(x, z))
        {
            return grid[x, z].transform.position;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// 격자 완전히 제거
    /// </summary>
    public void ClearGrid()
    {
        if (grid != null)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int z = 0; z < grid.GetLength(1); z++)
                {
                    if (grid[x, z] != null)
                    {
                        Destroy(grid[x, z].gameObject);
                    }
                }
            }
        }
        grid = null;
    }

    private void OnDestroy()
    {
        ClearGrid();
    }

    // 디버그용: 격자 시각화
    private void OnDrawGizmos()
    {
        if (grid == null) return;

        Gizmos.color = Color.green;
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                if (grid[x, z] != null)
                {
                    Vector3 pos = grid[x, z].transform.position;
                    Gizmos.DrawWireCube(pos, Vector3.one * cellSize * 0.9f);
                }
            }
        }
    }
}