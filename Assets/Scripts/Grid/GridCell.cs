using UnityEngine;

/// <summary>
/// Individual grid cell — position and fill state.
/// </summary>
public class GridCell : MonoBehaviour
{
    [Header("Cell Info")]
    [SerializeField] private Vector2Int gridPosition;
    [SerializeField] private bool isFilled;
    [SerializeField] private Color filledColor = Color.white;

    [Header("Visual")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material emptyMaterial;
    [SerializeField] private Material filledMaterial;

    public Vector2Int GridPosition => gridPosition;
    public bool IsFilled => isFilled;

    /// <summary>
    /// Initialize the cell at the given grid coordinates.
    /// </summary>
    public void Initialize(int x, int z, float size)
    {
        gridPosition = new Vector2Int(x, z);
        isFilled = false;

        transform.localScale = new Vector3(size, 0.1f, size);

        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

        UpdateVisual();
    }

    /// <summary>
    /// Set the fill state of this cell.
    /// </summary>
    public void SetFilled(bool filled, Color color = default)
    {
        if (isFilled == filled && color == default) return;

        isFilled = filled;
        if (filled && color != default)
            filledColor = color;

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (meshRenderer == null) return;

        if (isFilled && filledMaterial != null)
        {
            meshRenderer.material = new Material(filledMaterial);
            meshRenderer.material.color = filledColor;
        }
        else if (!isFilled && emptyMaterial != null)
        {
            meshRenderer.material = emptyMaterial;
        }
    }

    /// <summary>
    /// Highlight this cell for placement preview.
    /// </summary>
    public void SetHighlight(bool highlight, Color highlightColor)
    {
        if (meshRenderer == null) return;

        if (highlight)
            meshRenderer.material.color = highlightColor;
        else
            UpdateVisual();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isFilled ? Color.red : Color.green;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
