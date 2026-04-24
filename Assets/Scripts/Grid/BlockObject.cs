using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A block instance — shape, selection state, and rotation.
/// </summary>
public class BlockObject : MonoBehaviour
{
    [Header("Block Shape")]
    [SerializeField] private bool[,] blockShape;
    [SerializeField] private Vector2Int shapeSize;

    [Header("State")]
    [SerializeField] private bool isSelected;
    [SerializeField] private bool isPlaced;
    [SerializeField] private int currentRotation; // 0, 90, 180, 270

    [Header("Placement Info")]
    [SerializeField] private Vector2Int placedGridPosition;
    [SerializeField] private bool[,] placedShape;

    public Vector2Int PlacedGridPosition => placedGridPosition;
    public bool[,] PlacedShape => placedShape;
    public Color BlockColor => blockColor;

    [Header("Visual")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Color blockColor = Color.white;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private float cellSpacing = 0.1f;

    private List<GameObject> visualCells = new List<GameObject>();

    public bool[,] BlockShape
    {
        get
        {
            if (blockShape == null)
            {
                Debug.LogError("blockShape is null in BlockShape getter!");
                return null;
            }
            return GetRotatedShape();
        }
    }
    public Vector2Int ShapeSize => GetRotatedSize();
    public bool IsSelected => isSelected;
    public bool IsPlaced => isPlaced;
    public int CurrentRotation => currentRotation;

    /// <summary>
    /// Initialize the block with shape, prefabs and color.
    /// </summary>
    public void Initialize(bool[,] shape, GameObject prefab, Material normal, Material selected, Color color = default)
    {
        if (shape == null)
        {
            Debug.LogError("Initialize: shape is null!");
            return;
        }

        blockShape = (bool[,])shape.Clone();
        shapeSize = new Vector2Int(shape.GetLength(0), shape.GetLength(1));
        cellPrefab = prefab;
        normalMaterial = normal;
        selectedMaterial = selected;
        blockColor = color == default ? Color.white : color;
        isSelected = false;
        isPlaced = false;
        currentRotation = 0;

        CreateVisual();
    }

    private void CreateVisual()
    {
        ClearVisual();

        int width = blockShape.GetLength(0);
        int height = blockShape.GetLength(1);

        // Center offset
        float offsetX = -(width * (cellSize + cellSpacing)) / 2f + (cellSize + cellSpacing) / 2f;
        float offsetZ = -(height * (cellSize + cellSpacing)) / 2f + (cellSize + cellSpacing) / 2f;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (blockShape[x, z])
                {
                    Vector3 localPos = new Vector3(
                        offsetX + x * (cellSize + cellSpacing),
                        0f,
                        offsetZ + z * (cellSize + cellSpacing)
                    );

                    GameObject cell = Instantiate(cellPrefab, transform);
                    cell.transform.localPosition = localPos;
                    cell.transform.localScale = new Vector3(cellSize, 0.2f, cellSize);
                    cell.name = $"BlockCell_{x}_{z}";

                    cell.layer = LayerMask.NameToLayer("Block");

                    if (cell.GetComponent<Collider>() == null)
                        cell.AddComponent<BoxCollider>();

                    MeshRenderer renderer = cell.GetComponent<MeshRenderer>();
                    if (renderer != null && normalMaterial != null)
                    {
                        renderer.material = new Material(normalMaterial);
                        renderer.material.color = blockColor;
                    }

                    visualCells.Add(cell);
                }
            }
        }
    }

    private void ClearVisual()
    {
        foreach (var cell in visualCells)
            if (cell != null) Destroy(cell);
        visualCells.Clear();
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        UpdateVisual();
    }

    /// <summary>
    /// Mark the block as placed (or un-placed) at the given grid position.
    /// </summary>
    public void SetPlaced(bool placed, Vector2Int gridPos = default, bool[,] shape = null)
    {
        isPlaced = placed;

        if (placed)
        {
            placedGridPosition = gridPos;
            placedShape = shape != null ? (bool[,])shape.Clone() : null;
            gameObject.SetActive(false);
        }
        else
        {
            placedGridPosition = Vector2Int.zero;
            placedShape = null;
            gameObject.SetActive(true);
        }
    }

    private void UpdateVisual()
    {
        Material material = isSelected ? selectedMaterial : normalMaterial;

        foreach (var cell in visualCells)
        {
            if (cell != null)
            {
                MeshRenderer renderer = cell.GetComponent<MeshRenderer>();
                if (renderer != null && material != null)
                {
                    renderer.material = new Material(material);
                    renderer.material.color = blockColor;
                }
            }
        }
    }

    /// <summary>
    /// Rotate the block 90 degrees clockwise.
    /// </summary>
    public void Rotate()
    {
        currentRotation = (currentRotation + 90) % 360;
        CreateVisual();
        UpdateVisual();
    }

    private bool[,] GetRotatedShape()
    {
        if (blockShape == null)
        {
            Debug.LogError("GetRotatedShape: blockShape is null!");
            return null;
        }

        int rotationCount = currentRotation / 90;
        bool[,] result = (bool[,])blockShape.Clone();

        for (int i = 0; i < rotationCount; i++)
            result = RotateShapeClockwise(result);

        return result;
    }

    private bool[,] RotateShapeClockwise(bool[,] shape)
    {
        int width = shape.GetLength(0);
        int height = shape.GetLength(1);
        bool[,] rotated = new bool[height, width];

        for (int x = 0; x < width; x++)
            for (int z = 0; z < height; z++)
                rotated[z, width - 1 - x] = shape[x, z];

        return rotated;
    }

    private Vector2Int GetRotatedSize()
    {
        if (currentRotation == 90 || currentRotation == 270)
            return new Vector2Int(shapeSize.y, shapeSize.x);
        return shapeSize;
    }

    /// <summary>
    /// Reset the block to its initial state (for stage restart).
    /// </summary>
    public void Reset()
    {
        isSelected = false;
        isPlaced = false;
        currentRotation = 0;
        gameObject.SetActive(true);
        CreateVisual();
        UpdateVisual();
    }

    private void OnDestroy() => ClearVisual();
}
