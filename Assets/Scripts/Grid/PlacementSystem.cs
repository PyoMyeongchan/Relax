using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 블록 배치 로직 (검증, 프리뷰, 배치 실행)
/// </summary>
public class PlacementSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridSystem gridSystem;
    [SerializeField] private GridStateManager stateManager;
    [SerializeField] private InputManager inputManager;

    [Header("Preview Settings")]
    [SerializeField] private Color validPlacementColor = new Color(0f, 1f, 0f, 0.3f);
    [SerializeField] private Color invalidPlacementColor = new Color(1f, 0f, 0f, 0.3f);
    [SerializeField] private bool showPreview = true;

    private BlockObject currentBlock;
    private List<GridCell> previewCells = new List<GridCell>();

    // 이벤트
    public System.Action<BlockObject, Vector2Int> OnBlockPlaced;
    public System.Action OnPlacementFailed;
    public System.Action OnAllBlocksPlaced;

    private void Awake()
    {
        // 자동으로 컴포넌트 찾기
        if (gridSystem == null)
            gridSystem = GetComponent<GridSystem>();
        
        if (stateManager == null)
            stateManager = GetComponent<GridStateManager>();
        
        if (inputManager == null)
            inputManager = FindObjectOfType<InputManager>();

        // Null 체크
        if (gridSystem == null)
            Debug.LogError("GridSystem not found!");
        
        if (stateManager == null)
            Debug.LogError("GridStateManager not found!");
        
        if (inputManager == null)
            Debug.LogError("InputManager not found!");
    }

    private void OnEnable()
    {
        if (inputManager != null)
        {
            inputManager.OnBlockSelected += OnBlockSelected;
            inputManager.OnBlockDeselected += OnBlockDeselected;
            inputManager.OnGridCellClicked += OnGridCellClicked;
        }
    }

    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.OnBlockSelected -= OnBlockSelected;
            inputManager.OnBlockDeselected -= OnBlockDeselected;
            inputManager.OnGridCellClicked -= OnGridCellClicked;
        }

        ClearPreview();
    }

    /// <summary>
    /// 블록 선택됨
    /// </summary>
    private void OnBlockSelected(BlockObject block)
    {
        currentBlock = block;
        Debug.Log($"PlacementSystem: Block selected for placement");
    }

    /// <summary>
    /// 블록 선택 해제됨
    /// </summary>
    private void OnBlockDeselected(BlockObject block)
    {
        currentBlock = null;
        ClearPreview();
    }

    /// <summary>
    /// 격자 셀 클릭됨 - 배치 시도 또는 취소
    /// </summary>
    private void OnGridCellClicked(GridCell cell)
    {
        // 블록이 선택되어 있으면 배치 시도
        if (currentBlock != null)
        {
            Vector2Int gridPos = cell.GridPosition;
            TryPlaceBlock(currentBlock, gridPos);
            return;
        }
        
        // 선택된 블록이 없으면 배치 취소 확인
        TryRemoveBlockAtCell(cell);
    }

    /// <summary>
    /// 해당 셀에 배치된 블록 제거
    /// </summary>
    private void TryRemoveBlockAtCell(GridCell cell)
    {
        if (cell == null || !cell.IsFilled) return;

        // 모든 배치된 블록을 확인
        BlockSpawner spawner = FindObjectOfType<BlockSpawner>();
        if (spawner == null) return;

        foreach (var block in spawner.SpawnedBlocks)
        {
            if (block != null && block.IsPlaced)
            {
                // 이 블록이 클릭한 셀을 포함하는지 확인
                if (IsBlockAtPosition(block, cell.GridPosition))
                {
                    RemoveBlock(block);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 블록이 특정 위치에 있는지 확인
    /// </summary>
    private bool IsBlockAtPosition(BlockObject block, Vector2Int cellPos)
    {
        if (block.PlacedShape == null) return false;

        Vector2Int blockPos = block.PlacedGridPosition;
        int width = block.PlacedShape.GetLength(0);
        int height = block.PlacedShape.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (block.PlacedShape[x, z])
                {
                    Vector2Int checkPos = new Vector2Int(blockPos.x + x, blockPos.y + z);
                    if (checkPos == cellPos)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 배치된 블록 제거
    /// </summary>
    private void RemoveBlock(BlockObject block)
    {
        if (block == null || block.PlacedShape == null) return;

        // 격자 셀들을 비우기
        stateManager.RemoveBlock(block.PlacedGridPosition, block.PlacedShape);

        // 블록을 다시 활성화
        block.SetPlaced(false);

        Debug.Log($"Block removed: {block.name}");
    }

    /// <summary>
    /// 블록 배치 시도
    /// </summary>
    public bool TryPlaceBlock(BlockObject block, Vector2Int gridPosition)
    {
        if (block == null)
        {
            Debug.LogWarning("Block is null!");
            return false;
        }

        bool[,] shape = block.BlockShape;

        // 배치 가능 여부 검증
        if (CanPlaceBlock(gridPosition, shape))
        {
            PlaceBlock(block, gridPosition, shape);
            return true;
        }
        else
        {
            OnPlacementFailed?.Invoke();
            Debug.Log("Cannot place block at this position");
            return false;
        }
    }

    /// <summary>
    /// 블록 배치 가능 여부 검증
    /// </summary>
    private bool CanPlaceBlock(Vector2Int gridPos, bool[,] blockShape)
    {
        if (gridSystem == null || stateManager == null)
        {
            return false;
        }

        if (blockShape == null)
        {
            return false;
        }

        int width = blockShape.GetLength(0);
        int height = blockShape.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (blockShape[x, z])
                {
                    int targetX = gridPos.x + x;
                    int targetZ = gridPos.y + z;

                    // 격자 범위 체크
                    if (!gridSystem.IsValidPosition(targetX, targetZ))
                    {
                        return false;
                    }

                    // GridCell이 null이 아닌지 체크
                    GridCell cell = gridSystem.GetCell(targetX, targetZ);
                    if (cell == null)
                    {
                        return false;
                    }

                    // 이미 채워진 셀인지 체크
                    if (!stateManager.IsCellEmpty(targetX, targetZ))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// 블록 배치 실행
    /// </summary>
    private void PlaceBlock(BlockObject block, Vector2Int gridPos, bool[,] shape)
    {
        // 격자 상태 업데이트
        stateManager.PlaceBlock(gridPos, shape);

        // 블록을 배치된 상태로 설정 (위치 정보 저장)
        block.SetPlaced(true, gridPos, shape);

        // 이벤트 발생
        OnBlockPlaced?.Invoke(block, gridPos);

        Debug.Log($"Block placed at {gridPos}");

        // 선택 해제
        currentBlock = null;
        inputManager.ForceDeselectBlock();

        // 프리뷰 제거
        ClearPreview();

        // 모든 블록 배치 완료 체크
        CheckAllBlocksPlaced();
    }

    /// <summary>
    /// 배치 프리뷰 표시
    /// </summary>
    public void ShowPlacementPreview(Vector2Int gridPos, bool[,] shape)
    {
        if (!showPreview) return;
        if (gridSystem == null || stateManager == null) return;

        ClearPreview();

        bool canPlace = CanPlaceBlock(gridPos, shape);
        Color previewColor = canPlace ? validPlacementColor : invalidPlacementColor;

        int width = shape.GetLength(0);
        int height = shape.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (shape[x, z])
                {
                    int targetX = gridPos.x + x;
                    int targetZ = gridPos.y + z;

                    if (gridSystem.IsValidPosition(targetX, targetZ))
                    {
                        GridCell cell = gridSystem.GetCell(targetX, targetZ);
                        if (cell != null)
                        {
                            cell.SetHighlight(true, previewColor);
                            previewCells.Add(cell);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 프리뷰 제거
    /// </summary>
    private void ClearPreview()
    {
        foreach (var cell in previewCells)
        {
            if (cell != null)
            {
                cell.SetHighlight(false, Color.white);
            }
        }
        previewCells.Clear();
    }

    /// <summary>
    /// 모든 블록이 배치되었는지 확인
    /// </summary>
    private void CheckAllBlocksPlaced()
    {
        BlockSpawner spawner = FindObjectOfType<BlockSpawner>();
        if (spawner != null && spawner.AreAllBlocksPlaced())
        {
            OnAllBlocksPlaced?.Invoke();
            Debug.Log("All blocks placed!");
        }
    }

    /// <summary>
    /// 격자가 완전히 채워졌는지 확인
    /// </summary>
    public bool IsGridComplete()
    {
        return stateManager.IsGridFull();
    }

    /// <summary>
    /// 마우스 호버 시 프리뷰 (Update에서 호출)
    /// </summary>
    private void Update()
    {
        if (currentBlock == null || !showPreview) return;
        if (gridSystem == null || stateManager == null) return;

        // New Input System: 마우스/터치 위치
        Vector2 screenPos = UnityEngine.InputSystem.Mouse.current?.position.ReadValue() ?? Vector2.zero;
        
        if (screenPos == Vector2.zero && UnityEngine.InputSystem.Touchscreen.current != null)
        {
            var touch = UnityEngine.InputSystem.Touchscreen.current.primaryTouch;
            if (touch.isInProgress)
            {
                screenPos = touch.position.ReadValue();
            }
        }

        if (screenPos == Vector2.zero) return;

        // 마우스 위치를 격자 좌표로 변환
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Vector2Int gridPos = gridSystem.WorldToGridPosition(hit.point);
            ShowPlacementPreview(gridPos, currentBlock.BlockShape);
        }
    }
}