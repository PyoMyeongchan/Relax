using UnityEngine;

/// <summary>
/// 격자 셀 상태 관리 (채우기/비우기/체크)
/// </summary>
public class GridStateManager : MonoBehaviour
{
    private GridSystem gridSystem;
    private int totalCells;
    private int filledCells;

    public int TotalCells => totalCells;
    public int FilledCells => filledCells;

    private void Awake()
    {
        gridSystem = GetComponent<GridSystem>();
        if (gridSystem == null)
        {
            Debug.LogError("GridSystem not found on the same GameObject!");
        }
    }

    /// <summary>
    /// 격자 초기화 (전체 셀 개수 설정)
    /// </summary>
    public void Initialize(int width, int height)
    {
        totalCells = width * height;
        filledCells = 0;
    }

    /// <summary>
    /// 특정 위치가 비어있는지 확인
    /// </summary>
    public bool IsCellEmpty(int x, int z)
    {
        GridCell cell = gridSystem.GetCell(x, z);
        if (cell != null)
        {
            return !cell.IsFilled;
        }
        return false;
    }

    /// <summary>
    /// 셀을 채움 상태로 변경
    /// </summary>
    public void FillCell(int x, int z, Color color = default)
    {
        GridCell cell = gridSystem.GetCell(x, z);
        if (cell != null && !cell.IsFilled)
        {
            cell.SetFilled(true, color);
            filledCells++;
        }
    }

    /// <summary>
    /// 셀을 빈 상태로 변경
    /// </summary>
    public void EmptyCell(int x, int z)
    {
        GridCell cell = gridSystem.GetCell(x, z);
        if (cell != null && cell.IsFilled)
        {
            cell.SetFilled(false);
            filledCells--;
        }
    }

    /// <summary>
    /// 블록을 격자에 배치 (여러 셀 채우기)
    /// </summary>
    public void PlaceBlock(Vector2Int gridPos, bool[,] blockShape, Color color = default)
    {
        int blockWidth = blockShape.GetLength(0);
        int blockHeight = blockShape.GetLength(1);

        for (int x = 0; x < blockWidth; x++)
        {
            for (int z = 0; z < blockHeight; z++)
            {
                if (blockShape[x, z])
                {
                    FillCell(gridPos.x + x, gridPos.y + z, color);
                }
            }
        }
    }

    /// <summary>
    /// 블록을 격자에서 제거 (여러 셀 비우기)
    /// </summary>
    public void RemoveBlock(Vector2Int gridPos, bool[,] blockShape)
    {
        int blockWidth = blockShape.GetLength(0);
        int blockHeight = blockShape.GetLength(1);

        for (int x = 0; x < blockWidth; x++)
        {
            for (int z = 0; z < blockHeight; z++)
            {
                if (blockShape[x, z])
                {
                    EmptyCell(gridPos.x + x, gridPos.y + z);
                }
            }
        }
    }

    /// <summary>
    /// 격자가 완전히 채워졌는지 확인
    /// </summary>
    public bool IsGridFull()
    {
        return filledCells >= totalCells;
    }

    /// <summary>
    /// 격자 초기화 (모든 셀 비우기)
    /// </summary>
    public void ResetAllCells()
    {
        for (int x = 0; x < gridSystem.GridWidth; x++)
        {
            for (int z = 0; z < gridSystem.GridHeight; z++)
            {
                GridCell cell = gridSystem.GetCell(x, z);
                if (cell != null && cell.IsFilled)
                {
                    cell.SetFilled(false);
                }
            }
        }
        filledCells = 0;
    }

    /// <summary>
    /// 채워진 비율 가져오기 (0.0 ~ 1.0)
    /// </summary>
    public float GetFillPercentage()
    {
        if (totalCells == 0) return 0f;
        return (float)filledCells / totalCells;
    }
}