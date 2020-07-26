using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region SingleTon

    public static AudioManager Instance { get; private set; }



    private void Awake()

    {

        if (Instance != null)

        {

            Destroy(gameObject);

        }

        else

        {

            Instance = this;

        }

    }

    #endregion

    [SerializeField] AudioSource music;
    [SerializeField] AudioSource effect;

    private const string PREFS_MUSIC_VOLUME = "MusicVolume";
    private const string PREFS_EFFECT_VOLUME = "EffectVolume";



    public void PlaySound(AudioClip audio)
    {
        effect.PlayOneShot(audio);
    }

    public void SetMusicVolume(float volume)
    {
        music.volume = volume;
        PlayerPrefs.SetFloat(PREFS_MUSIC_VOLUME, volume);
    }

    public void SetEffectVolume(float volume)
    {
        effect.volume = volume;
        PlayerPrefs.SetFloat(PREFS_EFFECT_VOLUME, volume);
    }

    public float GetMusicVolume()
    {
        return music.volume;
    }
    public float GetEffectVolume()
    {
        return effect.volume;
    }
}
