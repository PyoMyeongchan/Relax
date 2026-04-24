using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/// <summary>
/// Handles click/touch input using the New Input System.
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

    // Events
    public System.Action<BlockObject> OnBlockSelected;
    public System.Action<BlockObject> OnBlockDeselected;
    public System.Action<GridCell> OnGridCellClicked;
    public System.Action<Vector3> OnEmptySpaceClicked;

    private PlayerInput playerInput;
    private InputAction tapAction;
    private InputAction positionAction;
    private InputAction rotateAction;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null)
            Debug.LogError("Main Camera not found!");

        SetupInputActions();
    }

    private void SetupInputActions()
    {
        var inputActionAsset = ScriptableObject.CreateInstance<InputActionAsset>();
        var gameplayMap = inputActionAsset.AddActionMap("Gameplay");

        // Tap / click
        tapAction = gameplayMap.AddAction("Tap", InputActionType.Button);
        tapAction.AddBinding("<Mouse>/leftButton");
        tapAction.AddBinding("<Touchscreen>/primaryTouch/press");

        // Pointer position
        positionAction = gameplayMap.AddAction("Position", InputActionType.Value);
        positionAction.AddBinding("<Mouse>/position");
        positionAction.AddBinding("<Touchscreen>/primaryTouch/position");

        // Rotate (spacebar)
        rotateAction = gameplayMap.AddAction("Rotate", InputActionType.Button);
        rotateAction.AddBinding("<Keyboard>/space");

        gameplayMap.Enable();

        tapAction.performed += OnTapPerformed;
        rotateAction.performed += OnRotatePerformed;
    }

    private void OnEnable()
    {
        tapAction?.Enable();
        positionAction?.Enable();
        rotateAction?.Enable();
    }

    private void OnDisable()
    {
        tapAction?.Disable();
        positionAction?.Disable();
        rotateAction?.Disable();
    }

    private void OnTapPerformed(InputAction.CallbackContext context)
    {
        // Ignore clicks on UI elements
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Vector2 screenPosition = positionAction.ReadValue<Vector2>();
        HandleClick(screenPosition);
    }

    private void OnRotatePerformed(InputAction.CallbackContext context)
    {
        if (selectedBlock != null)
            RotateSelectedBlock();
    }

    private void HandleClick(Vector2 screenPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        // 1. Check for block click
        if (Physics.Raycast(ray, out hit, maxRaycastDistance, blockLayer))
        {
            BlockObject block = hit.collider.GetComponentInParent<BlockObject>();
            if (block != null && !block.IsPlaced)
            {
                SelectBlock(block);
                return;
            }
        }

        // 2. Check for grid cell click
        if (Physics.Raycast(ray, out hit, maxRaycastDistance, gridLayer))
        {
            GridCell cell = hit.collider.GetComponent<GridCell>();
            if (cell != null)
            {
                OnGridCellClicked?.Invoke(cell);
                return;
            }
        }

        // 3. Empty space
        OnEmptySpaceClicked?.Invoke(hit.point);

        if (selectedBlock != null)
            DeselectBlock();
    }

    private void SelectBlock(BlockObject block)
    {
        // Clicking the already-selected block deselects it
        if (selectedBlock == block)
        {
            DeselectBlock();
            return;
        }

        if (selectedBlock != null)
        {
            selectedBlock.SetSelected(false);
            OnBlockDeselected?.Invoke(selectedBlock);
        }

        selectedBlock = block;
        selectedBlock.SetSelected(true);
        OnBlockSelected?.Invoke(selectedBlock);
    }

    private void DeselectBlock()
    {
        if (selectedBlock != null)
        {
            selectedBlock.SetSelected(false);
            OnBlockDeselected?.Invoke(selectedBlock);
            selectedBlock = null;
        }
    }

    private void RotateSelectedBlock()
    {
        selectedBlock?.Rotate();
    }

    public BlockObject GetSelectedBlock() => selectedBlock;

    public void ForceDeselectBlock() => DeselectBlock();

    public bool HasSelectedBlock() => selectedBlock != null;

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
        if (tapAction != null) tapAction.performed -= OnTapPerformed;
        if (rotateAction != null) rotateAction.performed -= OnRotatePerformed;
    }
}
