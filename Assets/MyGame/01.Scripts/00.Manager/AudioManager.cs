using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Security.Cryptography;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    #region Unity_Function
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        MusicVolume(0.5f);
        SFXVolume(0.5f);
        PlayMusic("Theme");
    }
    #endregion

    #region Public_Function
    /// <summary>
    /// 음악 실행
    /// </summary>
    /// <param name="name">실행할 음악의 이름</param>
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if(s == null)
        {
            Debug.Log("음악을 찾지 못함");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    /// <summary>
    /// 음악 정지
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }

    /// <summary>
    /// 효과음 실행
    /// </summary>
    /// <param name="name">효과음 이름</param>
    public void PlaySFX(string name) 
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("효과음을 찾지 못함");
        }
        sfxSource.PlayOneShot(s.clip);
    }
    /// <summary>
    /// 음악 mute
    /// </summary>
    public void ToggleMusicMute()
    {
        musicSource.mute = !musicSource.mute;
    }
    /// <summary>
    /// 효과음 mute
    /// </summary>
    public void ToggleSFXMute()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    /// <summary>
    /// 음악 볼류 조절
    /// </summary>
    /// <param name="volume"></param>
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    /// <summary>
    /// 효과음 볼륨 조절
    /// </summary>
    /// <param name="volume"></param>
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
    #endregion
}
