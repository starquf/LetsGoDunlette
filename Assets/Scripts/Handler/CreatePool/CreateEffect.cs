using UnityEngine;

public class CreateEffect : MonoBehaviour
{
    [Header("����Ʈ ������")]
    public GameObject effectObj;

    [Header("UI ������")]
    public GameObject enemyIndicator;
    public GameObject logLine;
    public GameObject iconInfoPrefab;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        PoolManager.CreatePool<EffectObj>(effectObj, transform, 10);

        if (enemyIndicator != null)
        {
            PoolManager.CreatePool<EnemyIndicatorText>(enemyIndicator, transform, 10);
        }

        PoolManager.CreatePool<LogLine>(logLine, transform, 10);

        PoolManager.CreatePool<IconInfo>(iconInfoPrefab, transform, 3);
    }
}
