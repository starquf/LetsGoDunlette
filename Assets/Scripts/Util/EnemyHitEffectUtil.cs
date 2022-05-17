using DG.Tweening;
using UnityEngine;

public class EnemyHitEffectUtil : MonoBehaviour
{
    public static void Typhoon(GameObject go)
    {
        go.transform.DOPunchRotation(new Vector3(0, 180, 0), 1, 20, 0.1f);
    }
}
