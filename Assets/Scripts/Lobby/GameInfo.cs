using UnityEngine;

[CreateAssetMenu(fileName = "GameInfo", menuName = "Relax/GameInfo")]
public class GameInfo : ScriptableObject
{
    public string gameName;
    [TextArea(2, 4)]
    public string description;
    public Sprite thumbnail;
    public string sceneName;
    public Color accentColor = new Color(0.2f, 0.6f, 1f);
    public bool isUnlocked = true;
}
