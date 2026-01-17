using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StageCreatorHelper : MonoBehaviour
{
    [ContextMenu("Create Stage 1-1 (3x3)")]
    void CreateStage1_1()
    {
        #if UNITY_EDITOR
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "1-1";
        stage.gridWidth = 3;
        stage.gridHeight = 3;
        
        // L자 대칭 (3칸) - 주황색
        BlockShapeData lBlock = new BlockShapeData(2, 2);
        lBlock.shape = new bool[2, 2] {
            { true, true },
            { true, false }
        };
        lBlock.blockColor = new Color(1f, 0.5f, 0f);
        lBlock.SaveColor();
        lBlock.FlattenShape();
        stage.blocks.Add(lBlock);
        
        // L자 대칭 반대 (3칸) - 파란색
        BlockShapeData lBlock2 = new BlockShapeData(2, 2);
        lBlock2.shape = new bool[2, 2] {
            { true, true },
            { false, true }
        };
        lBlock2.blockColor = new Color(0f, 0.3f, 1f);
        lBlock2.SaveColor();
        lBlock2.FlattenShape();
        stage.blocks.Add(lBlock2);
        
        // Triple (3칸) - 초록색
        BlockShapeData triBlock = new BlockShapeData(3, 1);
        triBlock.shape = new bool[3, 1] {
            { true },
            { true },
            { true }
        };
        triBlock.blockColor = new Color(0f, 1f, 0.5f);
        triBlock.SaveColor();
        triBlock.FlattenShape();
        stage.blocks.Add(triBlock);
        
        string path = "Assets/Resources/Stages/Stage_1_1.asset";
        AssetDatabase.CreateAsset(stage, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Stage 1-1 created");
        Selection.activeObject = stage;
        #endif
    }
    
    [ContextMenu("Create Stage 1-2 (4x3)")]
    void CreateStage1_2()
    {
        #if UNITY_EDITOR
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "1-2";
        stage.gridWidth = 4;
        stage.gridHeight = 3;
        
        // U자 (ㄷ자, 5칸) - 보라색
        BlockShapeData uBlock = new BlockShapeData(3, 2);
        uBlock.shape = new bool[3, 2] {
            { true, true },
            { true, false },
            { true, true }
        };
        uBlock.blockColor = new Color(0.5f, 0f, 1f);
        uBlock.SaveColor();
        uBlock.FlattenShape();
        stage.blocks.Add(uBlock);
        
        // L자 대칭 (3칸) - 주황색
        BlockShapeData lBlock = new BlockShapeData(2, 2);
        lBlock.shape = new bool[2, 2] {
            { true, true },
            { true, false }
        };
        lBlock.blockColor = new Color(1f, 0.5f, 0f);
        lBlock.SaveColor();
        lBlock.FlattenShape();
        stage.blocks.Add(lBlock);
        
        // O블록 (4칸) - 노란색
        BlockShapeData oBlock = new BlockShapeData(2, 2);
        oBlock.shape = new bool[2, 2] {
            { true, true },
            { true, true }
        };
        oBlock.blockColor = new Color(1f, 0.92f, 0.016f);
        oBlock.SaveColor();
        oBlock.FlattenShape();
        stage.blocks.Add(oBlock);
        
        string path = "Assets/Resources/Stages/Stage_1_2.asset";
        AssetDatabase.CreateAsset(stage, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Stage 1-2 created");
        Selection.activeObject = stage;
        #endif
    }
    
    [ContextMenu("Create Stage 1-3 (4x4)")]
    void CreateStage1_3()
    {
        #if UNITY_EDITOR
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "1-3";
        stage.gridWidth = 4;
        stage.gridHeight = 4;
        
        // H자 (6칸) - 빨간색
        BlockShapeData hBlock = new BlockShapeData(3, 3);
        hBlock.shape = new bool[3, 3] {
            { true, false, true },
            { true, true, true },
            { true, false, true }
        };
        hBlock.blockColor = new Color(1f, 0f, 0.3f);
        hBlock.SaveColor();
        hBlock.FlattenShape();
        stage.blocks.Add(hBlock);
        
        // 십자가 (5칸) - 노란색
        BlockShapeData crossBlock = new BlockShapeData(3, 3);
        crossBlock.shape = new bool[3, 3] {
            { false, true, false },
            { true, true, true },
            { false, true, false }
        };
        crossBlock.blockColor = new Color(1f, 0.92f, 0.016f);
        crossBlock.SaveColor();
        crossBlock.FlattenShape();
        stage.blocks.Add(crossBlock);
        
        // U자 작은거 (3칸) - 하늘색
        BlockShapeData uSmall = new BlockShapeData(2, 2);
        uSmall.shape = new bool[2, 2] {
            { true, true },
            { false, true }
        };
        uSmall.blockColor = new Color(0f, 0.9f, 1f);
        uSmall.SaveColor();
        uSmall.FlattenShape();
        stage.blocks.Add(uSmall);
        
        // Domino (2칸) - 핑크색
        BlockShapeData domi = new BlockShapeData(2, 1);
        domi.shape = new bool[2, 1] {
            { true },
            { true }
        };
        domi.blockColor = new Color(1f, 0.4f, 0.7f);
        domi.SaveColor();
        domi.FlattenShape();
        stage.blocks.Add(domi);
        
        string path = "Assets/Resources/Stages/Stage_1_3.asset";
        AssetDatabase.CreateAsset(stage, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Stage 1-3 created");
        Selection.activeObject = stage;
        #endif
    }
    
    [ContextMenu("Create Stage 1-4 (5x4)")]
    void CreateStage1_4()
    {
        #if UNITY_EDITOR
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "1-4";
        stage.gridWidth = 5;
        stage.gridHeight = 4;
        
        // H자 (6칸) - 보라색
        BlockShapeData hBlock = new BlockShapeData(3, 3);
        hBlock.shape = new bool[3, 3] {
            { true, false, true },
            { true, true, true },
            { true, false, true }
        };
        hBlock.blockColor = new Color(0.5f, 0f, 1f);
        hBlock.SaveColor();
        hBlock.FlattenShape();
        stage.blocks.Add(hBlock);
        
        // 직사각형 (6칸) - 주황색
        BlockShapeData rect = new BlockShapeData(2, 3);
        rect.shape = new bool[2, 3] {
            { true, true, true },
            { true, true, true }
        };
        rect.blockColor = new Color(1f, 0.5f, 0f);
        rect.SaveColor();
        rect.FlattenShape();
        stage.blocks.Add(rect);
        
        // U자 (5칸) - 빨간색
        BlockShapeData uBlock = new BlockShapeData(3, 2);
        uBlock.shape = new bool[3, 2] {
            { true, true },
            { true, false },
            { true, true }
        };
        uBlock.blockColor = new Color(1f, 0f, 0.3f);
        uBlock.SaveColor();
        uBlock.FlattenShape();
        stage.blocks.Add(uBlock);
        
        // Triple (3칸) - 초록색
        BlockShapeData tri = new BlockShapeData(3, 1);
        tri.shape = new bool[3, 1] {
            { true },
            { true },
            { true }
        };
        tri.blockColor = new Color(0f, 1f, 0.5f);
        tri.SaveColor();
        tri.FlattenShape();
        stage.blocks.Add(tri);
        
        string path = "Assets/Resources/Stages/Stage_1_4.asset";
        AssetDatabase.CreateAsset(stage, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Stage 1-4 created");
        Selection.activeObject = stage;
        #endif
    }
    
    [ContextMenu("Create Stage 1-5 (5x5)")]
    void CreateStage1_5()
    {
        #if UNITY_EDITOR
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "1-5";
        stage.gridWidth = 5;
        stage.gridHeight = 5;
        
        // H자 큰거 (10칸) - 보라색
        BlockShapeData hBig = new BlockShapeData(4, 3);
        hBig.shape = new bool[4, 3] {
            { true, false, true },
            { true, true, true },
            { true, true, true },
            { true, false, true }
        };
        hBig.blockColor = new Color(0.5f, 0f, 1f);
        hBig.SaveColor();
        hBig.FlattenShape();
        stage.blocks.Add(hBig);
        
        // 십자가 (5칸) - 노란색
        BlockShapeData cross = new BlockShapeData(3, 3);
        cross.shape = new bool[3, 3] {
            { false, true, false },
            { true, true, true },
            { false, true, false }
        };
        cross.blockColor = new Color(1f, 0.92f, 0.016f);
        cross.SaveColor();
        cross.FlattenShape();
        stage.blocks.Add(cross);
        
        // L자 대칭 (3칸) - 주황색
        BlockShapeData l1 = new BlockShapeData(2, 2);
        l1.shape = new bool[2, 2] {
            { true, true },
            { true, false }
        };
        l1.blockColor = new Color(1f, 0.5f, 0f);
        l1.SaveColor();
        l1.FlattenShape();
        stage.blocks.Add(l1);
        
        // L자 반대 (3칸) - 하늘색
        BlockShapeData l2 = new BlockShapeData(2, 2);
        l2.shape = new bool[2, 2] {
            { true, true },
            { false, true }
        };
        l2.blockColor = new Color(0f, 0.9f, 1f);
        l2.SaveColor();
        l2.FlattenShape();
        stage.blocks.Add(l2);
        
        // O블록 (4칸) - 초록색
        BlockShapeData oBlock = new BlockShapeData(2, 2);
        oBlock.shape = new bool[2, 2] {
            { true, true },
            { true, true }
        };
        oBlock.blockColor = new Color(0f, 1f, 0.5f);
        oBlock.SaveColor();
        oBlock.FlattenShape();
        stage.blocks.Add(oBlock);
        
        string path = "Assets/Resources/Stages/Stage_1_5.asset";
        AssetDatabase.CreateAsset(stage, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Stage 1-5 created");
        Selection.activeObject = stage;
        #endif
    }
}