using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Audio
{
    public string tag;

    public AudioClip audioClip;
}

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource backgroundAudioSource;

    public AudioSource uiSfxAudioSource;

    public AudioSource sfxAudioSource;

    public List<Audio> backgroundAudios;

    public Audio ceremonyAudio;

    public Audio confettiAudio; // with drum-roll

    public Audio singleClapHands;

    public Audio gameoverAudio;

    public Audio readyAudio;

    public Audio startAudio;

    public Audio playerJoinedAudio;

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

    public void PlayJoinPlayer()
    {
        sfxAudioSource.PlayOneShot(playerJoinedAudio.audioClip);
    }
    
    public void PlayCeremony()
    {
        SwitchAndPlayBG(ceremonyAudio.audioClip);
    }

    public void PlayDrumrollCelebration()
    {
        PlaySfx(confettiAudio.audioClip);
    }

    public void PlaySingleClapHands()
    {
        PlaySfx(singleClapHands.audioClip);
    }

    public void PitchUpBGM()
    {
        mixer.SetFloat("bg_pitch", 1.2f);
    }

    public void PitchDownBGM()
    {
        mixer.SetFloat("bg_pitch", 1.0f);
    }
}
