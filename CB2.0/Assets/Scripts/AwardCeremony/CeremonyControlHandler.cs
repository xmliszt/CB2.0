using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeremonyControlHandler : MonoBehaviour
{
    private PlayerController playerController;

    private void Start() {
        playerController = GetComponent<PlayerController>();
    }
}
