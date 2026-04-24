using UnityEngine;

/// <summary>
/// In-game scene UI (back to lobby button, etc.)
/// </summary>
public class GameSceneUI : MonoBehaviour
{
    public void OnBackToLobbyClicked()
    {
        SceneLoader.LoadLobby();
    }
}
