using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealth : LivingEntity
{
    [Header("적 기본 데이터")]
    public EnemyType enemyType;

    private SpriteRenderer sr;

    [HideInInspector]
    public EnemyIndicator indicator;

    public Image weaknessImg;

    [Header("보스 여부")]
    public bool isBoss = false;

    public UnityEvent onInit = null;

    private Vector3 originSize;

    public Sprite iconSpr;

    protected override void Awake()
    {
        base.Awake();

        sr = GetComponent<SpriteRenderer>();

        indicator = GetComponent<EnemyIndicator>();

        originSize = transform.localScale;

        iconSpr = hPCvs.transform.Find("head").GetComponent<Image>().sprite;

        cc.Init();
    }

    protected override void Start()
    {
        if (weaknessImg != null)
        {
            weaknessImg.sprite = GameManager.Instance.inventoryHandler.effectSprDic[weaknessType];
        }

        base.Start();
    }

    public override void GetDamage(int damage, bool isCritical = false)
    {
        base.GetDamage(damage, isCritical);
        Color color = sr.color;

        sr.color = Color.red;
        sr.DOColor(color.Equals(Color.clear) ? color : Color.white, 0.35f);

        if (IsDie)
        {
            //StartCoroutine(UnBeatTime(color));
        }
    }

    private IEnumerator UnBeatTime(Color defaultColor)
    {
        int countTime = 0;
        while (countTime < 10)
        {
            //Alpha Effect
            sr.color = countTime % 2 == 0 ? (Color)new Color32(255, 255, 255, 20) : (Color)new Color32(255, 255, 255, 200);
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
        GetDamageIgnoreShild(curHp);
    }

    protected override void Die()
    {
        base.Die();
        ShowDieEffect();
        Inventory inven = GetComponent<Inventory>();
        GameManager.Instance.inventoryHandler.RemoveAllOwnerPiece(inven);
        StartCoroutine(bh.battleEvent.ActionEvent(EventTimeEnemy.EnemyDie, this));

        bh.enemys.Remove(this);
        GameManager.Instance.inventoryHandler.RemoveInventory(inven);

        inven.skills.Clear();
    }

    public virtual void SetScale(float percent)
    {
        transform.DOScale(originSize * percent, 0.3f);
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

        onInit?.Invoke();

        sr.DOFade(1f, 1f)
            .From(0f)
            .SetEase(Ease.Linear);

        Inventory inven = GetComponent<Inventory>();
        inven.skills.Clear();
    }
}
