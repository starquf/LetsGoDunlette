using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] GameObject tile;
    [SerializeField] int gridHeight = 10;
    [SerializeField] int gridWidth = 10;
    [SerializeField] float tileSize = 1f;
    [SerializeField] float realTileSize = 1f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    Map GenerateTile(int x, int y)
    {
        float rt = tileSize * realTileSize;
        Map newTile = Instantiate(tile, transform).GetComponent<Map>();
        RectTransform rectTransform = newTile.GetComponent<RectTransform>();

        float posX = (x * rt + y * rt) / 2f;
        float posY = (x * rt - y * rt) / 1.6f;

        posX -= (gridWidth * rt / 2f) - (rt / 2f);

        newTile.transform.position = new Vector2(posX, posY);
        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0f);

        GameManager.Instance.mapManager.tiles.Add(newTile);

        return newTile;
    }

    public void GenerateGrid()
    {
        for (int x = gridWidth - 1; x >= 0; x--)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GenerateTile(x, y);
            }
        }

        Transform startTrm = GenerateTile(0, 4).transform;
        RectTransform rectTransform = startTrm.GetComponent<RectTransform>();

        startTrm.position = new Vector2(startTrm.position.x, startTrm.position.y - (tileSize * realTileSize));
        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0f);

        GameManager.Instance.mapManager.SetPlayerPos(startTrm);
    }

}
