using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : BaseMission
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioMixer _audioMixer; 
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _bgmSlider;

    private AudioSource _sfxSource;
    private AudioSource _bgmSource;

    private void Start()
    {
        SetSingleton();
        SetObject();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void SetObject()
    {
        _sfxSource = GetMissionComponent<AudioSource>("SFX");
        _bgmSource = GetMissionComponent<AudioSource>("BGM"); 
        _sfxSlider.onValueChanged.AddListener(SetVolumeSFX);
        _bgmSlider.onValueChanged.AddListener(SetVolumeBGM);
    }

    /// <summary>
    /// SFX ���� ����
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolumeSFX(float volume)
    {
        _audioMixer.SetFloat("SFX", volume * 20f); 
    }

    /// <summary>
    /// BGM ���� ����
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolumeBGM(float volume)
    {
        _audioMixer.SetFloat("BGM", volume * 20f);
    }

    /// <summary>
    /// BGM ��ü �� ���
    /// </summary>
    /// <param name="clip"></param>
    public void BGMPlay(AudioClip clip)
    {
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    /// <summary>
    /// SFX ��ü �� ��� 
    /// </summary>
    /// <param name="clip"></param>
    public void SFXPlay(AudioClip clip)
    {
        _sfxSource.clip = clip;
        _sfxSource.Play();
    }
}