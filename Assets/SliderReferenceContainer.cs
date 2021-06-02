using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderReferenceContainer : MonoBehaviour
{
    // To nie jest dobre omijanie problemu singletona :c
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private Slider musicSlider;

    private void Awake()
    {
        GameManager.GM.SoundManager.SetMyAudioSliders(sfxSlider, musicSlider);
    }

    public void AdjustSfxVolumeCall(float _volume)
    {
        GameManager.GM.SoundManager.AdjustSfxVolume(_volume);
    }

    public void AdjustMusicVolumeCall(float _volume)
    {
        GameManager.GM.SoundManager.AdjustMusicVolume(_volume);
    }
}
