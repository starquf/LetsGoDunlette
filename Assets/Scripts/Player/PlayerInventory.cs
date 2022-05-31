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
    [Header("�Ķ����")]
    public int capacity = 13;

    [Header("���۽� ����� ����")]
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
