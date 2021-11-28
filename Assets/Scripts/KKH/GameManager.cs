using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public roulette roulette;
    public GameObject rollingPanel;

    private int rollingType = 0;

    public Text txt;
    public GameObject player;
    public GameObject enemy;
    public Image playerHPbar;
    public Image enemyHpbar;
    private float defaultPlayerHp = 10;
    private float defaultEnemyHp = 10;
    private float playerHp;
    private float enemyHp;

    //±ÍÂú¾Æ¼­ ÀÓ½Ã

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        playerHp = defaultPlayerHp;
        enemyHp = defaultEnemyHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RollRoulette()
    {
        rollingPanel.SetActive(false);
        roulette.RollRoulette(rollingType);
    }

    public void Attack(RType rType)
    {
        //print(rType);
        switch (rType)
        {
            case RType.rEnemyAttack:
                Damaged(1f, 1);
                break;
            case RType.rDefault:
                Damaged(2f);
                break;
            case RType.rWater:
                Damaged(5f);
                break;
            case RType.rlightning:
                Damaged(5f);
                break;
            default:
                break;
        }
        RollingActive();
    }
    private void EnemyDead()
    {
        enemy.SetActive(false);
        rollingType = 1;
        txt.text = "Tap To Award";
    }

    private void ReviveEnemy()
    {
        rollingType = 0;
        txt.text = "Tap To Attack";
        enemy.SetActive(true);
        enemyHp = defaultEnemyHp;
        SetHp();
    }

    private void Damaged(float damage, int type = 0)
    {
        if(type == 0)
        {
            enemyHp -= damage;
            if (enemyHp <= 0)
            {
                EnemyDead();
            }
        }
        else
        {
            playerHp -= damage;
        }
        SetHp();
    }

    private void SetHp()
    {
        playerHPbar.fillAmount = playerHp / defaultPlayerHp;
        enemyHpbar.fillAmount = enemyHp / defaultEnemyHp;

        //print(playerHp);
        //print(enemyHp);
    }

    public void AddRoulette()
    {
        RollingActive();
        ReviveEnemy();
    }


    
    private void RollingActive()
    {
        rollingPanel.SetActive(true);
    }
}
