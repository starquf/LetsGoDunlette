using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTrm : MonoBehaviour
{
    public RectTransform trm;
    private RectTransform myRect;

    private void Start()
    {
        myRect = GetComponent<RectTransform>();
    }
    private void Update()
    {
        myRect.position = trm.position;
    }
}
