using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnHDepthSwitcher : MonoBehaviour
{
    public List<SpriteRenderer> SpriteToSwitch;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (SpriteRenderer s in SpriteToSwitch)
            {
                s.sortingLayerName = "Foreground";
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (SpriteRenderer s in SpriteToSwitch)
            {
                s.sortingLayerName = "Background";
            }
        }
    }
}
