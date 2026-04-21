using UnityEngine;
using UnityEditor;

public class LobbySetupHelper
{
    [MenuItem("Relax/Setup/Save GameCard Prefab")]
    static void SaveGameCardPrefab()
    {
        GameObject template = GameObject.Find("GameCardTemplate");
        if (template == null)
        {
            Debug.LogError("GameCardTemplate not found in scene!");
            return;
        }

        string path = "Assets/Prefab/GameCard.prefab";
        bool success;
        PrefabUtility.SaveAsPrefabAssetAndConnect(template, path, InteractionMode.AutomatedAction, out success);

        if (success)
        {
            Debug.Log("GameCard prefab saved at " + path);
            AssetDatabase.Refresh();

            // LobbyManager에 프리팹 자동 연결
            LobbyManager lobbyManager = Object.FindObjectOfType<LobbyManager>();
            if (lobbyManager != null)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                SerializedObject so = new SerializedObject(lobbyManager);
                so.FindProperty("gameCardPrefab").objectReferenceValue = prefab;
                so.ApplyModifiedProperties();
                Debug.Log("GameCard prefab assigned to LobbyManager");
            }
        }
        else
        {
            Debug.LogError("Failed to save GameCard prefab!");
        }
    }

    [MenuItem("Relax/Setup/Create Tetris GameInfo Asset")]
    static void CreateTetrisGameInfo()
    {
        GameInfo info = ScriptableObject.CreateInstance<GameInfo>();
        info.gameName = "블록 채우기";
        info.description = "다양한 모양의 블록을 격자에 완벽하게 채워보세요.";
        info.sceneName = "GameScene_Tetris";
        info.accentColor = new Color(0.2f, 0.6f, 1f);
        info.isUnlocked = true;

        string path = "Assets/ScriptObject/Games/TetrisGame.asset";
        System.IO.Directory.CreateDirectory("Assets/ScriptObject/Games");
        AssetDatabase.CreateAsset(info, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("TetrisGame.asset created at " + path);
        Selection.activeObject = info;
    }
}
