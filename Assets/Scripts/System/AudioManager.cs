using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private Dictionary<Sfx, float> lastPlayTimeDictionary = new Dictionary<Sfx, float>();
    public float defaultCooldown = 0.5f; // �⺻ ����

    [Header("#BGM")]
    public AudioClip bgmClip1;
    public AudioClip bgmClip2;
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
        PlayBgm(true, 1);
    }

    public void Init()
    {
        //���� �ʱ⼳��
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
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;

        // �ʱ� Ŭ�� ����
        if (bgmClip1 != null)
            bgmPlayer.clip = bgmClip1;
        else
            Debug.LogError("BgmClip1�� �ʱ�ȭ���� �ʾҽ��ϴ�.");

        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // ȿ���� �÷��̾� �ʱ�ȭ
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

    public void PlayBgm(bool isPlay, int bgmIndex = 1)
    {
        bgmPlayer.volume = bgmVolume;

        if (isPlay)
        {
            if (bgmPlayer != null)  // null üũ �߰�
            {
                if (bgmIndex == 1)
                    bgmPlayer.clip = bgmClip1;
                else if (bgmIndex == 2)
                    bgmPlayer.clip = bgmClip2;

                bgmPlayer.Play();
            }
            else
            {
                Debug.LogError("BgmPlayer�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            }
        }
        else
        {
            if (bgmPlayer != null)  // null üũ �߰�
            {
                bgmPlayer.Stop();
            }
            else
            {
                Debug.LogError("BgmPlayer�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
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

            // �Ҹ� ũ�� ����
            float adjustedVolume = sfxVolume;
            sfxPlayers[loopIndex].volume = adjustedVolume;

            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    // BGM ���� ���� �޼ҵ�
    public void SetBgmVolume(float volume)
    {
        GameManager.instance.SaveSound("BgmVolume", volume);
        bgmVolume = volume;
        bgmPlayer.volume = volume;
    }

    // SFX ���� ���� �޼ҵ�
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