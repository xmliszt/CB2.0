using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnHCheckoutController : MonoBehaviour
{
    public int engagedWithPlayer;

    public GameObject basket;
    public GameObject progress; 

    // Start is called before the first frame update
    void Start()
    {
        // not engaged with any player
        engagedWithPlayer = -1;

        // basket not active
        basket.SetActive(false);

        // progress disabled
        progress.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && engagedWithPlayer == -1)
        {
            engagedWithPlayer = collision.GetComponent<PlayerStatsManager>().GetPlayerStats().playerID;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && engagedWithPlayer == collision.GetComponent<PlayerStatsManager>().GetPlayerStats().playerID)
        {
            engagedWithPlayer = -1;
        }
    }

    public void Checkout()
    {
        // set basket to active
        basket.SetActive(true);
    }
}
