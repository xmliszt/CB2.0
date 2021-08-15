using System.Collections.Generic;
using UnityEngine;

public class EnterSelectionPanelDetection : MonoBehaviour
{
    private int minigameIndex;

    private CanvasGroup group;

    private List<GameObject> enteredObjects;

    private void Start()
    {
        group = GetComponent<CanvasGroup>();
        enteredObjects = new List<GameObject>();
    }

    public void SetMinigameIndex(int index)
    {
        minigameIndex = index;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (minigameIndex == 0)
        {
            if (enteredObjects.Count == 0)
            {
                group.alpha = 0.7f;
            }
            if (other.CompareTag("Player"))
            {
                GameObject player = other.gameObject;
                enteredObjects.Add (player);
                player
                    .GetComponent<PlayerZoneManager>()
                    .SetZone(gameObject.tag, gameObject);
                player
                    .GetComponent<GameLobbyControlHandler>()
                    .SetCurrentMinigameSelectorEntered(minigameIndex);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (minigameIndex == 0)
        {
            if (enteredObjects.Count == 1)
            {
                group.alpha = 1.0f;
            }
            if (other.CompareTag("Player"))
            {
                GameObject player = other.gameObject;
                enteredObjects.Remove (player);
                player
                    .GetComponent<PlayerZoneManager>()
                    .SetZone("null", gameObject);
            }
        }
    }
}
