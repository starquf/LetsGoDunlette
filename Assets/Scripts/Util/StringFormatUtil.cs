using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringFormatUtil : MonoBehaviour
{
    private static readonly string enemyAttackString = "�÷��̾�� {0}�� ���ظ� ������.";
    public static string GetEnemyAttackString(int value)
    {
        return string.Format(enemyAttackString, value);
    }
}
