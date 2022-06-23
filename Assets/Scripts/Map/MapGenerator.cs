using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public MapManager mapManager;

    [SerializeField] private GameObject tile;
    private int gridHeight;
    private int gridWidth;
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private float realTileSize = 1f;

    private Map GenerateTile(int x, int y)
    {
        float rt = tileSize * realTileSize;
        //Map newTile = Instantiate(tile, transform).GetComponent<Map>();
        Map newTile = PoolManager.GetItem<Map>();
        RectTransform rectTransform = newTile.GetComponent<RectTransform>();

        float posX = ((x * rt) + (y * rt)) / 2f;
        float posY = ((x * rt) - (y * rt)) / 1.6f;

        posX -= (gridWidth * rt / 2f) - (rt / 2f);

        newTile.transform.position = new Vector2(posX, posY);
        //rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0f);

        newTile.name = $"{x}, {y}";

        newTile.GetComponent<CanvasGroup>().alpha = 1;

        newTile.InitMap(mapManager);

        GameManager.Instance.mapManager.tiles.Add(new Vector2(x, y), newTile);

        return newTile;
    }

    public void GenerateGrid(int gridHeight, int gridWidth, Action onEndGenerate = null)
    {
        this.gridHeight = gridHeight;
        this.gridWidth = gridWidth;

        for (int x = gridWidth - 1; x >= 0; x--)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GenerateTile(x, y);
            }
        }

        Map startMap = GenerateTile(-1, gridHeight - 1);
        Transform startTrm = startMap.transform;
        RectTransform rectTransform = startTrm.GetComponent<RectTransform>();

        onEndGenerate?.Invoke();
    }

}
