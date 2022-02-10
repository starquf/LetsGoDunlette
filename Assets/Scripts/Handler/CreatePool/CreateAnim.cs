using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAnim : MonoBehaviour
{
    [Header("扁夯 捞棋飘")]
    public GameObject TextUpAnim;

    [Header("老馆 加己")]
    public GameObject contractAnim;
    public GameObject c_sphereCastAnim;
    public GameObject c_ManaSphereHitAnim;

    [Header("锅俺 加己")]
    public GameObject e_ManaSphereHitAnim;
    public GameObject lightningRodAnim;
    public GameObject staticAnim;
    public GameObject staticStunAnim;

    [Header("磊楷 加己")]
    public GameObject n_ManaSphereHitAnim;
    public GameObject posionCloudAnim;

    [Header("拱 加己")]
    public GameObject w_ManaSphereHitAnim;
    public GameObject boatFareAnim;
    public GameObject boatFareBonusMoneyAnim;

    [Header("阂 加己")]
    public GameObject f_ManaSphereHitAnim;
    public GameObject arsonAnim;
    public GameObject chainExplosionAnim;
    public GameObject chainExplosionBonusAnim;

    [Header("利 加己")]
    public GameObject buttAnim;
    public GameObject swordAnim;
    public GameObject biteAnim;
    public GameObject scratchAnim;
    public GameObject recoverAnim;
    public GameObject shieldAnim;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        // ===========================================================================  扁夯 捞棋飘
        PoolManager.CreatePool<Anim_TextUp>(TextUpAnim, this.transform, 5);

        // ===========================================================================  老馆 加己
        PoolManager.CreatePool<Anim_Contract>(contractAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_C_SphereCast>(c_sphereCastAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_C_ManaSphereHit>(c_ManaSphereHitAnim, this.transform, 1);

        // ===========================================================================  锅俺 加己
        PoolManager.CreatePool<Anim_E_ManaSphereHit>(e_ManaSphereHitAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_E_LightningRod>(lightningRodAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_E_Static>(staticAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_E_Static_Stun>(staticStunAnim, this.transform, 1);

        // ===========================================================================  磊楷 加己
        PoolManager.CreatePool<Anim_N_ManaSphereHit>(n_ManaSphereHitAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_N_PosionCloud>(posionCloudAnim, this.transform, 1);

        // ===========================================================================  拱 加己
        PoolManager.CreatePool<Anim_W_ManaSphereHit>(w_ManaSphereHitAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_W_BoatFare>(boatFareAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_W_BoatFareBonusMoney>(boatFareBonusMoneyAnim, this.transform, 1);

        // ===========================================================================  阂 加己
        PoolManager.CreatePool<Anim_F_ManaSphereHit>(f_ManaSphereHitAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_F_Arson>(arsonAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_F_ChainExplosion>(chainExplosionAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_F_ChainExplosionBonus>(chainExplosionBonusAnim, this.transform, 1);

        // ===========================================================================  利 加己
        PoolManager.CreatePool<Anim_M_Butt>(buttAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Sword>(swordAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Bite>(biteAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Scratch>(scratchAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Recover>(recoverAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Shield>(shieldAnim, this.transform, 1);
    }
}
