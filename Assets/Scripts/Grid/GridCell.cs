using UnityEngine;

/// <summary>
/// 개별 격자 셀 (위치, 상태 관리)
/// </summary>
public class GridCell : MonoBehaviour
{
    [Header("Cell Info")]
    [SerializeField] private Vector2Int gridPosition;
    [SerializeField] private bool isFilled;

    [Header("Visual")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material emptyMaterial;
    [SerializeField] private Material filledMaterial;

    public Vector2Int GridPosition => gridPosition;
    public bool IsFilled => isFilled;

    /// <summary>
    /// 셀 초기화
    /// </summary>
    public void Initialize(int x, int z, float size)
    {
        gridPosition = new Vector2Int(x, z);
        isFilled = false;

        // 크기 조정
        transform.localScale = new Vector3(size, 0.1f, size);

        // MeshRenderer 자동 찾기
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        UpdateVisual();
    }

    /// <summary>
    /// 셀 상태 변경
    /// </summary>
    public void SetFilled(bool filled)
    {
        if (isFilled == filled) return;

        isFilled = filled;
        UpdateVisual();
    }

    /// <summary>
    /// 시각적 업데이트
    /// </summary>
    private void UpdateVisual()
    {
        if (meshRenderer == null) return;

        if (isFilled && filledMaterial != null)
        {
            meshRenderer.material = filledMaterial;
        }
        else if (!isFilled && emptyMaterial != null)
        {
            meshRenderer.material = emptyMaterial;
        }
    }

    /// <summary>
    /// 하이라이트 표시 (배치 가능 위치 미리보기)
    /// </summary>
    public void SetHighlight(bool highlight, Color highlightColor)
    {
        if (meshRenderer == null) return;

        if (highlight)
        {
            meshRenderer.material.color = highlightColor;
        }
        else
        {
            UpdateVisual();
        }
    }

    // 디버그용
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isFilled ? Color.red : Color.green;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}