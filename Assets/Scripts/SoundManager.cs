using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public enum Sound
    {
        BuildingPlaced,
        BuildingDamaged,
        BuildingDestroyed,
        EnemyDie,
        EnemyHit,
        EnemyWaveStarting,
        GameOver,
    }

    private AudioSource audioSource;
    private Dictionary<Sound, AudioClip> soundAudioClipDictionary;
    private float volume = 0.5f;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat("soundVolume",0.5f);

        soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();
        foreach (Sound sound in System.Enum.GetValues(typeof(Sound)))
        {
            soundAudioClipDictionary.Add(sound, Resources.Load<AudioClip>(sound.ToString()));
        }
    }

    public void PlaySound(Sound sound) 
    {
        audioSource.PlayOneShot(soundAudioClipDictionary[sound],volume);
    }

    public void IncreaseVolume() 
    {
        volume += 0.1f;
        volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("soundVolume", volume);
    }
    public void DecreaseVolume()
    {
        volume -= 0.1f;
        volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("soundVolume", volume);
    }

    public float GetVolume() 
    {
        return volume;
    }
}
