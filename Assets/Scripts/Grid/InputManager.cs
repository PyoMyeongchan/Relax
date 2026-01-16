using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/// <summary>
/// 클릭 입력 처리 (New Input System 사용)
/// </summary>
public class InputManager : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private float maxRaycastDistance = 100f;

    [Header("Current State")]
    [SerializeField] private BlockObject selectedBlock;

    // 이벤트
    public System.Action<BlockObject> OnBlockSelected;
    public System.Action<BlockObject> OnBlockDeselected;
    public System.Action<GridCell> OnGridCellClicked;
    public System.Action<Vector3> OnEmptySpaceClicked;

    // New Input System
    private PlayerInput playerInput;
    private InputAction tapAction;
    private InputAction positionAction;
    private InputAction rotateAction;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
        }

        SetupInputActions();
    }

    /// <summary>
    /// Input Actions 설정
    /// </summary>
    private void SetupInputActions()
    {
        // Input Actions 생성
        var inputActionAsset = ScriptableObject.CreateInstance<InputActionAsset>();
        
        var gameplayMap = inputActionAsset.AddActionMap("Gameplay");
        
        // Tap/Click Action
        tapAction = gameplayMap.AddAction("Tap", InputActionType.Button);
        tapAction.AddBinding("<Mouse>/leftButton");
        tapAction.AddBinding("<Touchscreen>/primaryTouch/press");
        
        // Position Action (마우스/터치 위치)
        positionAction = gameplayMap.AddAction("Position", InputActionType.Value);
        positionAction.AddBinding("<Mouse>/position");
        positionAction.AddBinding("<Touchscreen>/primaryTouch/position");
        
        // Rotate Action (스페이스바)
        rotateAction = gameplayMap.AddAction("Rotate", InputActionType.Button);
        rotateAction.AddBinding("<Keyboard>/space");
        
        // Enable
        gameplayMap.Enable();
        
        // 이벤트 연결
        tapAction.performed += OnTapPerformed;
        rotateAction.performed += OnRotatePerformed;
    }

    private void OnEnable()
    {
        if (tapAction != null) tapAction.Enable();
        if (positionAction != null) positionAction.Enable();
        if (rotateAction != null) rotateAction.Enable();
    }

    private void OnDisable()
    {
        if (tapAction != null) tapAction.Disable();
        if (positionAction != null) positionAction.Disable();
        if (rotateAction != null) rotateAction.Disable();
    }

    /// <summary>
    /// Tap/Click 이벤트
    /// </summary>
    private void OnTapPerformed(InputAction.CallbackContext context)
    {
        // UI 클릭은 무시
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Vector2 screenPosition = positionAction.ReadValue<Vector2>();
        HandleClick(screenPosition);
    }

    /// <summary>
    /// Rotate 이벤트
    /// </summary>
    private void OnRotatePerformed(InputAction.CallbackContext context)
    {
        if (selectedBlock != null)
        {
            RotateSelectedBlock();
        }
    }

    /// <summary>
    /// 클릭/터치 처리
    /// </summary>
    private void HandleClick(Vector2 screenPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        // 1. 블록 클릭 체크
        if (Physics.Raycast(ray, out hit, maxRaycastDistance, blockLayer))
        {
            BlockObject block = hit.collider.GetComponentInParent<BlockObject>();
            if (block != null && !block.IsPlaced)
            {
                SelectBlock(block);
                return;
            }
        }

        // 2. 격자 셀 클릭 체크
        if (Physics.Raycast(ray, out hit, maxRaycastDistance, gridLayer))
        {
            GridCell cell = hit.collider.GetComponent<GridCell>();
            if (cell != null)
            {
                OnGridCellClicked?.Invoke(cell);
                return;
            }
        }

        // 3. 빈 공간 클릭
        OnEmptySpaceClicked?.Invoke(hit.point);
        
        // 블록 선택 해제
        if (selectedBlock != null)
        {
            DeselectBlock();
        }
    }

    /// <summary>
    /// 블록 선택
    /// </summary>
    private void SelectBlock(BlockObject block)
    {
        // 이미 선택된 블록을 다시 클릭하면 해제
        if (selectedBlock == block)
        {
            DeselectBlock();
            return;
        }

        // 기존 선택 해제
        if (selectedBlock != null)
        {
            selectedBlock.SetSelected(false);
            OnBlockDeselected?.Invoke(selectedBlock);
        }

        // 새 블록 선택
        selectedBlock = block;
        selectedBlock.SetSelected(true);
        
        // 디버그
        Debug.Log($"Block selected: {selectedBlock.name}");
        Debug.Log($"Block shape is null? {selectedBlock.BlockShape == null}");
        
        OnBlockSelected?.Invoke(selectedBlock);
    }

    /// <summary>
    /// 블록 선택 해제
    /// </summary>
    private void DeselectBlock()
    {
        if (selectedBlock != null)
        {
            selectedBlock.SetSelected(false);
            OnBlockDeselected?.Invoke(selectedBlock);
            selectedBlock = null;

            Debug.Log("Block deselected");
        }
    }

    /// <summary>
    /// 선택된 블록 회전
    /// </summary>
    private void RotateSelectedBlock()
    {
        if (selectedBlock != null)
        {
            selectedBlock.Rotate();
            Debug.Log($"Block rotated: {selectedBlock.CurrentRotation}°");
        }
    }

    /// <summary>
    /// 현재 선택된 블록 가져오기
    /// </summary>
    public BlockObject GetSelectedBlock()
    {
        return selectedBlock;
    }

    /// <summary>
    /// 강제로 블록 선택 해제
    /// </summary>
    public void ForceDeselectBlock()
    {
        DeselectBlock();
    }

    /// <summary>
    /// 블록이 선택되어 있는지
    /// </summary>
    public bool HasSelectedBlock()
    {
        return selectedBlock != null;
    }

    // 디버그용
    private void OnDrawGizmos()
    {
        if (selectedBlock != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(selectedBlock.transform.position, 1f);
        }
    }

    private void OnDestroy()
    {
        // 이벤트 해제
        if (tapAction != null)
            tapAction.performed -= OnTapPerformed;
        
        if (rotateAction != null)
            rotateAction.performed -= OnRotatePerformed;
    }
}