using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyManager : MonoBehaviour
{
    [Header("Game List")]
    [SerializeField] private List<GameInfo> games;

    [Header("UI")]
    [SerializeField] private Transform cardContainer;
    [SerializeField] private GameObject gameCardPrefab;

    private void Start()
    {
        SpawnCards();
    }

    private void SpawnCards()
    {
        if (cardContainer == null || gameCardPrefab == null) return;

        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);

        foreach (var info in games)
        {
            if (info == null) continue;
            GameObject cardObj = Instantiate(gameCardPrefab, cardContainer);
            GameCard card = cardObj.GetComponent<GameCard>();
            if (card != null)
                card.Initialize(info);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(cardContainer as RectTransform);
    }
}
