using UnityEngine;

/// <summary>
/// 게임 씬 내 UI (로비 복귀 버튼 등)
/// </summary>
public class GameSceneUI : MonoBehaviour
{
    public void OnBackToLobbyClicked()
    {
        SceneLoader.LoadLobby();
    }
}
