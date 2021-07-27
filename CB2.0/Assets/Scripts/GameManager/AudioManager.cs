using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Audio
{
    public string tag;

    public AudioClip audioClip;
}

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundAudioSource;

    public List<Audio> backgroundAudios;

    public Audio gameoverAudio;

    public Audio readyAudio;

    public Audio startAudio;

    private int backgroundAudioIdx = 0; // currently selected background audio index

    private void Awake()
    {
        DontDestroyOnLoad (gameObject);
    }

    private void Start()
    {
        SwitchAndPlayBG(backgroundAudios[backgroundAudioIdx].audioClip);
    }

    private void SwitchAndPlayBG(AudioClip clip)
    {
        backgroundAudioSource.Stop();
        backgroundAudioSource.clip = clip;
        backgroundAudioSource.Play();
    }

    public void RotateBackgroundMusic()
    {
        backgroundAudioIdx++;
        if (backgroundAudioIdx == backgroundAudios.Count)
        {
            backgroundAudioIdx = 0;
        }
        Audio selectedBackgroundAudio = backgroundAudios[backgroundAudioIdx];
        SwitchAndPlayBG(selectedBackgroundAudio.audioClip);
    }

    public void PlayReady()
    {
        backgroundAudioSource.Stop();
        StartCoroutine(DelayPlay(readyAudio.audioClip, 0.8f));
    }

    IEnumerator DelayPlay(AudioClip clip, float delay) {
        yield return new WaitForSeconds(delay);
        backgroundAudioSource.PlayOneShot(clip);
    }

    public void PlayStart()
    {
        StartCoroutine(DelayPlay(startAudio.audioClip, 0f));
        SwitchAndPlayBG(backgroundAudios[backgroundAudioIdx].audioClip);
    }

    public void PlayGameover()
    {
        SwitchAndPlayBG(gameoverAudio.audioClip);
    }
}
