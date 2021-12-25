using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAnim : MonoBehaviour
{
    [Header("�Ϲ� �Ӽ�")]
    public GameObject contractAnim;

    [Header("���� �Ӽ�")]
    public GameObject lightningRodAnim;
    public GameObject staticAnim;

    [Header("�ڿ� �Ӽ�")]
    public GameObject posionCloudAnim;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        // ===========================================================================  �Ϲ� �Ӽ�
        PoolManager.CreatePool<Anim_Contract>(contractAnim, this.transform, 2);

        // ===========================================================================  ���� �Ӽ�
        PoolManager.CreatePool<Anim_LightningRod>(lightningRodAnim, this.transform, 2);
        PoolManager.CreatePool<Anim_Static>(staticAnim, this.transform, 2);

        // ===========================================================================  �ڿ� �Ӽ�
        PoolManager.CreatePool<Anim_PosionCloud>(posionCloudAnim, this.transform, 2);
    }
}
