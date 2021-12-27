using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomUIHandler : MonoBehaviour
{
    public Text goldText;

    private void Start()
    {
        GameManager.Instance.OnUpdateUI += SetGoldUI;
        SetGoldUI();
    }

    public void SetGoldUI()
    {
        goldText.text = GameManager.Instance.Gold.ToString();
    }
}
