using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectUIHandler : MonoBehaviour
{
    public Button nextBtn;
    public Button prevBtn;
    public Button selectBtn;

    public List<GameObject> playerPrefabList = new List<GameObject>();
    public List<Image> playerImgList = new List<Image>();
    public Image playerImg;
    
    private List<PlayerInfo> playerList = new List<PlayerInfo>();

    private int currentIdx = 0;

    public PieceDicHandler pieceDicHandler;

    private void Awake()
    {
        for (int i = 0; i < playerPrefabList.Count; i++)
        {
            PlayerInfo player = Instantiate(playerPrefabList[i]).GetComponent<PlayerInfo>();

            playerList.Add(player);
        }

        ChangePlayer();
        ResetSelect();
    }

    private void Start()
    {
        nextBtn.onClick.AddListener(() =>
        {
            MoveNext();
            ResetSelect();
        });

        prevBtn.onClick.AddListener(() =>
        {
            MovePrev();
            ResetSelect();
        });

        selectBtn.onClick.AddListener(() =>
        {
            ChangePlayer();
            ResetSelect();
        });
    }

    private void MoveNext()
    {
        if (currentIdx >= playerList.Count - 1)
            return;

        currentIdx++;
    }

    private void MovePrev()
    {
        if (currentIdx <= 0)
            return;

        currentIdx--;
    }

    private void ResetSelect()
    {
        TextMeshProUGUI text = selectBtn.GetComponentInChildren<TextMeshProUGUI>();

        text.text = GameManager.Instance.currentPlayer.Equals(playerList[currentIdx].characterName) ?
            "º±≈√µ " : "º±≈√";

        playerImg.sprite = playerList[currentIdx].playerImg;
    }

    private void ChangePlayer()
    {
        PlayerInfo currentPlayer = playerList[currentIdx];

        for (int i = 0; i < playerImgList.Count; i++)
        {
            playerImgList[i].sprite = currentPlayer.playerImg;
        }

        GameManager.Instance.currentPlayer = currentPlayer.characterName;
        pieceDicHandler.ChangePlayer(currentPlayer.GetComponent<PlayerInventory>());
    }
}
