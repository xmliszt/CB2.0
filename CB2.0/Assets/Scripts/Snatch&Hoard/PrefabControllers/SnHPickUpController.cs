using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnHPickUpController : MonoBehaviour
{
    // easy type access
    public PickUpTypeEnum pickupType;

    public SpriteRenderer sprite;

    public int engagedWithPlayer;

    // list of sprites
    public List<Sprite> itemSprites;
    
    // called when instantiated
    public void SetPickUp(PickUpTypeEnum _pickup)
    {
        pickupType = _pickup;
        sprite.sprite = itemSprites[(int)_pickup];
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && engagedWithPlayer == -1)
        {
            engagedWithPlayer = collision.GetComponent<SnHPlayerControlHandler>().playerID;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && engagedWithPlayer != -1)
        {
            if (collision.GetComponent<SnHPlayerControlHandler>().playerID == engagedWithPlayer)
            {
                engagedWithPlayer = -1;
            }
        }
    }
}
