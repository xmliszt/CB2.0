using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeremonyControlHandler : MonoBehaviour
{
    private PlayerController playerController;

    private void Start() {
        playerController = GetComponent<PlayerController>();
    }

    public void OnCeremonyStart()
    {
        gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void OnCeremonyEnd()
    {
        gameObject.transform.localScale = new Vector3(1,1,1);
    }
}
