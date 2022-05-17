using UnityEngine;

public class EnemyEffectTrmHandler : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.enemyEffectTrm = transform;
    }
}
