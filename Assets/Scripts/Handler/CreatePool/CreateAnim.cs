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

    [Header("磊楷 加己")]
    public GameObject posionCloudAnim;

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
        PoolManager.CreatePool<Anim_LightningRod>(lightningRodAnim, this.transform, 2);
        PoolManager.CreatePool<Anim_Static>(staticAnim, this.transform, 2);

        // ===========================================================================  磊楷 加己
        PoolManager.CreatePool<Anim_PosionCloud>(posionCloudAnim, this.transform, 2);
    }
}
