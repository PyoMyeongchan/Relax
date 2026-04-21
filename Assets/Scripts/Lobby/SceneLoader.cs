using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public const string LobbyScene = "LobbyScene";

    public static void LoadLobby()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(LobbyScene);
    }

    public static void LoadGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
