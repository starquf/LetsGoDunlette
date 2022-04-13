using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAnim : MonoBehaviour
{
    [Header("???? ??????")]
    public GameObject TextUpAnim;
    public GameObject skillDeterminedAnim;

    [Header("???? ????")]
    public GameObject contractAnim;
    public GameObject c_sphereCastAnim;
    public GameObject c_ManaSphereHitAnim;

    [Header("???? ????")]
    public GameObject e_ManaSphereHitAnim;
    public GameObject lightningRodAnim;
    public GameObject staticAnim;
    public GameObject staticStunAnim;

    [Header("???? ????")]
    public GameObject n_DrainAnim;
    public GameObject n_ManaSphereHitAnim;
    public GameObject posionCloudAnim;
    public GameObject windAnim;

    [Header("?? ????")]
    public GameObject w_ManaSphereHitAnim;
    public GameObject w_Bubble;
    public GameObject boatFareAnim;
    public GameObject boatFareBonusMoneyAnim;
    public GameObject splashAnim;
    public GameObject splash1Anim;

    [Header("?? ????")]
    public GameObject f_ManaSphereHitAnim;
    public GameObject arsonAnim;
    public GameObject chainExplosionAnim;
    public GameObject chainExplosionBonusAnim;

    [Header("?? ????")]
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

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        // ===========================================================================  ???? ??????
        PoolManager.CreatePool<Anim_TextUp>(TextUpAnim, this.transform, 5);
        PoolManager.CreatePool<Anim_SkillDetermined>(skillDeterminedAnim, this.transform, 1);

        // ===========================================================================  ???? ????
        PoolManager.CreatePool<Anim_Contract>(contractAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_C_SphereCast>(c_sphereCastAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_C_ManaSphereHit>(c_ManaSphereHitAnim, this.transform, 1);

        // ===========================================================================  ???? ????
        PoolManager.CreatePool<Anim_E_ManaSphereHit>(e_ManaSphereHitAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_E_LightningRod>(lightningRodAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_E_Static>(staticAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_E_Static_Stun>(staticStunAnim, this.transform, 1);

        // ===========================================================================  ???? ????
        PoolManager.CreatePool<Anim_N_ManaSphereHit>(n_ManaSphereHitAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_N_Drain>(n_DrainAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_N_PosionCloud>(posionCloudAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_N_Wind>(windAnim, this.transform, 1);

        // ===========================================================================  ?? ????
        PoolManager.CreatePool<Anim_W_ManaSphereHit>(w_ManaSphereHitAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_W_Bubble>(w_Bubble, this.transform, 1);
        PoolManager.CreatePool<Anim_W_BoatFare>(boatFareAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_W_BoatFareBonusMoney>(boatFareBonusMoneyAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_W_Splash>(splashAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_W_Splash1>(splash1Anim, this.transform, 1);
        PoolManager.CreatePool<Anim_Shield>(shieldAnim2, this.transform, 1);

        // ===========================================================================  ?? ????
        PoolManager.CreatePool<Anim_F_ManaSphereHit>(f_ManaSphereHitAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_F_Arson>(arsonAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_F_ChainExplosion>(chainExplosionAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_F_ChainExplosionBonus>(chainExplosionBonusAnim, this.transform, 1);

        // ===========================================================================  ?? ????
        PoolManager.CreatePool<Anim_M_Butt>(buttAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Sword>(swordAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Bite>(biteAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Scratch>(scratchAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Recover>(recoverAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Shield>(shieldAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Wisp>(wispAnim, this.transform, 1);
    }
}
