using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 블록 개체 (모양, 선택 상태, 회전)
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

    [Header("Visual")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material selectedMaterial;
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
    /// 블록 초기화
    /// </summary>
    public void Initialize(bool[,] shape, GameObject prefab, Material normal, Material selected)
    {
        if (shape == null)
        {
            Debug.LogError("Initialize: shape is null!");
            return;
        }

        // Clone으로 복사!
        blockShape = (bool[,])shape.Clone();
        shapeSize = new Vector2Int(shape.GetLength(0), shape.GetLength(1));
        cellPrefab = prefab;
        normalMaterial = normal;
        selectedMaterial = selected;
        isSelected = false;
        isPlaced = false;
        currentRotation = 0;

        Debug.Log($"BlockObject initialized: {shapeSize.x}x{shapeSize.y}");

        CreateVisual();
    }

    /// <summary>
    /// 블록 시각적 생성
    /// </summary>
    private void CreateVisual()
    {
        ClearVisual();

        int width = blockShape.GetLength(0);
        int height = blockShape.GetLength(1);

        // 중앙 정렬을 위한 오프셋
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
                    
                    // Layer 설정
                    cell.layer = LayerMask.NameToLayer("Block");
                    
                    // Collider 확인 및 추가
                    if (cell.GetComponent<Collider>() == null)
                    {
                        cell.AddComponent<BoxCollider>();
                    }

                    MeshRenderer renderer = cell.GetComponent<MeshRenderer>();
                    if (renderer != null && normalMaterial != null)
                    {
                        renderer.material = normalMaterial;
                    }

                    visualCells.Add(cell);
                }
            }
        }
    }

    /// <summary>
    /// 시각 요소 제거
    /// </summary>
    private void ClearVisual()
    {
        foreach (var cell in visualCells)
        {
            if (cell != null)
            {
                Destroy(cell);
            }
        }
        visualCells.Clear();
    }

    /// <summary>
    /// 블록 선택 상태 변경
    /// </summary>
    public void SetSelected(bool selected)
    {
        isSelected = selected;
        UpdateVisual();
    }

    /// <summary>
    /// 블록 배치 완료
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
            // 배치 취소
            placedGridPosition = Vector2Int.zero;
            placedShape = null;
            gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 시각적 업데이트
    /// </summary>
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
                    renderer.material = material;
                }
            }
        }
    }

    /// <summary>
    /// 블록 회전 (시계방향 90도)
    /// </summary>
    public void Rotate()
    {
        currentRotation = (currentRotation + 90) % 360;
        CreateVisual(); // 회전 후 시각 재생성
        UpdateVisual();
    }

    /// <summary>
    /// 현재 회전 상태의 블록 모양 반환
    /// </summary>
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
        {
            result = RotateShapeClockwise(result);
        }

        return result;
    }

    /// <summary>
    /// 모양을 시계방향 90도 회전
    /// </summary>
    private bool[,] RotateShapeClockwise(bool[,] shape)
    {
        int width = shape.GetLength(0);
        int height = shape.GetLength(1);
        bool[,] rotated = new bool[height, width];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                rotated[z, width - 1 - x] = shape[x, z];
            }
        }

        return rotated;
    }

    /// <summary>
    /// 현재 회전 상태의 블록 크기 반환
    /// </summary>
    private Vector2Int GetRotatedSize()
    {
        if (currentRotation == 90 || currentRotation == 270)
        {
            return new Vector2Int(shapeSize.y, shapeSize.x);
        }
        return shapeSize;
    }

    /// <summary>
    /// 블록 리셋 (재사용용)
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

    private void OnDestroy()
    {
        ClearVisual();
    }
}