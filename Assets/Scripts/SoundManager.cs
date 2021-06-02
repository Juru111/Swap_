using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class SoundManager : MonoBehaviour
{
    private float sfxVolume = 0.3f;
    private float musicVolume = 0.4f;
    private Slider sfxSlider;
    private Slider musicSlider;
    private GameObject oneShotGameObject;
    private AudioSource oneShotAudioSource;
    private Dictionary<SoundTypes, float> soundTimerDictionary;

    public SoundAudioClip[] SoundAudioClips;
    

    private void Start()
    {
        soundTimerDictionary = new Dictionary<SoundTypes, float>();

        soundTimerDictionary[SoundTypes.PlayerStep] = 0f;
        soundTimerDictionary[SoundTypes.Grab] = 0f;
        soundTimerDictionary[SoundTypes.ChargingAtack] = 0f;

        sfxVolume = PlayerPrefs.GetFloat("SfxVolume", sfxVolume);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", musicVolume);

        RefreshAudioSliders();
        AdjustMusicVolume(musicVolume);
    }

    public void RefreshAudioSliders()
    {
        sfxSlider.value = sfxVolume;
        musicSlider.value = musicVolume;
    }

    public void PlaySound(SoundTypes soundType)
    {
        if (CanPlaySound(soundType))
        {
            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("One Shot Sounds");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }

            oneShotAudioSource.volume = sfxVolume/2;
            var audioClipToPlay = GiveAudioClip(soundType);
            if(audioClipToPlay != null)
            {
                oneShotAudioSource.PlayOneShot(audioClipToPlay);
            }  
        }
    }

    private bool CanPlaySound(SoundTypes soundType)
    {
        switch (soundType)
        {
            case SoundTypes.PlayerStep:
                return CheckCooldown(soundType, 0.5f);
            case SoundTypes.Grab:
                return CheckCooldown(soundType, 0.1f);
            case SoundTypes.ChargingAtack:
                return CheckCooldown(soundType, 0.3f);
            default:
                return true;
        }
    }

    private bool CheckCooldown(SoundTypes soundType, float cooldown)
    {
        if (soundTimerDictionary.ContainsKey(soundType))
        {
            float lastTimePlayed = soundTimerDictionary[soundType];
            float SoundCoolDown = cooldown; //jak czêsto mo¿e pojawiæ siê sound danego typu
            if (lastTimePlayed + SoundCoolDown < Time.time)
            {
                soundTimerDictionary[soundType] = Time.time;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    private AudioClip GiveAudioClip(SoundTypes sound)
    {
        foreach (var soundAudioClip in SoundAudioClips)
        {
            if (soundAudioClip.sound == sound)
            {
                if(soundAudioClip.audioClips.Length > 0)
                {
                    return soundAudioClip.audioClips[Random.Range(0, soundAudioClip.audioClips.Length)];
                }
                else
                {
                    Debug.Log("SoundClip is empty: " + sound);
                    return null;
                }
            }
        }
        return null;
    }

    public void TurnMusicDown(float time, float factor = 0.5f)
    {
        StartCoroutine(TurnMusicDownCorr(time, factor));
    }

    private IEnumerator TurnMusicDownCorr(float time, float factor)
    {
        float currMusicVolume = GiveMusicVolume();
        float currSfxVolume = GiveSfxVolume();
        AdjustMusicVolume(currMusicVolume * (1- factor));
        AdjustSfxVolume(currSfxVolume * 4);
        yield return new WaitForSeconds(time);
        AdjustMusicVolume(currMusicVolume);
        AdjustSfxVolume(currSfxVolume);
        yield return null;
    }

    public float GiveSfxVolume()
    {
        return sfxVolume;
    }

    public float GiveMusicVolume()
    {
        return musicVolume;
    }

    public void AdjustSfxVolume(float _volume)
    {
        sfxVolume = _volume;
    }

    public void AdjustMusicVolume(float _volume)
    {
        musicVolume = _volume;
        GameManager.GM.AdjustMusicVolume(musicVolume);
    }

    public void SetMyAudioSliders(Slider _sfxSlider, Slider _musicSlider) // To nie jest dobre omijanie problemu singletona :c
    {
        sfxSlider = _sfxSlider;
        musicSlider = _musicSlider;
    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundTypes sound;
        public AudioClip[] audioClips;
    }
}
