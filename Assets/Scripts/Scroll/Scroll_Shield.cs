using System;

public class Scroll_Shield : Scroll
{
    public override void Start()
    {
        base.Start();
        scrollType = ScrollType.Shield;
    }

    public override void Use(Action onEndUse, Action onCancelUse)
    {
        bh.player.AddShield(50);

        GameManager.Instance.animHandler.GetAnim(AnimName.M_Shield).SetPosition(bh.playerImgTrans.position)
            .SetScale(1)
            .Play(() =>
        {
            onEndUse?.Invoke();
        });
    }
}
