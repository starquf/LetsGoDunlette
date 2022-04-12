using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : LivingEntity
{
    [Header("적 기본 데이터")]
    public EnemyType enemyType;

    private Collider2D coll;
    private SpriteRenderer sr;

    [HideInInspector]
    public EnemyIndicator indicator;

    public Image weaknessImg;

    [Header("보스 여부")]
    public bool isBoss = false;

    protected override void Awake()
    {
        base.Awake();

        coll = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        indicator = GetComponent<EnemyIndicator>();
    }

    protected override void Start()
    {
        if (weaknessImg != null)
        {
            weaknessImg.sprite = GameManager.Instance.inventoryHandler.effectSprDic[weaknessType];
        }

        base.Start();
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);

        sr.color = Color.red;
        sr.DOColor(Color.white, 0.35f);

        if (!IsDie)
        {
            StartCoroutine(UnBeatTime());
        }
    }

    IEnumerator UnBeatTime()
    {
        int countTime = 0;
        while (countTime < 10)
        {
            //Alpha Effect
            if (countTime % 2 == 0)
            {
                sr.color = new Color32(255, 255, 255, 20);
            }
            else
            {
                sr.color = new Color32(255, 255, 255, 200);
            }
            //Wait Update Frame
            yield return new WaitForSeconds(0.1f);
            countTime++;

            //Alpha Effect End
            sr.color = new Color32(255, 255, 255, 255);
            yield return null;
        }
    }

    public virtual void Kill()
    {
        GetDamage(curHp);
    }

    protected override void Die()
    {
        //print($"{gameObject.name} 죽음!!");

        ShowDieEffect();

        GameManager.Instance.inventoryHandler.RemoveAllOwnerPiece(GetComponent<Inventory>());
        StartCoroutine(GameManager.Instance.battleHandler.battleEvent.ActionEvent(EventTimeEnemy.EnemyDie, this));

        onEnemyDie?.Invoke();

        coll.enabled = false;
        bh.enemys.Remove(this);
    }

    public virtual void ShowDieEffect()
    {
        sr.DOFade(0f, 1f)
            .SetEase(Ease.Linear)
            .OnComplete(() => gameObject.SetActive(false));
    }

    public override void Init()
    {
        base.Init();

        sr.DOFade(1f, 1f)
            .From(0f)
            .SetEase(Ease.Linear);

        coll.enabled = true;
    }
}
