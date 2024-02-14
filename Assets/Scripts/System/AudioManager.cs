using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private Dictionary<Sfx, float> lastPlayTimeDictionary = new Dictionary<Sfx, float>();
    public float defaultCooldown = 0.5f; // 기본 간격

    [Header("#BGM")]
    public AudioClip[] bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    public AudioClip[] sfxClip;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex; // channel index


    public enum Sfx
    {
        attack, hit, open, ui
    }

    void Awake()
    {
    }

    void Start()
    {
        //PlayBgm(true, 1);
    }

    public void Init()
    {
        //음량 초기설정
        float loadBgm = GameManager.instance.LoadSound("BgmVolume");
        if (loadBgm != -1)
        {
            bgmVolume = loadBgm;
        }
        float loadSfx = GameManager.instance.LoadSound("SfxVolume");
        if (loadSfx != -1)
        {
            sfxVolume = loadSfx;
        }
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("sfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true;
        }
    }

    public void PlayBgm(bool isPlay, int bgmIndex = 0)
    {

        bgmPlayer.volume = bgmVolume;

        if (isPlay)
        {
            if (bgmPlayer != null)  // null 체크 추가
            {
                //곡 시작코드
                bgmPlayer.clip = bgmClip[bgmIndex];

                bgmPlayer.Play();
            }
            else
            {
                Debug.LogError("BgmPlayer가 초기화되지 않았습니다.");
            }
        }
        else
        {
            if (bgmPlayer != null)  // null 체크 추가
            {
                bgmPlayer.Stop();
            }
            else
            {
                Debug.LogError("BgmPlayer가 초기화되지 않았습니다.");
            }
        }

    }

    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    public void Playsfx(Sfx sfx)
    {
        float currentTime = Time.time;

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClip[(int)sfx];

            // 소리 크기 조절
            float adjustedVolume = sfxVolume;
            sfxPlayers[loopIndex].volume = adjustedVolume;

            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    // BGM 볼륨 조절 메소드
    public void SetBgmVolume(float volume)
    {
        GameManager.instance.SaveSound("BgmVolume", volume);
        bgmVolume = volume;
        bgmPlayer.volume = volume;
    }

    // SFX 볼륨 조절 메소드
    public void SetSfxVolume(float volume)
    {
        GameManager.instance.SaveSound("SfxVolume", volume);
        sfxVolume = volume;
        foreach (AudioSource sfxPlayer in sfxPlayers)
        {
            sfxPlayer.volume = volume;
        }
    }
}