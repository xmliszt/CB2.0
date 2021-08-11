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
    // STS sound effects
    gym = 8,
    computer = 9,
    karaoke = 10,
    birthday = 11,
    ready1 = 12,
    ready2 = 13,
    ready3 = 14,
    ready4 = 15
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

    public void StopSFX()
    {
        source.Stop();
    }

    public void PlayReadySFX(int order)
    {
        SFXType[] readyTypes = new SFXType[4]{SFXType.ready1, SFXType.ready2, SFXType.ready3, SFXType.ready4};
        source.PlayOneShot(sfxAudiosMap[readyTypes[order]].clip);
    }
}
