using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class SoundManager : MonoBehaviour
{
    [SerializeField]
    private float sfxVolume = 0.6f;
    private float musicVolume = 0.6f;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
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
        
        //sfxSlider.value = sfxVolume;
        //musicSlider.value = musicVolume;
        AdjustMusicVolume(musicVolume);
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

            oneShotAudioSource.volume = sfxVolume;
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

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundTypes sound;
        public AudioClip[] audioClips;
    }
}
