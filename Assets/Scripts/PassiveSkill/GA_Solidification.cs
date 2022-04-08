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
                                      Anim_M_Shield hitEffect = PoolManager.GetItem<Anim_M_Shield>();
                                      hitEffect.transform.position = transform.position;

                                      hitEffect.Play(() =>
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
