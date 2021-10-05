using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScrollManager : MonoBehaviour
{
    public Transform contentsParent;
    private float contentsWidth;
    private float contentsCount;
    private float padding;
    private float cameraWidth;
    private float cameraHeight;

    void Start()
    {
        cameraHeight = Camera.main.orthographicSize * 2;
        cameraWidth = Camera.main.aspect * cameraHeight;
        contentsWidth = contentsParent.GetChild(0).GetComponent<RectTransform>().rect.width;
        contentsCount = contentsParent.GetComponent<RectTransform>().rect.width / contentsWidth;
        padding = (contentsWidth - cameraWidth)/2;
    }

    public void SetTransfromScroll()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
