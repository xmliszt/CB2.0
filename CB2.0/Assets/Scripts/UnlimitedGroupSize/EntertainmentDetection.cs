using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntertainmentDetection : MonoBehaviour
{
    // public GameObject player;

    public PlayerStats playerStats;

    private bool zoneFull = false;

    private EntertainmentController entertainmentController;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Entertainments") && !zoneFull) {
            zoneFull = true;
            Debug.Log("ENTERTAINMENT ENTERS ZONE");

            // Update score
            entertainmentController = other.gameObject.GetComponent<EntertainmentController>();
            entertainmentController.AddScore(playerStats);

        }    
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (entertainmentController == other.gameObject.GetComponent<EntertainmentController>() 
        && zoneFull) {
            zoneFull = false;
            Debug.Log("ENTERTAINMENT LEAVE ZONE");

            // Remove score
            entertainmentController.RemoveScore(playerStats);
        }
    }
}
