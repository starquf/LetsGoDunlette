using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUtilHandler : MonoBehaviour
{
    private BattleHandler bh;
    private InventoryHandler inventory;
    private SkillRullet mainRullet;

    [Header("ĳ���� ����Ʈ")]
    [SerializeField] private List<PlayerInfo> playerPrefab = new List<PlayerInfo>();

    [Header("ĳ���� �ʱ�ȭ ����")]
    [SerializeField] private GameObject hpCvs;
    [SerializeField] private Image damageBGEffect;
    [SerializeField] private TextMeshProUGUI topPanelHpText;
    [SerializeField] private Transform ccUITrm;
    [SerializeField] private InventoryIndicator playerIndicator;

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;
        inventory = GameManager.Instance.inventoryHandler;
    }

    public void Init(SkillRullet rullet)
    {
        mainRullet = rullet;
    }

    public bool CheckRulletPenalty(bool isPlayerPiece)
    {
        List<RulletPiece> pieces = mainRullet.GetPieces();

        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i] != null)
            {
                if ((pieces[i] as SkillPiece).isPlayerSkill != isPlayerPiece)
                {
                    return false;
                }
            }
            else
            {
                //print("��ĭ �߻�");
                return false;
            }
        }

        return true;
    }

    public void ChangeRulletPiece(int pieceIdx)
    {
        SkillPiece skill = inventory.GetRandomUnusedSkill();

        mainRullet.GetComponent<SkillRullet>().ChangePiece(pieceIdx, skill);
    }

    public void ChangeRulletPiece(int pieceIdx, SkillPiece piece)
    {
        mainRullet.GetComponent<SkillRullet>().ChangePiece(pieceIdx, piece);
    }

    public IEnumerator DrawRulletPieces()
    {
        List<RulletPiece> pieces = mainRullet.GetPieces();
        List<int> changeIdxList = new List<int>();

        for (int i = 0; i < pieces.Count; i++)
        {
            // ����ִ°��̶��
            if (pieces[i] == null)
            {
                changeIdxList.Add(i);
            }
        }

        for (int i = 0; i < changeIdxList.Count; i++)
        {
            SkillPiece skill = inventory.GetRandomUnusedSkill();
            mainRullet.SetPiece(changeIdxList[i], skill);

            if (i != changeIdxList.Count - 1)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(0.45f);

        yield break;
    }

    public IEnumerator ResetRulletPiecesWithCondition(Action<Vector3> onResetPiecePosition = null, bool hasWait = false)
    {
        // �κ��丮���� ������ 6���� ��ų�� �̾� �귿�� �����Ѵ�. ��, �ּ��� ���� ��ų 1���� �� ��ų 2���� ����ȴ�.
        // true : �÷��̾�    false : ��

        List<bool> condition = new List<bool>() { true, true, false };
        List<RulletPiece> pieces = mainRullet.GetPieces();

        for (int i = 0; i < condition.Count; i++)
        {
            SkillPiece skill = GetRandomPlayerOrEnemySkill(condition[i]);

            if (pieces.Count > i)
            {
                ChangeRulletPiece(i, skill);

                if (pieces[i] != null)
                {
                    onResetPiecePosition?.Invoke(pieces[i].skillIconImg.transform.position);

                    yield return null;
                }
            }
            else
            {
                mainRullet.AddPiece(skill);
            }

            if (hasWait)
            {
                yield return new WaitForSeconds(0.15f);
            }
        }

        for (int i = condition.Count; i < 6; i++)
        {
            SkillPiece skill = GetRandomSkill();

            if (pieces.Count > i)
            {
                ChangeRulletPiece(i, skill);

                if (pieces[i] != null)
                {
                    onResetPiecePosition?.Invoke(pieces[i].skillIconImg.transform.position);

                    yield return null;
                }
            }
            else
            {
                mainRullet.AddPiece(skill);
            }

            if (i != 5)
            {
                if (hasWait)
                {
                    yield return new WaitForSeconds(0.15f);
                }
            }
        }

        yield return new WaitForSeconds(0.45f);

        yield break;
    }

    public void SetSkillsToGraveyard()
    {
        List<RulletPiece> pieces = mainRullet.GetPieces();

        for (int i = 0; i < pieces.Count; i++)
        {
            SetPieceToGraveyard(pieces[i] as SkillPiece);
            pieces[i] = null;
        }
    }

    public SkillPiece GetRandomPlayerOrEnemySkill(bool isPlayer) //true �� �÷��̾�
    {
        SkillPiece skill = inventory.GetRandomPlayerOrEnemySkill(isPlayer);

        skill.transform.localScale = new Vector3(0.2f, 0.2f, 1f);

        return skill;
    }

    public SkillPiece GetRandomSkill()
    {
        SkillPiece skill = inventory.GetRandomUnusedSkill();
        skill.transform.localScale = new Vector3(0.2f, 0.2f, 1f);

        return skill;
    }

    public IEnumerator ResetRullet(Action onEndReset = null, Action<RulletPiece> onChangePiece = null)
    {
        List<RulletPiece> pieces = mainRullet.GetPieces();

        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i] != null)
            {
                onChangePiece?.Invoke(pieces[i]);
            }

            ChangeRulletPiece(i);

            if (i != 5)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(0.45f);

        onEndReset.Invoke();

        yield break;
    }

    public void SetPieceToGraveyard(int pieceIdx)
    {
        mainRullet.PutRulletPieceToGraveYard(pieceIdx);
    }

    public void SetPieceToGraveyard(SkillPiece piece)
    {
        if (piece == null)
        {
            return;
        }

        inventory.SetSkillToGraveyard(piece);
    }

    public void SetPieceToInventory(SkillPiece piece)
    {
        inventory.SetSkillToInventory(piece);
    }

    public void SetRulletEmpty(int pieceIdx)
    {
        mainRullet.SetEmpty(pieceIdx);
    }

    public void SetTimer(float time, Action onEndWait)
    {
        StartCoroutine(WaitTimer(time, onEndWait));
    }

    private IEnumerator WaitTimer(float time, Action onEndWait)
    {
        yield return new WaitForSeconds(time);

        onEndWait?.Invoke();
    }

    public bool CheckDie(LivingEntity player)
    {
        return player.IsDie;
    }

    public bool CheckEnemyDie(List<EnemyHealth> enemys)
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            if (!enemys[i].IsDie)
            {
                return false;
            }
        }

        return true;
    }

    public List<EnemyHealth> CheckLivingEnemy(List<EnemyHealth> enemys)
    {
        List<EnemyHealth> livingEnemys = new List<EnemyHealth>();

        for (int i = 0; i < enemys.Count; i++)
        {
            if (!enemys[i].IsDie)
            {
                livingEnemys.Add(enemys[i]);
            }
        }

        return livingEnemys;
    }

    public List<LivingEntity> DeepCopyEnemyList(List<EnemyHealth> targetList)
    {
        List<LivingEntity> list = new List<LivingEntity>();

        for (int i = 0; i < targetList.Count; i++)
        {
            list.Add(targetList[i]);
        }

        return list;
    }

    public Sprite GetDesIcon(SkillPiece skillPiece, DesIconType type)
    {
        Sprite icon = null;

        icon = type switch
        {
            DesIconType.Attack => inventory.effectSprDic[skillPiece.currentType],
            DesIconType.Stun => GameManager.Instance.ccIcons[0],
            DesIconType.Silence => GameManager.Instance.ccIcons[1],
            DesIconType.Exhausted => GameManager.Instance.ccIcons[2],
            DesIconType.Wound => GameManager.Instance.ccIcons[3],
            DesIconType.Invincibility => GameManager.Instance.ccIcons[4],
            DesIconType.Fascinate => GameManager.Instance.ccIcons[5],
            DesIconType.Heating => GameManager.Instance.ccIcons[6],
            DesIconType.Shield => GameManager.Instance.buffIcons[0],
            DesIconType.Heal => GameManager.Instance.buffIcons[1],
            DesIconType.Upgrade => GameManager.Instance.buffIcons[2],
            _ => null,
        };
        return icon;
    }

    public string GetElementalName(ElementalType elemental)
    {
        string name = "";

        switch (elemental)
        {
            case ElementalType.None:
                name = "���Ӽ�";
                break;

            case ElementalType.Nature:
                name = "�ڿ�";
                break;

            case ElementalType.Electric:
                name = "����";
                break;

            case ElementalType.Fire:
                name = "��";
                break;

            case ElementalType.Water:
                name = "��";
                break;

            case ElementalType.Monster:
                name = "��";
                break;
        }

        return name;
    }

    public PlayerHealth CreatePlayer(PlayerCharacterName name)
    {
        for (int i = 0; i < playerPrefab.Count; i++)
        {
            if (playerPrefab[i].characterName.Equals(name))
            {
                print("ĳ���� ����");

                PlayerHealth player = Instantiate(playerPrefab[i].gameObject).GetComponent<PlayerHealth>();

                InitPlayer(player);

                return player;
            }
        }

        Debug.LogError("��ġ�ϴ� �÷��̾� �������� �����ϴ�!!");

        return null;
    }

    private void InitPlayer(PlayerHealth player)
    {
        player.hPCvs = hpCvs;
        player.cc.ccUITrm = ccUITrm;
        player.damageBGEffect = damageBGEffect;
        player.topPanelHPText = topPanelHpText;
        player.GetComponent<PlayerInventory>().indicator = playerIndicator;

        player.InitHpCvs();
        player.cc.Init();
    }
}
