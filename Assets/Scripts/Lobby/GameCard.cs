using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GameCard : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    [SerializeField] private Image background;
    [SerializeField] private Image thumbnail;
    [SerializeField] private TextMeshProUGUI gameNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject lockOverlay;

    private GameInfo gameInfo;

    public void Initialize(GameInfo info)
    {
        gameInfo = info;

        if (gameNameText != null) gameNameText.text = info.gameName;
        else Debug.LogWarning($"[GameCard] gameNameText is not assigned on {name}");

        if (descriptionText != null) descriptionText.text = info.description;
        else Debug.LogWarning($"[GameCard] descriptionText is not assigned on {name}");

        if (background != null) background.color = info.accentColor;
        else Debug.LogWarning($"[GameCard] background is not assigned on {name}");

        if (thumbnail != null && info.thumbnail != null)
            thumbnail.sprite = info.thumbnail;

        if (playButton != null)
        {
            bool locked = !info.isUnlocked;
            playButton.interactable = !locked;
            if (lockOverlay != null)
                lockOverlay.SetActive(locked);
        }
        else Debug.LogWarning($"[GameCard] playButton is not assigned on {name}");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"[GameCard] OnPointerDown at position={eventData.position}");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"[GameCard] OnPointerClick at position={eventData.position}");
    }

    public void OnPlayClicked()
    {
        Debug.Log($"[GameCard] OnPlayClicked called. gameInfo={gameInfo}, isUnlocked={gameInfo?.isUnlocked}, sceneName={gameInfo?.sceneName}");
        if (gameInfo == null)
        {
            Debug.LogError("[GameCard] gameInfo is null!");
            return;
        }
        if (!gameInfo.isUnlocked)
        {
            Debug.LogWarning("[GameCard] Game is locked!");
            return;
        }
        Debug.Log($"[GameCard] Loading scene: {gameInfo.sceneName}");
        SceneLoader.LoadGame(gameInfo.sceneName);
    }
}
