using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableGameObjects : MonoBehaviour
{
    private bool someoneUsing = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<STSControlHandler>().PlayerOnTriggerEnterInteractable(this.tag, gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<STSControlHandler>().PlayerOnTriggerExitInteractable();
            PlayerStopUsing();
        }
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    public int PlayerUsing()
    {
        // returns 0 if no one using, and player can use
        // returns 1 if someone is using and player not allowed to use

        if (!someoneUsing)
        {
            someoneUsing = true;
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public void PlayerStopUsing()
    {
        someoneUsing = false;
    }
}
