using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnHPickUpController : MonoBehaviour
{
    public SnHPickUps.PickUpType pickupType;

    public SpriteRenderer sprite;

    public List<Sprite> pickupSprites;

    public bool isEngagedWithAnyPlayer;
    public int player;
    
    // called when instantiated
    public void SetPickUp(SnHPickUps.PickUpType _pickup)
    {
        pickupType = _pickup;
        sprite.sprite = pickupSprites[(int) pickupType];
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isEngagedWithAnyPlayer)
        {
            isEngagedWithAnyPlayer = true;
            player = collision.GetComponent<SnHPlayerControlHandler>().playerID;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isEngagedWithAnyPlayer)
        {
            if (collision.GetComponent<SnHPlayerControlHandler>().playerID == player)
            {
                isEngagedWithAnyPlayer = false;
                player = -1;
            }
        }
    }
}
