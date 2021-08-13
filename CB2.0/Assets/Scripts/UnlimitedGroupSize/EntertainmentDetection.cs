using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntertainmentDetection : MonoBehaviour
{
    public int playerID;

    public Players players;

    private bool zoneFull = false;

    private EntertainmentController entertainmentController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Entertainments") && !zoneFull)
        {
            zoneFull = true;
            // Update score
            entertainmentController =
                other.gameObject.GetComponent<EntertainmentController>();
            if (players.GetPlayers().ContainsKey(playerID))
                entertainmentController
                    .AddScore(players.GetPlayers()[playerID].playerStats);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (
            entertainmentController ==
            other.gameObject.GetComponent<EntertainmentController>() &&
            zoneFull
        )
        {
            zoneFull = false;
            // Remove score
            if (players.GetPlayers().ContainsKey(playerID))
                entertainmentController
                    .RemoveScore(players.GetPlayers()[playerID].playerStats);
        }
    }
}
