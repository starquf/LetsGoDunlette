using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAnim : MonoBehaviour
{
    [Header("�⺻ ����Ʈ")]
    public GameObject TextUpAnim;

    [Header("�Ϲ� �Ӽ�")]
    public GameObject contractAnim;
    public GameObject c_sphereCastAnim;
    public GameObject c_ManaSphereHitAnim;

    [Header("���� �Ӽ�")]
    public GameObject e_ManaSphereHitAnim;
    public GameObject lightningRodAnim;
    public GameObject staticAnim;
    public GameObject staticStunAnim;

    [Header("�ڿ� �Ӽ�")]
    public GameObject n_ManaSphereHitAnim;
    public GameObject posionCloudAnim;

    [Header("�� �Ӽ�")]
    public GameObject w_ManaSphereHitAnim;
    public GameObject boatFareAnim;
    public GameObject boatFareBonusMoneyAnim;

    [Header("�� �Ӽ�")]
    public GameObject f_ManaSphereHitAnim;
    public GameObject arsonAnim;
    public GameObject chainExplosionAnim;
    public GameObject chainExplosionBonusAnim;

    [Header("�� �Ӽ�")]
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
        // ===========================================================================  �⺻ ����Ʈ
        PoolManager.CreatePool<Anim_TextUp>(TextUpAnim, this.transform, 5);

        // ===========================================================================  �Ϲ� �Ӽ�
        PoolManager.CreatePool<Anim_Contract>(contractAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_C_SphereCast>(c_sphereCastAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_C_ManaSphereHit>(c_ManaSphereHitAnim, this.transform, 1);

        // ===========================================================================  ���� �Ӽ�
        PoolManager.CreatePool<Anim_E_ManaSphereHit>(e_ManaSphereHitAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_E_LightningRod>(lightningRodAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_E_Static>(staticAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_E_Static_Stun>(staticStunAnim, this.transform, 1);

        // ===========================================================================  �ڿ� �Ӽ�
        PoolManager.CreatePool<Anim_N_ManaSphereHit>(n_ManaSphereHitAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_N_PosionCloud>(posionCloudAnim, this.transform, 1);

        // ===========================================================================  �� �Ӽ�
        PoolManager.CreatePool<Anim_W_ManaSphereHit>(w_ManaSphereHitAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_W_BoatFare>(boatFareAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_W_BoatFareBonusMoney>(boatFareBonusMoneyAnim, this.transform, 1);

        // ===========================================================================  �� �Ӽ�
        PoolManager.CreatePool<Anim_F_ManaSphereHit>(f_ManaSphereHitAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_F_Arson>(arsonAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_F_ChainExplosion>(chainExplosionAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_F_ChainExplosionBonus>(chainExplosionBonusAnim, this.transform, 1);

        // ===========================================================================  �� �Ӽ�
        PoolManager.CreatePool<Anim_M_Butt>(buttAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Sword>(swordAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Bite>(biteAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Scratch>(scratchAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Recover>(recoverAnim, this.transform, 1);
        PoolManager.CreatePool<Anim_M_Shield>(shieldAnim, this.transform, 1);
    }
}
