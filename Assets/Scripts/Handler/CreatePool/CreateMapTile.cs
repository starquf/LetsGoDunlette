using UnityEngine;

public class CreateMapTile : MonoBehaviour
{
    public Transform mapGeneratorTrm = null;
    [Header("�� Ÿ��")]
    public GameObject mapTile = null;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        PoolManager.CreatePool<Map>(mapTile, mapGeneratorTrm, 25);
    }
}
