using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public float MasterVoulme { get; private set; } = 0.5f;
    public float BGMVolume => bgmVolume * MasterVoulme;
    public float FxVoulme => fxVoulme * MasterVoulme;

    private float bgmVolume = 1f;
    private float fxVoulme = 1f;

    private AudioSource bgmAudioSourece;
    private List<AudioSource> fxAudioSourceList = new List<AudioSource>();

    private Dictionary<string, AudioClip> bgmSoundDic = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> fxSoundDic = new Dictionary<string, AudioClip>();

    private static SoundHandler instance;
    public static SoundHandler Instance
    {
        get
        {
            if (instance == null) // instance 가 비어있다면
            {
                instance = FindObjectOfType<SoundHandler>(); // 찾아준다
                if (instance == null) // 그래도 없다면 
                {
                    instance = new GameObject(typeof(SoundHandler).ToString()).AddComponent<SoundHandler>(); // 만든다
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);

        foreach (AudioClip audioClip in Resources.LoadAll<AudioClip>("Sound/BGM")) // Resource 폴더에있는 사운드들 담아두기
        {
            bgmSoundDic.Add(audioClip.name, audioClip);
            //Debug.Log(audioClip.name);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = default;
        }
    }

    private AudioClip GetBGMSound(string name)
    {
        if (!bgmSoundDic.TryGetValue(name, out AudioClip result))
        {
            Debug.LogWarning(name + "Not Found");
        }
        return result;
    }

    private AudioClip GetFxSound(string name)
    {
        if (!fxSoundDic.TryGetValue(name, out AudioClip result))
        {
            result = Resources.Load<AudioClip>("Sound/Fx/" + name);
            if (result == null)
            {
                Debug.LogWarning(name + "Not Found");
                return null;
            }
            fxSoundDic.Add(name, result);
        }
        return result;
    }

    private AudioSource MakeAudioSourceObject(string name)
    {
        GameObject audioObject = new GameObject
        {
            name = name
        };
        audioObject.transform.SetParent(gameObject.transform);

        return audioObject.AddComponent<AudioSource>();
    }

    private void SetAudioSource(AudioSource audioSource, AudioClip audioClip, bool isLoop, float volume, bool isMute = false)
    {
        audioSource.clip = audioClip;
        audioSource.loop = isLoop;
        audioSource.volume = volume;
        audioSource.mute = isMute;
    }

    public void AdjustMasterVolume(float newVolume)
    {
        MasterVoulme = newVolume;
        AdjustBGMVolume(bgmVolume);
        AdjustFxVoulme(fxVoulme);
    }

    public void AdjustBGMVolume(float newVolume)
    {
        bgmVolume = newVolume;
        if (bgmAudioSourece != null)
        {
            bgmAudioSourece.volume = BGMVolume;
        }
    }

    public void AdjustFxVoulme(float newVolume)
    {
        fxVoulme = newVolume;
        foreach (AudioSource fxAudioSource in fxAudioSourceList)
        {
            if (fxAudioSource != null)
            {
                fxAudioSource.volume = FxVoulme;
            }
        }
    }

    public void PlayBGMSound(string name)
    {
        if (bgmAudioSourece == null)
        {
            bgmAudioSourece = MakeAudioSourceObject("BGMObject");
        }

        SetAudioSource(bgmAudioSourece, GetBGMSound(name), true, BGMVolume, false);
        bgmAudioSourece.Play();

        StartCoroutine(crossBgm());
    }

    public void PlayFXSound(string name)
    {
        foreach (AudioSource fxAudioSource in fxAudioSourceList)
        {
            if (!fxAudioSource.isPlaying)
            {
                SetAudioSource(fxAudioSource, GetFxSound(name), false, FxVoulme, false);
                fxAudioSource.Play();
                return;
            }
        }

        fxAudioSourceList.Add(MakeAudioSourceObject("FxObject"));
        PlayFXSound(name);
    }

    private IEnumerator crossBgm()
    {
        float newvolume = bgmVolume;
        float nowvolume = 0;

        while (nowvolume < newvolume)
        {
            nowvolume += Time.deltaTime * 0.5f;
            nowvolume = Mathf.Clamp(nowvolume, 0, newvolume);

            //Debug.Log(nowvolume);
            AdjustBGMVolume(nowvolume);

            yield return null;
        }
    }

}