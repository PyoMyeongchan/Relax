using UnityEngine;

/// <summary>
/// 블록 모양 데이터 (직렬화 가능)
/// </summary>
[System.Serializable]
public class BlockShapeData
{
    [Header("Shape Definition")]
    [Tooltip("2D array representing block shape (true = filled, false = empty)")]
    public bool[,] shape;

    [Header("Shape Size")]
    public int width;
    public int height;

    /// <summary>
    /// 1D 배열로 저장 (Unity Inspector 직렬화용)
    /// </summary>
    [SerializeField]
    private bool[] shapeFlattened;

    /// <summary>
    /// 기본 생성자
    /// </summary>
    public BlockShapeData()
    {
        width = 1;
        height = 1;
        shape = new bool[1, 1] { { true } };
        FlattenShape();
    }

    /// <summary>
    /// 크기 지정 생성자
    /// </summary>
    public BlockShapeData(int w, int h)
    {
        width = w;
        height = h;
        shape = new bool[w, h];
        FlattenShape();
    }

    /// <summary>
    /// 모양 직접 지정 생성자
    /// </summary>
    public BlockShapeData(bool[,] blockShape)
    {
        if (blockShape == null)
        {
            Debug.LogError("Block shape is null!");
            width = 1;
            height = 1;
            shape = new bool[1, 1] { { true } };
        }
        else
        {
            width = blockShape.GetLength(0);
            height = blockShape.GetLength(1);
            shape = (bool[,])blockShape.Clone(); // Clone 추가!
        }
        FlattenShape();
    }

    /// <summary>
    /// 2D 배열을 1D로 평탄화 (직렬화용)
    /// </summary>
    public void FlattenShape()
    {
        if (shape == null)
        {
            // shape가 null이면 복원 시도
            UnflattenShape();
            if (shape == null) return;
        }

        shapeFlattened = new bool[width * height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                shapeFlattened[x * height + y] = shape[x, y];
            }
        }
    }

    /// <summary>
    /// 1D 배열을 2D로 복원 (역직렬화용)
    /// </summary>
    public void UnflattenShape()
    {
        if (shapeFlattened == null || shapeFlattened.Length == 0)
        {
            Debug.LogWarning("shapeFlattened is null or empty!");
            return;
        }
        
        if (width <= 0 || height <= 0)
        {
            Debug.LogWarning($"Invalid dimensions: {width}x{height}");
            return;
        }

        shape = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = x * height + y;
                if (index < shapeFlattened.Length)
                {
                    shape[x, y] = shapeFlattened[index];
                }
            }
        }
    }

    /// <summary>
    /// 검증: 최소 하나 이상의 셀이 채워져 있는지
    /// </summary>
    public bool IsValid()
    {
        if (shape == null) return false;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (shape[x, y]) return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 채워진 셀 개수
    /// </summary>
    public int GetFilledCellCount()
    {
        int count = 0;
        if (shape == null) return count;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (shape[x, y]) count++;
            }
        }
        return count;
    }

    // OnValidate는 ScriptableObject에서 사용
    public void OnValidate()
    {
        UnflattenShape();
    }
}

/// <summary>
/// 미리 정의된 블록 모양 (테트리스 스타일)
/// </summary>
public static class PredefinedShapes
{
    // I 블록 (1x4)
    public static bool[,] I_Shape = new bool[,]
    {
        { true },
        { true },
        { true },
        { true }
    };

    // O 블록 (2x2)
    public static bool[,] O_Shape = new bool[,]
    {
        { true, true },
        { true, true }
    };

    // T 블록
    public static bool[,] T_Shape = new bool[,]
    {
        { false, true, false },
        { true, true, true }
    };

    // L 블록
    public static bool[,] L_Shape = new bool[,]
    {
        { true, false },
        { true, false },
        { true, true }
    };

    // J 블록
    public static bool[,] J_Shape = new bool[,]
    {
        { false, true },
        { false, true },
        { true, true }
    };

    // Z 블록
    public static bool[,] Z_Shape = new bool[,]
    {
        { true, true, false },
        { false, true, true }
    };

    // S 블록
    public static bool[,] S_Shape = new bool[,]
    {
        { false, true, true },
        { true, true, false }
    };

    // 1x1 블록
    public static bool[,] Single_Shape = new bool[,]
    {
        { true }
    };

    // 1x2 블록
    public static bool[,] Domino_Shape = new bool[,]
    {
        { true, true }
    };

    // 1x3 블록
    public static bool[,] Triple_Shape = new bool[,]
    {
        { true, true, true }
    };
}