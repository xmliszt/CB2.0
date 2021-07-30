using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiController : MonoBehaviour
{
    private ParticleSystem confetti;
    void Start()
    {
        confetti = GetComponent<ParticleSystem>();
    }

    public void PlayConfetti()
    {
        confetti.Play();
    }
}
