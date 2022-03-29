using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillAllButton : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(KillAll);
    }

    private void KillAll()
    {
        GameManager.Instance.battleHandler.BattleForceEnd();
        GameManager.Instance.battleHandler.CheckBattleEnd();
        GameManager.Instance.battleHandler.mainRullet.StopForceRullet();
    }

}
