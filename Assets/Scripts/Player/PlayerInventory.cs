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
    private int capacity;

    [Header("���۽� ����� ����")]
    public List<Deck> decks = new List<Deck>();

    private void Awake()
    {
        isPlayerInven = true;
    }

    public bool IsInventoryFull()
    {
        capacity = GameManager.Instance.GetPlayer().MaxPieceCount;
        return capacity <= skills.Count;
    }
}
