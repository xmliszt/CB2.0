using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseCollider : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int player = other.GetComponent<STSControlHandler>().GetPlayerID();
            GetComponentInParent<STSBirthdayActivity>().PlayerEnteredRoom(player, this.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int player = other.GetComponent<STSControlHandler>().GetPlayerID();
            GetComponentInParent<STSBirthdayActivity>().PlayerExitedRoom(player, this.name);
        }
    }
}
