using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public MapManager mapManager;

    [SerializeField] GameObject tile;
    [SerializeField] int gridHeight = 10;
    public int gridWidth = 10;
    [SerializeField] float tileSize = 1f;
    [SerializeField] float realTileSize = 1f;

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

        newTile.name = $"{x}, {y}";

        newTile.InitMap(mapManager);

        GameManager.Instance.mapManager.tiles.Add(new Vector2(x, y), newTile);

        return newTile;
    }

    public void GenerateGrid(Action onEndGenerate = null)
    {
        for (int x = gridWidth - 1; x >= 0; x--)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GenerateTile(x, y);
            }
        }

        Map startMap = GenerateTile(-1, gridWidth-1);
        Transform startTrm = startMap.transform;
        RectTransform rectTransform = startTrm.GetComponent<RectTransform>();

        onEndGenerate?.Invoke();
    }

}
