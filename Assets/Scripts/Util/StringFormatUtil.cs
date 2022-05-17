using UnityEngine;

public class StringFormatUtil : MonoBehaviour
{
    private static readonly string enemyAttackString = "플레이어에게 {0}의 피해를 입힌다.";
    public static string GetEnemyAttackString(int value)
    {
        return string.Format(enemyAttackString, value);
    }

}
