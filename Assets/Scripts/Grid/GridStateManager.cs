using UnityEngine;

/// <summary>
/// Tracks which grid cells are filled and detects grid completion.
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
            Debug.LogError("GridSystem not found on the same GameObject!");
    }

    /// <summary>
    /// Initialize with the total number of cells for the current stage.
    /// </summary>
    public void Initialize(int width, int height)
    {
        totalCells = width * height;
        filledCells = 0;
    }

    public bool IsCellEmpty(int x, int z)
    {
        GridCell cell = gridSystem.GetCell(x, z);
        return cell != null && !cell.IsFilled;
    }

    public void FillCell(int x, int z, Color color = default)
    {
        GridCell cell = gridSystem.GetCell(x, z);
        if (cell != null && !cell.IsFilled)
        {
            cell.SetFilled(true, color);
            filledCells++;
        }
    }

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
    /// Place a block — fills all cells covered by the shape.
    /// </summary>
    public void PlaceBlock(Vector2Int gridPos, bool[,] blockShape, Color color = default)
    {
        int blockWidth = blockShape.GetLength(0);
        int blockHeight = blockShape.GetLength(1);

        for (int x = 0; x < blockWidth; x++)
            for (int z = 0; z < blockHeight; z++)
                if (blockShape[x, z])
                    FillCell(gridPos.x + x, gridPos.y + z, color);
    }

    /// <summary>
    /// Remove a block — empties all cells covered by the shape.
    /// </summary>
    public void RemoveBlock(Vector2Int gridPos, bool[,] blockShape)
    {
        int blockWidth = blockShape.GetLength(0);
        int blockHeight = blockShape.GetLength(1);

        for (int x = 0; x < blockWidth; x++)
            for (int z = 0; z < blockHeight; z++)
                if (blockShape[x, z])
                    EmptyCell(gridPos.x + x, gridPos.y + z);
    }

    public bool IsGridFull() => filledCells >= totalCells;

    public void ResetAllCells()
    {
        for (int x = 0; x < gridSystem.GridWidth; x++)
        {
            for (int z = 0; z < gridSystem.GridHeight; z++)
            {
                GridCell cell = gridSystem.GetCell(x, z);
                if (cell != null && cell.IsFilled)
                    cell.SetFilled(false);
            }
        }
        filledCells = 0;
    }

    public float GetFillPercentage()
    {
        if (totalCells == 0) return 0f;
        return (float)filledCells / totalCells;
    }
}
