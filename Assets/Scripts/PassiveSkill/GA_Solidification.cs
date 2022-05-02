using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GA_Solidification : MonoBehaviour
{
    public int shieldValue;

    // Start is called before the first frame update
    void Start()
    {
        BattleHandler bh = GameManager.Instance.battleHandler;

        Action<Action> onNextTurn = action =>
                              {
                                  if (shieldValue > 0)
                                  {
                                       GameManager.Instance.animHandler.GetAnim(AnimName.M_Shield).SetPosition(transform.position)
                                        .SetScale(1)
                                        .Play(() =>
                                      {
                                          action?.Invoke();

                                      });
                                      gameObject.GetComponent<EnemyHealth>().AddShield(shieldValue);
                                  }
                                  else
                                  {
                                      action?.Invoke();
                                  }
                              };

        NormalEvent eventInfo = new NormalEvent(onNextTurn, EventTime.StartTurn);
        bh.battleEvent.BookEvent(eventInfo);
    }
}
