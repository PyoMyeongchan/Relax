using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 스테이지 생성 헬퍼 (에디터 전용)
/// </summary>
public class StageCreatorHelper : MonoBehaviour
{
    [ContextMenu("Create Stage 1-1 (4x3)")]
    void CreateStage1_1()
    {
        #if UNITY_EDITOR
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "1-1";
        stage.gridWidth = 4;
        stage.gridHeight = 3;
        
        // O 블록 (2x2 = 4칸)
        BlockShapeData oBlock = new BlockShapeData(2, 2);
        oBlock.shape = new bool[2, 2] {
            { true, true },
            { true, true }
        };
        oBlock.FlattenShape();
        stage.blocks.Add(oBlock);
        
        // I 블록 (4x1 = 4칸)
        BlockShapeData iBlock = new BlockShapeData(4, 1);
        iBlock.shape = new bool[4, 1] {
            { true },
            { true },
            { true },
            { true }
        };
        iBlock.FlattenShape();
        stage.blocks.Add(iBlock);
        
        // Domino 블록 (2x1 = 2칸)
        BlockShapeData domiBlock = new BlockShapeData(2, 1);
        domiBlock.shape = new bool[2, 1] {
            { true },
            { true }
        };
        domiBlock.FlattenShape();
        stage.blocks.Add(domiBlock);
        
        // Domino 블록 (2x1 = 2칸)
        BlockShapeData domi2Block = new BlockShapeData(2, 1);
        domi2Block.shape = new bool[2, 1] {
            { true },
            { true }
        };
        domi2Block.FlattenShape();
        stage.blocks.Add(domi2Block);
        
        // 총 12칸!
        
        // Resources/Stages 폴더에 저장
        string path = "Assets/Resources/Stages/Stage_1_1.asset";
        AssetDatabase.CreateAsset(stage, path);
        AssetDatabase.SaveAssets();
        
        Debug.Log($"Stage created at: {path}");
        Debug.Log($"Total blocks: {stage.blocks.Count}");
        
        // 검증
        foreach (var block in stage.blocks)
        {
            Debug.Log($"Block: {block.width}x{block.height}, cells: {block.GetFilledCellCount()}");
        }
        
        // 선택
        Selection.activeObject = stage;
        #endif
    }
    
    [ContextMenu("Create Stage 1-2 (4x4)")]
    void CreateStage1_2()
    {
        #if UNITY_EDITOR
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "1-2";
        stage.gridWidth = 4;
        stage.gridHeight = 4;
        
        // T 블록 (3x2 = 4칸)
        BlockShapeData t1 = new BlockShapeData(3, 2);
        t1.shape = new bool[3, 2] {
            { false, true },
            { true, true },
            { false, true }
        };
        t1.FlattenShape();
        stage.blocks.Add(t1);
        
        // T 블록 (3x2 = 4칸)
        BlockShapeData t2 = new BlockShapeData(3, 2);
        t2.shape = new bool[3, 2] {
            { false, true },
            { true, true },
            { false, true }
        };
        t2.FlattenShape();
        stage.blocks.Add(t2);
        
        // L 블록 (3x2 = 4칸)
        BlockShapeData l1 = new BlockShapeData(3, 2);
        l1.shape = new bool[3, 2] {
            { true, false },
            { true, false },
            { true, true }
        };
        l1.FlattenShape();
        stage.blocks.Add(l1);
        
        // I 블록 (4x1 = 4칸)
        BlockShapeData i1 = new BlockShapeData(4, 1);
        i1.shape = new bool[4, 1] { { true }, { true }, { true }, { true } };
        i1.FlattenShape();
        stage.blocks.Add(i1);
        
        // 총 16칸! (T x2, L x1, I x1)
        
        string path = "Assets/Resources/Stages/Stage_1_2.asset";
        AssetDatabase.CreateAsset(stage, path);
        AssetDatabase.SaveAssets();
        
        Debug.Log($"Stage 1-2 created: {stage.blocks.Count} blocks, total cells: {stage.GetTotalBlockCells()}");
        Selection.activeObject = stage;
        #endif
    }
    
    [ContextMenu("Create Stage 1-3 (5x5)")]
    void CreateStage1_3()
    {
        #if UNITY_EDITOR
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "1-3";
        stage.gridWidth = 5;
        stage.gridHeight = 5;
        
        // L 블록 (3x2 = 4칸)
        BlockShapeData l1 = new BlockShapeData(3, 2);
        l1.shape = new bool[3, 2] {
            { true, false },
            { true, false },
            { true, true }
        };
        l1.FlattenShape();
        stage.blocks.Add(l1);
        
        // J 블록 (3x2 = 4칸) - L블록 반대!
        BlockShapeData j1 = new BlockShapeData(3, 2);
        j1.shape = new bool[3, 2] {
            { false, true },
            { false, true },
            { true, true }
        };
        j1.FlattenShape();
        stage.blocks.Add(j1);
        
        // T 블록 (3x2 = 4칸)
        BlockShapeData t1 = new BlockShapeData(3, 2);
        t1.shape = new bool[3, 2] {
            { false, true },
            { true, true },
            { false, true }
        };
        t1.FlattenShape();
        stage.blocks.Add(t1);
        
        // T 블록 (3x2 = 4칸)
        BlockShapeData t2 = new BlockShapeData(3, 2);
        t2.shape = new bool[3, 2] {
            { false, true },
            { true, true },
            { false, true }
        };
        t2.FlattenShape();
        stage.blocks.Add(t2);
        
        // Z 블록 (3x2 = 4칸)
        BlockShapeData z1 = new BlockShapeData(3, 2);
        z1.shape = new bool[3, 2] {
            { true, true },
            { false, true },
            { false, true }
        };
        z1.FlattenShape();
        stage.blocks.Add(z1);
        
        // Triple (3x1 = 3칸)
        BlockShapeData tri = new BlockShapeData(3, 1);
        tri.shape = new bool[3, 1] { { true }, { true }, { true } };
        tri.FlattenShape();
        stage.blocks.Add(tri);
        
        // Domino (2x1 = 2칸)
        BlockShapeData domi = new BlockShapeData(2, 1);
        domi.shape = new bool[2, 1] { { true }, { true } };
        domi.FlattenShape();
        stage.blocks.Add(domi);
        
        // 총 25칸! (L x2, T x2, Z, Triple, Domino)
        
        string path = "Assets/Resources/Stages/Stage_1_3.asset";
        AssetDatabase.CreateAsset(stage, path);
        AssetDatabase.SaveAssets();
        
        Debug.Log($"Stage 1-3 created: {stage.blocks.Count} blocks, total cells: {stage.GetTotalBlockCells()}");
        Selection.activeObject = stage;
        #endif
    }
}