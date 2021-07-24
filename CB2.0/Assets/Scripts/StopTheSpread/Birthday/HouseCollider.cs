using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
