using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saro : MonoBehaviour
{
    void Start()
    {
        GetComponent<EnemyHealth>().onEnemyDie = OnDie;
    }

    private void OnDie()
    {

    }
}
