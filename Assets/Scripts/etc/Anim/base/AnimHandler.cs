using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimInfo
{
    public AnimName name;
    public AnimationClip clip;
    public Vector3 defaultSize = Vector3.one;
}

public class AnimHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject animObjBase;
    [SerializeField]
    private GameObject textAnimObj;

    public List<AnimInfo> animInfos = new List<AnimInfo>();

    private Dictionary<AnimName, AnimationClip> animDic;

    private void Awake()
    {
        GameManager.Instance.animHandler = this;

        PoolManager.CreatePool<AnimObj>(animObjBase, transform, 2);
        PoolManager.CreatePool<AnimObj>(textAnimObj, transform, 2);

        InitAnim();
    }

    private void InitAnim()
    {
        animDic = new Dictionary<AnimName, AnimationClip>();

        for (int i = 0; i < animInfos.Count; i++)
        {
            animDic.Add(animInfos[i].name, animInfos[i].clip);
        }
    }

    public AnimObj GetAnim(AnimName name)
    {
        AnimObj animObj = PoolManager.GetItem<AnimObj>();

        if (animDic.TryGetValue(name, out AnimationClip clip))
        {
            animObj.SetAnim(clip);
        }
        else
        {
            Debug.LogError($"애니메이션이 없습니다!!  이름 : {name.ToString()}");
        }

        return animObj;
    }

    public Anim_TextUp GetTextAnim()
    {
        Anim_TextUp animObj = PoolManager.GetItem<Anim_TextUp>();

        return animObj;
    }
}
