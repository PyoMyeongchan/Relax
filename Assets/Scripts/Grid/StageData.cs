using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 스테이지 데이터 (ScriptableObject)
/// </summary>
[CreateAssetMenu(fileName = "Stage_", menuName = "Puzzle Game/Stage Data", order = 1)]
public class StageData : ScriptableObject
{
    [Header("Stage Info")]
    [Tooltip("스테이지 번호 (예: 1-1, 1-2)")]
    public string stageName = "1-1";
    
    [Tooltip("스테이지 설명")]
    [TextArea(2, 4)]
    public string stageDescription = "";

    [Header("Grid Settings")]
    [Tooltip("격자 가로 크기")]
    [Range(3, 10)]
    public int gridWidth = 5;
    
    [Tooltip("격자 세로 크기")]
    [Range(3, 10)]
    public int gridHeight = 5;

    [Header("Blocks")]
    [Tooltip("이 스테이지에서 사용할 블록들")]
    public List<BlockShapeData> blocks = new List<BlockShapeData>();

    [Header("Difficulty (Optional)")]
    [Tooltip("난이도 (1=쉬움, 5=어려움)")]
    [Range(1, 5)]
    public int difficulty = 1;

    /// <summary>
    /// 스테이지 검증
    /// </summary>
    public bool Validate()
    {
        // 격자 크기 체크
        if (gridWidth <= 0 || gridHeight <= 0)
        {
            Debug.LogError($"[{stageName}] Invalid grid size!");
            return false;
        }

        // 블록이 없으면 안됨
        if (blocks == null || blocks.Count == 0)
        {
            Debug.LogError($"[{stageName}] No blocks defined!");
            return false;
        }

        // 각 블록 검증
        foreach (var block in blocks)
        {
            if (block == null || !block.IsValid())
            {
                Debug.LogError($"[{stageName}] Invalid block found!");
                return false;
            }
        }

        // 블록들이 격자를 채울 수 있는지 체크
        int totalBlockCells = GetTotalBlockCells();
        int gridCells = gridWidth * gridHeight;

        if (totalBlockCells > gridCells)
        {
            Debug.LogWarning($"[{stageName}] Too many block cells ({totalBlockCells}) for grid ({gridCells})!");
        }
        else if (totalBlockCells < gridCells)
        {
            Debug.LogWarning($"[{stageName}] Not enough block cells ({totalBlockCells}) to fill grid ({gridCells})!");
        }

        return true;
    }

    /// <summary>
    /// 모든 블록의 총 셀 개수
    /// </summary>
    public int GetTotalBlockCells()
    {
        int total = 0;
        foreach (var block in blocks)
        {
            if (block != null)
            {
                total += block.GetFilledCellCount();
            }
        }
        return total;
    }

    /// <summary>
    /// 블록 추가 (에디터 헬퍼)
    /// </summary>
    public void AddBlock(bool[,] shape)
    {
        blocks.Add(new BlockShapeData(shape));
    }

    /// <summary>
    /// 미리 정의된 블록 추가 (에디터 헬퍼)
    /// </summary>
    public void AddPredefinedBlock(string shapeName)
    {
        bool[,] shape = null;

        switch (shapeName.ToUpper())
        {
            case "I": shape = PredefinedShapes.I_Shape; break;
            case "O": shape = PredefinedShapes.O_Shape; break;
            case "T": shape = PredefinedShapes.T_Shape; break;
            case "L": shape = PredefinedShapes.L_Shape; break;
            case "J": shape = PredefinedShapes.J_Shape; break;
            case "Z": shape = PredefinedShapes.Z_Shape; break;
            case "S": shape = PredefinedShapes.S_Shape; break;
            case "SINGLE": shape = PredefinedShapes.Single_Shape; break;
            case "DOMINO": shape = PredefinedShapes.Domino_Shape; break;
            case "TRIPLE": shape = PredefinedShapes.Triple_Shape; break;
            default:
                Debug.LogWarning($"Unknown shape: {shapeName}");
                return;
        }

        if (shape != null)
        {
            AddBlock(shape);
        }
    }

    /// <summary>
    /// 모든 블록 제거
    /// </summary>
    public void ClearBlocks()
    {
        blocks.Clear();
    }

    // Inspector에서 수정할 때마다 검증
    private void OnValidate()
    {
        // 블록 데이터 복원
        foreach (var block in blocks)
        {
            if (block != null)
            {
                block.OnValidate();
            }
        }

        // 스테이지 이름이 비어있으면 파일명 사용
        if (string.IsNullOrEmpty(stageName))
        {
            stageName = name;
        }
    }
}

#if UNITY_EDITOR
/// <summary>
/// 스테이지 데이터 생성 헬퍼 (에디터 전용)
/// </summary>
public static class StageDataHelper
{
    /// <summary>
    /// 간단한 테스트 스테이지 생성
    /// </summary>
    public static StageData CreateTestStage()
    {
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "Test Stage";
        stage.gridWidth = 5;
        stage.gridHeight = 5;
        
        // 간단한 블록 3개 추가
        stage.AddPredefinedBlock("L");
        stage.AddPredefinedBlock("O");
        stage.AddPredefinedBlock("I");

        return stage;
    }

    /// <summary>
    /// 5x5 격자를 완벽히 채우는 스테이지
    /// </summary>
    public static StageData CreatePerfectFitStage()
    {
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "Perfect Fit 5x5";
        stage.gridWidth = 5;
        stage.gridHeight = 5;

        // 총 25칸을 채우는 블록 조합
        stage.AddPredefinedBlock("L");      // 4칸
        stage.AddPredefinedBlock("L");      // 4칸
        stage.AddPredefinedBlock("T");      // 4칸
        stage.AddPredefinedBlock("O");      // 4칸
        stage.AddPredefinedBlock("I");      // 4칸
        stage.AddPredefinedBlock("DOMINO"); // 2칸
        stage.AddPredefinedBlock("TRIPLE"); // 3칸
        // 총 25칸

        return stage;
    }
}
#endif