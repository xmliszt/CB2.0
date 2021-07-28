using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    dash = 0,
    hit = 1,
    shoot = 2,
    drop = 3,
    coin = 4,
    _lock = 5,
    submitResult = 6,
    changeOutfit = 7,
}

[System.Serializable]
public class SFXAudio
{
    public SFXType _type;

    public AudioClip clip;
}

public class PlayerAudioController : MonoBehaviour
{
    public List<SFXAudio> sfxAudios;

    private Dictionary<SFXType, SFXAudio>
        sfxAudiosMap = new Dictionary<SFXType, SFXAudio>();

    public AudioSource source;

    private void Start()
    {
        foreach (SFXAudio sfxAudio in sfxAudios)
        {
            sfxAudiosMap.Add(sfxAudio._type, sfxAudio);
        }
    }

    public void PlaySFX(SFXType _type)
    {
        source.PlayOneShot(sfxAudiosMap[_type].clip);
    }
}
