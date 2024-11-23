using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer audioMixer;

    public AudioSource music;

    public float fadeDuration = 1f;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        StopMusic();
    }

    private void Start()
    {
        playMusic();
    }

    void Update()
    {

    }

    public void StopMusic()
    {
        music.Stop();
    }

    public void playMusic()
    {
        if (music.isPlaying == false)
        {
            StopMusic();
            //StartCoroutine(FadeOut());
            StartCoroutine(FadeIn(music));
        }
    }

    public void MethodFadeOut(AudioSource source)
    {
        StartCoroutine(FadeOut(source));
    }

    private IEnumerator FadeOut(AudioSource source)
    {
        float startVolume = source.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        source.volume = 0;

        source.Stop();
    }

    private IEnumerator FadeIn(AudioSource source)
    {
        source.Play();

        float startVolume = 0;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(startVolume, 0.3f, t / fadeDuration);
            yield return null;
        }

        source.volume = 0.3f;
    }
}
