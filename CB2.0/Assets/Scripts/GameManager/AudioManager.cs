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

    public AudioSource uiSfxAudioSource;

    public List<Audio> backgroundAudios;

    public Audio gameoverAudio;

    public Audio readyAudio;

    public Audio startAudio;

    public IntVariable BGMIndex;

    public void OnGameLobbyInitialized()
    {
        SwitchAndPlayBG(backgroundAudios[BGMIndex.Value].audioClip);
    }

    private void SwitchAndPlayBG(AudioClip clip)
    {
        backgroundAudioSource.Stop();
        backgroundAudioSource.clip = clip;
        backgroundAudioSource.Play();
    }

    private void PlaySfx(AudioClip clip)
    {
        uiSfxAudioSource.PlayOneShot(clip);
    }

    public void RotateBackgroundMusic()
    {
        BGMIndex.Increment(1);
        if (BGMIndex.Value == backgroundAudios.Count)
        {
            BGMIndex.Set(0);
        }
        Audio selectedBackgroundAudio = backgroundAudios[BGMIndex.Value];
        SwitchAndPlayBG(selectedBackgroundAudio.audioClip);
    }

    public void PlayReady()
    {
         BGMIndex.Increment(1);
        if (BGMIndex.Value == backgroundAudios.Count)
        {
            BGMIndex.Set(0);
        }
        StartCoroutine(DelayPlay(readyAudio.audioClip, 0.8f));
    }

    IEnumerator DelayPlay(AudioClip clip, float delay) {
        yield return new WaitForSeconds(delay);
        PlaySfx(clip);
    }

    public void PlayStart()
    {
        PlaySfx(startAudio.audioClip);
        SwitchAndPlayBG(backgroundAudios[BGMIndex.Value].audioClip);
    }

    public void PlayGameover()
    {
        PlaySfx(gameoverAudio.audioClip);
    }
}
