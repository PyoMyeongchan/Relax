using UnityEngine;

/// <summary>
/// Block shape data (serializable).
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

    [Header("Visual")]
    public Color blockColor = Color.white;

    /// <summary>
    /// Flattened 1D array for Unity Inspector serialization.
    /// </summary>
    [SerializeField]
    private bool[] shapeFlattened;

    [SerializeField]
    private float colorR, colorG, colorB, colorA;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public BlockShapeData()
    {
        width = 1;
        height = 1;
        shape = new bool[1, 1] { { true } };
        FlattenShape();
    }

    /// <summary>
    /// Constructor with explicit dimensions.
    /// </summary>
    public BlockShapeData(int w, int h)
    {
        width = w;
        height = h;
        shape = new bool[w, h];
        FlattenShape();
    }

    /// <summary>
    /// Constructor with a pre-built shape.
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
            shape = (bool[,])blockShape.Clone();
        }
        FlattenShape();
    }

    /// <summary>
    /// Flatten the 2D shape to a 1D array for serialization.
    /// </summary>
    public void FlattenShape()
    {
        if (shape == null)
        {
            UnflattenShape();
            if (shape == null) return;
        }

        shapeFlattened = new bool[width * height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                shapeFlattened[x * height + y] = shape[x, y];

        SaveColor();
    }

    /// <summary>
    /// Restore the 2D shape from the flattened 1D array.
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
                    shape[x, y] = shapeFlattened[index];
            }
        }
    }

    /// <summary>
    /// Returns true if at least one cell is filled.
    /// </summary>
    public bool IsValid()
    {
        if (shape == null) return false;
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (shape[x, y]) return true;
        return false;
    }

    /// <summary>
    /// Number of filled cells.
    /// </summary>
    public int GetFilledCellCount()
    {
        int count = 0;
        if (shape == null) return count;
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (shape[x, y]) count++;
        return count;
    }

    // Called by ScriptableObject.OnValidate
    public void OnValidate()
    {
        UnflattenShape();
        LoadColor();
    }

    /// <summary>
    /// Save color components for serialization.
    /// </summary>
    public void SaveColor()
    {
        colorR = blockColor.r;
        colorG = blockColor.g;
        colorB = blockColor.b;
        colorA = blockColor.a;
    }

    /// <summary>
    /// Restore color from serialized components.
    /// </summary>
    public void LoadColor()
    {
        blockColor = new Color(colorR, colorG, colorB, colorA);
    }
}

/// <summary>
/// Predefined block shapes.
/// </summary>
public static class PredefinedShapes
{
    // I-block (1x4)
    public static bool[,] I_Shape = new bool[,]
    {
        { true },
        { true },
        { true },
        { true }
    };

    // O-block (2x2)
    public static bool[,] O_Shape = new bool[,]
    {
        { true, true },
        { true, true }
    };

    // T-block
    public static bool[,] T_Shape = new bool[,]
    {
        { false, true, false },
        { true, true, true }
    };

    // L-block
    public static bool[,] L_Shape = new bool[,]
    {
        { true, false },
        { true, false },
        { true, true }
    };

    // J-block
    public static bool[,] J_Shape = new bool[,]
    {
        { false, true },
        { false, true },
        { true, true }
    };

    // Z-block
    public static bool[,] Z_Shape = new bool[,]
    {
        { true, true, false },
        { false, true, true }
    };

    // S-block
    public static bool[,] S_Shape = new bool[,]
    {
        { false, true, true },
        { true, true, false }
    };

    // 1x1 single
    public static bool[,] Single_Shape = new bool[,]
    {
        { true }
    };

    // 1x2 domino
    public static bool[,] Domino_Shape = new bool[,]
    {
        { true, true }
    };

    // 1x3 triple
    public static bool[,] Triple_Shape = new bool[,]
    {
        { true, true, true }
    };
}
