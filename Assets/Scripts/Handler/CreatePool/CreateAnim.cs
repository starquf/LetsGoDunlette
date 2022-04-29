using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAnim : MonoBehaviour
{
    [Header("UI Anim")]
    public GameObject TextUpAnim;
    public GameObject skillDeterminedAnim;
    public GameObject rewardDeterminedAnim;

    [Header("Common Anim")]
    public GameObject contractAnim;
    public GameObject c_sphereCastAnim;
    public GameObject c_ManaSphereHitAnim;

    [Header("Electric Anim")]
    public GameObject e_ManaSphereHitAnim;
    public GameObject lightningRodAnim;
    public GameObject staticAnim;
    public GameObject staticStunAnim;

    [Header("Nature Anim")]
    public GameObject n_DrainAnim;
    public GameObject n_ManaSphereHitAnim;
    public GameObject posionCloudAnim;
    public GameObject windAnim;

    [Header("Water Anim")]
    public GameObject w_ManaSphereHitAnim;
    public GameObject w_Bubble;
    public GameObject boatFareAnim;
    public GameObject boatFareBonusMoneyAnim;
    public GameObject splashAnim;
    public GameObject splash1Anim;

    [Header("Fire Anim")]
    public GameObject f_ManaSphereHitAnim;
    public GameObject arsonAnim;
    public GameObject chainExplosionAnim;
    public GameObject chainExplosionBonusAnim;

    [Header("Enemy Anim")]
    public GameObject buttAnim;
    public GameObject swordAnim;
    public GameObject biteAnim;
    public GameObject scratchAnim;
    public GameObject recoverAnim;
    public GameObject shieldAnim;
    public GameObject shieldAnim2;
    public GameObject wispAnim;

    [Header("There are Variety Anim Effects")]
    public GameObject buffEffect01;
    public GameObject buffEffect02;
    public GameObject buffEffect03;
    public GameObject buffEffect04;
    [Header("WaterIce Variety Effects")]
    public GameObject waterIceEffect01;
    public GameObject waterIceEffect02;
    public GameObject waterIceEffect03;
    public GameObject waterIceEffect04;
    [Header("Etc Variety Effects")]
    public GameObject etcSkillEffect01;
    public GameObject etcSkillEffect02;
    public GameObject etcSkillEffect03;
    public GameObject etcSkillEffect04;
    public GameObject etcSkillEffect05;
    public GameObject etcSkillEffect06;
    [Header("Fire Variety Effects")]
    public GameObject fireEffect01;
    public GameObject fireEffect02;
    public GameObject fireEffect03;
    public GameObject fireEffect04;
    [Header("Elec Variety Effects")]
    public GameObject elecEffect01;
    public GameObject elecEffect02;
    public GameObject elecEffect03;
    public GameObject elecEffect04;
    public GameObject elecEffect05;
    public GameObject elecEffect06;
    public GameObject elecEffect07;

    [Header("CC Effects ")]
    public GameObject stunEffect;
    public GameObject slientEffect;


    private void Awake()
    {
        PoolManager.CreatePool<Anim_TextUp>(TextUpAnim, this.transform, 5);
        //CreatePool();
    }

    private void CreatePool()
    {
        // ===========================================================================  ???? ??????
        PoolManager.CreatePool<Anim_TextUp>(TextUpAnim, this.transform, 5);
        PoolManager.CreatePool<Anim_SkillDetermined>(skillDeterminedAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_RewardDetermined>(rewardDeterminedAnim, this.transform, 1);

        // ===========================================================================  ???? ????
        // ===========================================================================  ???? ????

        // ===========================================================================  ???? ????

        // ===========================================================================  ?? ????
        // ===========================================================================  ?? ????

        // ===========================================================================  ?? ????
        PoolManager.CreatePool<Anim_M_Sword>(swordAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Bite>(biteAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Scratch>(scratchAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Recover>(recoverAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Shield>(shieldAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Wisp>(wispAnim, this.transform, 1);

        // ===================================================================
        PoolManager.CreatePool<Anim_BuffEffect01>(buffEffect01, this.transform, 1);
        PoolManager.CreatePool<Anim_BuffEffect02>(buffEffect02, this.transform, 1);
        PoolManager.CreatePool<Anim_BuffEffect03>(buffEffect03, this.transform, 1);
        PoolManager.CreatePool<Anim_BuffEffect04>(buffEffect04, this.transform, 1);

        PoolManager.CreatePool<Anim_WaterIce01>(waterIceEffect01, this.transform, 1);
        PoolManager.CreatePool<Anim_WaterIce02>(waterIceEffect02, this.transform, 1);
        PoolManager.CreatePool<Anim_WaterIce03>(waterIceEffect03, this.transform, 1);
        PoolManager.CreatePool<Anim_WaterIce04>(waterIceEffect04, this.transform, 1);

        PoolManager.CreatePool<Anim_SkillEffect01>(etcSkillEffect01, this.transform, 1);
        PoolManager.CreatePool<Anim_SkillEffect02>(etcSkillEffect02, this.transform, 1);
        PoolManager.CreatePool<Anim_SkillEffect03>(etcSkillEffect03, this.transform, 1);
        PoolManager.CreatePool<Anim_SkillEffect04>(etcSkillEffect04, this.transform, 1);
        PoolManager.CreatePool<Anim_SkillEffect05>(etcSkillEffect05, this.transform, 1);
        PoolManager.CreatePool<Anim_SkillEffect06>(etcSkillEffect06, this.transform, 1);

        PoolManager.CreatePool<Anim_FireEffect01>(fireEffect01, this.transform, 1);
        PoolManager.CreatePool<Anim_FireEffect02>(fireEffect02, this.transform, 1);
        PoolManager.CreatePool<Anim_FireEffect03>(fireEffect03, this.transform, 1);
        PoolManager.CreatePool<Anim_FireEffect04>(fireEffect04, this.transform, 1);

        PoolManager.CreatePool<Anim_ElecEffect01>(elecEffect01, this.transform, 1);
        PoolManager.CreatePool<Anim_ElecEffect02>(elecEffect02, this.transform, 1);
        PoolManager.CreatePool<Anim_ElecEffect03>(elecEffect03, this.transform, 1);
        PoolManager.CreatePool<Anim_ElecEffect04>(elecEffect04, this.transform, 1);
        PoolManager.CreatePool<Anim_ElecEffect05>(elecEffect05, this.transform, 1);
        PoolManager.CreatePool<Anim_ElecEffect06>(elecEffect06, this.transform, 1);
        PoolManager.CreatePool<Anim_ElecEffect07>(elecEffect07, this.transform, 1);

        PoolManager.CreatePool<Anim_StunEffect>(stunEffect, this.transform, 1);
        PoolManager.CreatePool<Anim_SlientEffect>(slientEffect, this.transform, 1);
    }
}
