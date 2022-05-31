using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck
{
    public string deckName;
    public List<GameObject> skills = new List<GameObject>();
}

public class PlayerInventory : Inventory
{
    [Header("파라매터")]
    public int capacity = 13;

    [Header("시작시 사용할 덱들")]
    public List<Deck> decks = new List<Deck>();

    private void Awake()
    {
        isPlayerInven = true;

        if (GameManager.Instance.deckIdx >= 0 && GameManager.Instance.deckIdx < decks.Count)
        {
            skillPrefabs = decks[GameManager.Instance.deckIdx].skills;
        }
    }

    public bool IsInventoryFull()
    {
        return capacity <= skills.Count;
    }
}
