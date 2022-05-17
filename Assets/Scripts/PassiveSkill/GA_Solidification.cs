using System;
using UnityEngine;

public class GA_Solidification : MonoBehaviour
{
    public int shieldValue;
    private BattleHandler bh;

    // Start is called before the first frame update
    private void Start()
    {
        bh = GameManager.Instance.battleHandler;

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
