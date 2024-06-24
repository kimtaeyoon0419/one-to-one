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
    /// ���� ����
    /// </summary>
    /// <param name="name">������ ������ �̸�</param>
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if(s == null)
        {
            Debug.Log("������ ã�� ����");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }

    /// <summary>
    /// ȿ���� ����
    /// </summary>
    /// <param name="name">ȿ���� �̸�</param>
    public void PlaySFX(string name) 
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("ȿ������ ã�� ����");
        }
        sfxSource.PlayOneShot(s.clip);
    }
    /// <summary>
    /// ���� mute
    /// </summary>
    public void ToggleMusicMute()
    {
        musicSource.mute = !musicSource.mute;
    }
    /// <summary>
    /// ȿ���� mute
    /// </summary>
    public void ToggleSFXMute()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    /// <summary>
    /// ���� ���� ����
    /// </summary>
    /// <param name="volume"></param>
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    /// <summary>
    /// ȿ���� ���� ����
    /// </summary>
    /// <param name="volume"></param>
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
    #endregion
}
