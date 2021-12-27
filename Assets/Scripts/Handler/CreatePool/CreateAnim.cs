using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAnim : MonoBehaviour
{
    [Header("老馆 加己")]
    public GameObject contractAnim;
    public GameObject c_sphereCastAnim;

    [Header("锅俺 加己")]
    public GameObject lightningRodAnim;
    public GameObject staticAnim;
    public GameObject staticStunAnim;

    [Header("磊楷 加己")]
    public GameObject posionCloudAnim;

    [Header("拱 加己")]
    public GameObject boatFareAnim;

    [Header("阂 加己")]
    public GameObject ChainExplosionAnim;
    public GameObject ChainExplosionBonusAnim;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        // ===========================================================================  老馆 加己
        PoolManager.CreatePool<Anim_Contract>(contractAnim, this.transform, 2);
        PoolManager.CreatePool<Anim_C_SphereCast>(c_sphereCastAnim, this.transform, 2);

        // ===========================================================================  锅俺 加己
        PoolManager.CreatePool<Anim_E_LightningRod>(lightningRodAnim, this.transform, 2);
        PoolManager.CreatePool<Anim_E_Static>(staticAnim, this.transform, 2);
        PoolManager.CreatePool<Anim_E_Static_Stun>(staticStunAnim, this.transform, 2);

        // ===========================================================================  磊楷 加己
        PoolManager.CreatePool<Anim_N_PosionCloud>(posionCloudAnim, this.transform, 2);

        // ===========================================================================  拱 加己
        PoolManager.CreatePool<Anim_W_BoatFare>(boatFareAnim, this.transform, 2);

        // ===========================================================================  阂 加己
        PoolManager.CreatePool<Anim_F_ChainExplosion>(ChainExplosionAnim, this.transform, 2);
        PoolManager.CreatePool<Anim_F_ChainExplosionBonus>(ChainExplosionBonusAnim, this.transform, 2);
    }
}
