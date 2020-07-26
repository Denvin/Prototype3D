using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [SerializeField] CanvasGroup menu;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectSlider;


    public void ShowMenu()
    {
        menu.gameObject.SetActive(true);

        musicSlider.value = AudioManager.Instance.GetMusicVolume() * musicSlider.maxValue;
        effectSlider.value = AudioManager.Instance.GetEffectVolume() * effectSlider.maxValue;
    }
    public void HideMenu()
    {
        menu.gameObject.SetActive(false);
    }
    public void MusicVolumeChanged()
    {
        AudioManager.Instance.SetMusicVolume(musicSlider.value / musicSlider.maxValue);
    }
    public void EffectVolumeChanged()
    {
        AudioManager.Instance.SetEffectVolume(effectSlider.value / effectSlider.maxValue);
    }
}
