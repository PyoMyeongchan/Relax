using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StageCreatorHelper : MonoBehaviour
{
    // Stage 1-1 (3x3 = 9칸) - 튜토리얼
    // ㄴ자 + ㄱ자 + ㅡ자 = 9칸 ✓
    [ContextMenu("Create Stage 1-1 (3x3)")]
    void CreateStage1_1()
    {
        #if UNITY_EDITOR
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "1-1";
        stage.gridWidth = 3;
        stage.gridHeight = 3;
        
        // ㄴ자 - 주황색
        // ■■
        // ■
        BlockShapeData lBlock = new BlockShapeData(2, 2);
        lBlock.shape = new bool[2, 2] {
            { true, true },
            { true, false }
        };
        lBlock.blockColor = new Color(1f, 0.5f, 0f);
        lBlock.SaveColor();
        lBlock.FlattenShape();
        stage.blocks.Add(lBlock);
        
        // ㄱ자 - 파란색
        //  ■
        // ■■
        BlockShapeData gBlock = new BlockShapeData(2, 2);
        gBlock.shape = new bool[2, 2] {
            { false, true },
            { true, true }
        };
        gBlock.blockColor = new Color(0f, 0.5f, 1f);
        gBlock.SaveColor();
        gBlock.FlattenShape();
        stage.blocks.Add(gBlock);
        
        // ㅡ자 (가로 일자) - 초록색
        // ■■■
        BlockShapeData lineBlock = new BlockShapeData(1, 3);
        lineBlock.shape = new bool[1, 3] {
            { true, true, true }
        };
        lineBlock.blockColor = new Color(0f, 1f, 0.5f);
        lineBlock.SaveColor();
        lineBlock.FlattenShape();
        stage.blocks.Add(lineBlock);
        
        string path = "Assets/Resources/Stages/Stage_1_1.asset";
        AssetDatabase.CreateAsset(stage, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Stage 1-1 created: ㄴ(3) + ㄱ(3) + ㅡ(3) = 9 cells ✓");
        Selection.activeObject = stage;
        #endif
    }
    
    // Stage 1-2 (4x3 = 12칸)
    // ㄴ자(3) + ㄱ자(3) + ㅡ자(3) + ㅣ자(3) = 12칸 ✓
    [ContextMenu("Create Stage 1-2 (4x3)")]
    void CreateStage1_2()
    {
        #if UNITY_EDITOR
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "1-2";
        stage.gridWidth = 4;
        stage.gridHeight = 3;
        
        // ㄴ자 - 노란색
        // ■■
        // ■
        BlockShapeData lBlock = new BlockShapeData(2, 2);
        lBlock.shape = new bool[2, 2] {
            { true, true },
            { true, false }
        };
        lBlock.blockColor = new Color(1f, 0.92f, 0.016f);
        lBlock.SaveColor();
        lBlock.FlattenShape();
        stage.blocks.Add(lBlock);
        
        // ㄱ자 - 보라색
        //  ■
        // ■■
        BlockShapeData gBlock = new BlockShapeData(2, 2);
        gBlock.shape = new bool[2, 2] {
            { false, true },
            { true, true }
        };
        gBlock.blockColor = new Color(0.5f, 0f, 1f);
        gBlock.SaveColor();
        gBlock.FlattenShape();
        stage.blocks.Add(gBlock);
        
        // ㅡ자 (가로 일자) - 빨간색
        // ■■■
        BlockShapeData hLine = new BlockShapeData(1, 3);
        hLine.shape = new bool[1, 3] {
            { true, true, true }
        };
        hLine.blockColor = new Color(1f, 0f, 0.3f);
        hLine.SaveColor();
        hLine.FlattenShape();
        stage.blocks.Add(hLine);
        
        // ㅣ자 (세로 일자) - 초록색
        // ■
        // ■
        // ■
        BlockShapeData vLine = new BlockShapeData(3, 1);
        vLine.shape = new bool[3, 1] {
            { true },
            { true },
            { true }
        };
        vLine.blockColor = new Color(0f, 1f, 0.5f);
        vLine.SaveColor();
        vLine.FlattenShape();
        stage.blocks.Add(vLine);
        
        string path = "Assets/Resources/Stages/Stage_1_2.asset";
        AssetDatabase.CreateAsset(stage, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Stage 1-2 created: ㄴ(3) + ㄱ(3) + ㅡ(3) + ㅣ(3) = 12 cells ✓");
        Selection.activeObject = stage;
        #endif
    }
    
    // Stage 1-3 (4x4 = 16칸)
    // Z자반대(4) + ㄴ자세로(4) + L자(4) + Z자(4) = 16칸 ✓
    [ContextMenu("Create Stage 1-3 (4x4)")]
    void CreateStage1_3()
    {
        #if UNITY_EDITOR
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "1-3";
        stage.gridWidth = 4;
        stage.gridHeight = 4;
        
        // Z자 반대 (노랑) - 왼쪽으로 내려감
        //  ■■
        // ■■
        BlockShapeData zRev = new BlockShapeData(2, 3);
        zRev.shape = new bool[2, 3] {
            { false, true, true },
            { true, true, false }
        };
        zRev.blockColor = new Color(1f, 0.92f, 0.016f);
        zRev.SaveColor();
        zRev.FlattenShape();
        stage.blocks.Add(zRev);
        
        // ㄴ자 세로 (분홍)
        // ■■
        //  ■
        //  ■
        BlockShapeData lVert = new BlockShapeData(3, 2);
        lVert.shape = new bool[3, 2] {
            { true, true },
            { false, true },
            { false, true }
        };
        lVert.blockColor = new Color(1f, 0.4f, 0.7f);
        lVert.SaveColor();
        lVert.FlattenShape();
        stage.blocks.Add(lVert);
        
        // L자 (파랑)
        // ■
        // ■
        // ■■
        BlockShapeData lBlock = new BlockShapeData(3, 2);
        lBlock.shape = new bool[3, 2] {
            { true, false },
            { true, false },
            { true, true }
        };
        lBlock.blockColor = new Color(0f, 0.5f, 1f);
        lBlock.SaveColor();
        lBlock.FlattenShape();
        stage.blocks.Add(lBlock);
        
        // Z자 (빨강) - 오른쪽으로 내려감
        // ■■
        //  ■■
        BlockShapeData zBlock = new BlockShapeData(2, 3);
        zBlock.shape = new bool[2, 3] {
            { true, true, false },
            { false, true, true }
        };
        zBlock.blockColor = new Color(1f, 0f, 0.3f);
        zBlock.SaveColor();
        zBlock.FlattenShape();
        stage.blocks.Add(zBlock);
        
        string path = "Assets/Resources/Stages/Stage_1_3.asset";
        AssetDatabase.CreateAsset(stage, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Stage 1-3 created: Z반대(4) + ㄴ세로(4) + L(4) + Z(4) = 16 cells ✓");
        Selection.activeObject = stage;
        #endif
    }
    
    // Stage 1-4 (5x4 = 20칸)
    // L긴거(5) + ㄴ긴거(5) + O블록(4) + ㄴ자(3) + ㄱ자(3) = 20칸 ✓
    [ContextMenu("Create Stage 1-4 (5x4)")]
    void CreateStage1_4()
    {
        #if UNITY_EDITOR
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "1-4";
        stage.gridWidth = 5;
        stage.gridHeight = 4;
        
        // L긴거 (보라색)
        // ■
        // ■
        // ■
        // ■■
        BlockShapeData lLong = new BlockShapeData(4, 2);
        lLong.shape = new bool[4, 2] {
            { true, false },
            { true, false },
            { true, false },
            { true, true }
        };
        lLong.blockColor = new Color(0.5f, 0f, 1f);
        lLong.SaveColor();
        lLong.FlattenShape();
        stage.blocks.Add(lLong);
        
        // ㄴ긴거 (주황색)
        //  ■
        //  ■
        //  ■
        // ■■
        BlockShapeData nLong = new BlockShapeData(4, 2);
        nLong.shape = new bool[4, 2] {
            { false, true },
            { false, true },
            { false, true },
            { true, true }
        };
        nLong.blockColor = new Color(1f, 0.5f, 0f);
        nLong.SaveColor();
        nLong.FlattenShape();
        stage.blocks.Add(nLong);
        
        // O블록 (노란색)
        // ■■
        // ■■
        BlockShapeData oBlock = new BlockShapeData(2, 2);
        oBlock.shape = new bool[2, 2] {
            { true, true },
            { true, true }
        };
        oBlock.blockColor = new Color(1f, 0.92f, 0.016f);
        oBlock.SaveColor();
        oBlock.FlattenShape();
        stage.blocks.Add(oBlock);
        
        // ㄴ자 (초록색)
        // ■■
        // ■
        BlockShapeData lBlock = new BlockShapeData(2, 2);
        lBlock.shape = new bool[2, 2] {
            { true, true },
            { true, false }
        };
        lBlock.blockColor = new Color(0f, 1f, 0.5f);
        lBlock.SaveColor();
        lBlock.FlattenShape();
        stage.blocks.Add(lBlock);
        
        // ㄱ자 (파란색)
        //  ■
        // ■■
        BlockShapeData gBlock = new BlockShapeData(2, 2);
        gBlock.shape = new bool[2, 2] {
            { false, true },
            { true, true }
        };
        gBlock.blockColor = new Color(0f, 0.5f, 1f);
        gBlock.SaveColor();
        gBlock.FlattenShape();
        stage.blocks.Add(gBlock);
        
        string path = "Assets/Resources/Stages/Stage_1_4.asset";
        AssetDatabase.CreateAsset(stage, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Stage 1-4 created: L긴(5) + ㄴ긴(5) + O(4) + ㄴ(3) + ㄱ(3) = 20 cells ✓");
        Selection.activeObject = stage;
        #endif
    }
    
    // Stage 1-5 (5x5 = 25칸)
    // T자(5) + L긴거(5) + ㄴ긴거(5) + Z자(4) + ㄴ자(3) + ㄱ자(3) = 25칸 ✓
    [ContextMenu("Create Stage 1-5 (5x5)")]
    void CreateStage1_5()
    {
        #if UNITY_EDITOR
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "1-5";
        stage.gridWidth = 5;
        stage.gridHeight = 5;
        
        // T자 (노란색)
        //  ■
        // ■■■
        //  ■
        BlockShapeData tBlock = new BlockShapeData(3, 3);
        tBlock.shape = new bool[3, 3] {
            { false, true, false },
            { true, true, true },
            { false, true, false }
        };
        tBlock.blockColor = new Color(1f, 0.92f, 0.016f);
        tBlock.SaveColor();
        tBlock.FlattenShape();
        stage.blocks.Add(tBlock);
        
        // L긴거 (보라색)
        // ■
        // ■
        // ■
        // ■■
        BlockShapeData lLong = new BlockShapeData(4, 2);
        lLong.shape = new bool[4, 2] {
            { true, false },
            { true, false },
            { true, false },
            { true, true }
        };
        lLong.blockColor = new Color(0.5f, 0f, 1f);
        lLong.SaveColor();
        lLong.FlattenShape();
        stage.blocks.Add(lLong);
        
        // ㄴ긴거 (주황색)
        //  ■
        //  ■
        //  ■
        // ■■
        BlockShapeData nLong = new BlockShapeData(4, 2);
        nLong.shape = new bool[4, 2] {
            { false, true },
            { false, true },
            { false, true },
            { true, true }
        };
        nLong.blockColor = new Color(1f, 0.5f, 0f);
        nLong.SaveColor();
        nLong.FlattenShape();
        stage.blocks.Add(nLong);
        
        // Z자 (빨간색)
        // ■■
        //  ■■
        BlockShapeData zBlock = new BlockShapeData(2, 3);
        zBlock.shape = new bool[2, 3] {
            { true, true, false },
            { false, true, true }
        };
        zBlock.blockColor = new Color(1f, 0f, 0.3f);
        zBlock.SaveColor();
        zBlock.FlattenShape();
        stage.blocks.Add(zBlock);
        
        // ㄴ자 (초록색)
        // ■■
        // ■
        BlockShapeData lBlock = new BlockShapeData(2, 2);
        lBlock.shape = new bool[2, 2] {
            { true, true },
            { true, false }
        };
        lBlock.blockColor = new Color(0f, 1f, 0.5f);
        lBlock.SaveColor();
        lBlock.FlattenShape();
        stage.blocks.Add(lBlock);
        
        // ㄱ자 (파란색)
        //  ■
        // ■■
        BlockShapeData gBlock = new BlockShapeData(2, 2);
        gBlock.shape = new bool[2, 2] {
            { false, true },
            { true, true }
        };
        gBlock.blockColor = new Color(0f, 0.5f, 1f);
        gBlock.SaveColor();
        gBlock.FlattenShape();
        stage.blocks.Add(gBlock);
        
        string path = "Assets/Resources/Stages/Stage_1_5.asset";
        AssetDatabase.CreateAsset(stage, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Stage 1-5 created: T(5) + L긴(5) + ㄴ긴(5) + Z(4) + ㄴ(3) + ㄱ(3) = 25 cells ✓");
        Selection.activeObject = stage;
        #endif
    }
    
    // Stage 1-6 (6x4 = 24칸)
    // ㅡ자5칸(5) + L긴거(5) + T자(5) + Z자(4) + ㄴ자(3) + 도미노(2) = 24칸 ✓
    [ContextMenu("Create Stage 1-6 (6x4)")]
    void CreateStage1_6()
    {
        #if UNITY_EDITOR
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "1-6";
        stage.gridWidth = 6;
        stage.gridHeight = 4;
        
        // ㅡ자 5칸 (보라색)
        // ■■■■■
        BlockShapeData hLine5 = new BlockShapeData(1, 5);
        hLine5.shape = new bool[1, 5] {
            { true, true, true, true, true }
        };
        hLine5.blockColor = new Color(0.5f, 0f, 1f);
        hLine5.SaveColor();
        hLine5.FlattenShape();
        stage.blocks.Add(hLine5);
        
        // L긴거 (주황색)
        // ■
        // ■
        // ■
        // ■■
        BlockShapeData lLong = new BlockShapeData(4, 2);
        lLong.shape = new bool[4, 2] {
            { true, false },
            { true, false },
            { true, false },
            { true, true }
        };
        lLong.blockColor = new Color(1f, 0.5f, 0f);
        lLong.SaveColor();
        lLong.FlattenShape();
        stage.blocks.Add(lLong);
        
        // T자 (노란색)
        //  ■
        // ■■■
        //  ■
        BlockShapeData tBlock = new BlockShapeData(3, 3);
        tBlock.shape = new bool[3, 3] {
            { false, true, false },
            { true, true, true },
            { false, true, false }
        };
        tBlock.blockColor = new Color(1f, 0.92f, 0.016f);
        tBlock.SaveColor();
        tBlock.FlattenShape();
        stage.blocks.Add(tBlock);
        
        // Z자 (빨간색)
        // ■■
        //  ■■
        BlockShapeData zBlock = new BlockShapeData(2, 3);
        zBlock.shape = new bool[2, 3] {
            { true, true, false },
            { false, true, true }
        };
        zBlock.blockColor = new Color(1f, 0f, 0.3f);
        zBlock.SaveColor();
        zBlock.FlattenShape();
        stage.blocks.Add(zBlock);
        
        // ㄴ자 (초록색)
        // ■■
        // ■
        BlockShapeData lBlock = new BlockShapeData(2, 2);
        lBlock.shape = new bool[2, 2] {
            { true, true },
            { true, false }
        };
        lBlock.blockColor = new Color(0f, 1f, 0.5f);
        lBlock.SaveColor();
        lBlock.FlattenShape();
        stage.blocks.Add(lBlock);
        
        // 도미노 (하늘색)
        // ■
        // ■
        BlockShapeData domino = new BlockShapeData(2, 1);
        domino.shape = new bool[2, 1] {
            { true },
            { true }
        };
        domino.blockColor = new Color(0f, 0.9f, 1f);
        domino.SaveColor();
        domino.FlattenShape();
        stage.blocks.Add(domino);
        
        string path = "Assets/Resources/Stages/Stage_1_6.asset";
        AssetDatabase.CreateAsset(stage, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Stage 1-6 created: ㅡ5(5) + L긴(5) + T(5) + Z(4) + ㄴ(3) + 도미노(2) = 24 cells ✓");
        Selection.activeObject = stage;
        #endif
    }
    
    // Stage 1-7 (6x5 = 30칸)
    // ㅡ자6칸(6) + L긴거(5) + ㄴ긴거(5) + T자(5) + Z자(4) + ㄴ자(3) + 도미노(2) = 30칸 ✓
    [ContextMenu("Create Stage 1-7 (6x5)")]
    void CreateStage1_7()
    {
        #if UNITY_EDITOR
        StageData stage = ScriptableObject.CreateInstance<StageData>();
        stage.stageName = "1-7";
        stage.gridWidth = 6;
        stage.gridHeight = 5;
        
        // ㅡ자 6칸 (보라색)
        // ■■■■■■
        BlockShapeData hLine6 = new BlockShapeData(1, 6);
        hLine6.shape = new bool[1, 6] {
            { true, true, true, true, true, true }
        };
        hLine6.blockColor = new Color(0.5f, 0f, 1f);
        hLine6.SaveColor();
        hLine6.FlattenShape();
        stage.blocks.Add(hLine6);
        
        // L긴거 (주황색)
        // ■
        // ■
        // ■
        // ■■
        BlockShapeData lLong = new BlockShapeData(4, 2);
        lLong.shape = new bool[4, 2] {
            { true, false },
            { true, false },
            { true, false },
            { true, true }
        };
        lLong.blockColor = new Color(1f, 0.5f, 0f);
        lLong.SaveColor();
        lLong.FlattenShape();
        stage.blocks.Add(lLong);
        
        // ㄴ긴거 (파란색)
        //  ■
        //  ■
        //  ■
        // ■■
        BlockShapeData nLong = new BlockShapeData(4, 2);
        nLong.shape = new bool[4, 2] {
            { false, true },
            { false, true },
            { false, true },
            { true, true }
        };
        nLong.blockColor = new Color(0f, 0.5f, 1f);
        nLong.SaveColor();
        nLong.FlattenShape();
        stage.blocks.Add(nLong);
        
        // T자 (노란색)
        //  ■
        // ■■■
        //  ■
        BlockShapeData tBlock = new BlockShapeData(3, 3);
        tBlock.shape = new bool[3, 3] {
            { false, true, false },
            { true, true, true },
            { false, true, false }
        };
        tBlock.blockColor = new Color(1f, 0.92f, 0.016f);
        tBlock.SaveColor();
        tBlock.FlattenShape();
        stage.blocks.Add(tBlock);
        
        // Z자 (빨간색)
        // ■■
        //  ■■
        BlockShapeData zBlock = new BlockShapeData(2, 3);
        zBlock.shape = new bool[2, 3] {
            { true, true, false },
            { false, true, true }
        };
        zBlock.blockColor = new Color(1f, 0f, 0.3f);
        zBlock.SaveColor();
        zBlock.FlattenShape();
        stage.blocks.Add(zBlock);
        
        // ㄴ자 (초록색)
        // ■■
        // ■
        BlockShapeData lBlock = new BlockShapeData(2, 2);
        lBlock.shape = new bool[2, 2] {
            { true, true },
            { true, false }
        };
        lBlock.blockColor = new Color(0f, 1f, 0.5f);
        lBlock.SaveColor();
        lBlock.FlattenShape();
        stage.blocks.Add(lBlock);
        
        // 도미노 (하늘색)
        // ■
        // ■
        BlockShapeData domino = new BlockShapeData(2, 1);
        domino.shape = new bool[2, 1] {
            { true },
            { true }
        };
        domino.blockColor = new Color(0f, 0.9f, 1f);
        domino.SaveColor();
        domino.FlattenShape();
        stage.blocks.Add(domino);
        
        string path = "Assets/Resources/Stages/Stage_1_7.asset";
        AssetDatabase.CreateAsset(stage, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Stage 1-7 created: ㅡ6(6) + L긴(5) + ㄴ긴(5) + T(5) + Z(4) + ㄴ(3) + 도미노(2) = 30 cells ✓");
        Selection.activeObject = stage;
        #endif
    }
}