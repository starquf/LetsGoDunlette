using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoundSetting : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider fxSlider;

    public Text bgmText;
    public Text fxText;

    private List<Slider> sliders;

    private void Start()
    {
        sliders = new List<Slider>
        {
            bgmSlider,
            fxSlider
        };

        sliders.ForEach((x) =>
        x.onValueChanged.AddListener((value) =>
        {
            x.value = value;
            AdjustVolumes();
        }));
    }

    private void AdjustVolumes()
    {
        SoundHandler.Instance.AdjustBGMVolume(bgmSlider.value);
        bgmText.text = $"{(int)(bgmSlider.value * 100)}%";
        SoundHandler.Instance.AdjustFxVoulme(fxSlider.value);
        fxText.text = $"{(int)(fxSlider.value * 100)}%";
    }
}