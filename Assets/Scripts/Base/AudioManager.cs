using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    #region Singleton
    private static AudioManager s_instance;
    public static AudioManager instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new AudioManager();
            return s_instance;
        }
    }

    #endregion

    public AudioSource bgmPlayer;
    public AudioSource soundPlayer;

    public AudioManager()
    {
        s_instance = this;
    }

    public void Awake()
    {
        DontDestroyOnLoad(this);
        bgmPlayer = GameObject.Find("BGMPlayer").GetComponent<AudioSource>();
        soundPlayer = GameObject.Find("SoundPlayer").GetComponent<AudioSource>();
    }

	public void PlayBGM(SoundInfo info)
    {
        if (bgmPlayer.isPlaying) bgmPlayer.Stop();
        AudioClip clip = (AudioClip)Resources.Load(info.soundPath, typeof(AudioClip));
        if (clip == null) Debug.Log("error clip null");
        bgmPlayer.clip = clip;
        bgmPlayer.volume = info.volume;
        if (info.ttl == -1)
            bgmPlayer.loop = true;
        else
            bgmPlayer.loop = false;
        if (info.ttl > 0)
            StartCoroutine(WaitForStopBGM(info.ttl));

        bgmPlayer.Play();

    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySound(SoundInfo info)
    {
        AudioClip clip = (AudioClip)Resources.Load(info.soundPath, typeof(AudioClip));
        if (clip == null) Debug.Log("error clip null");
        soundPlayer.clip = clip;
        soundPlayer.volume = info.volume;
        bgmPlayer.Play();
    }

    IEnumerator WaitForStopBGM(float time)
    {
        yield return new WaitForSeconds(time);
        StopBGM();
    }

}

public class SoundInfo
{
    public string soundPath;
    public float volume;
    public float ttl = 0;

    public SoundInfo()
    {
    }

    public SoundInfo(string path, float vol, bool loop = false)
    {
        soundPath = path;
        volume = vol;
        if(loop)
        ttl = -1;
    }

    public SoundInfo(string path, float vol, float time)
    {
        soundPath = path;
        volume = vol;
        ttl = time;
    }
}
