using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntertainmentController : MonoBehaviour
{
    public GameObject fromPlayer;

    public Color color;

    private SpriteRenderer entertainmentSprite;
    // Start is called before the first frame update
    void Start()
    {
        entertainmentSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveItem()
    {
        // TODO: Outline the items
        
        // PlayerStats playerStats = gameObject.GetComponent<PlayerStats>();
        
        // bool outline = true;
        // int outlineSize = 16;
        
        // MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        // entertainmentSprite.GetPropertyBlock (mpb);
        // mpb.SetFloat("_Outline", outline ? 1f : 0);
        // mpb.SetColor("_OutlineColor", color);
        // mpb.SetFloat("_OutlineSize", outlineSize);
        // entertainmentSprite.SetPropertyBlock (mpb);
        
        gameObject.transform.position =
        new Vector2(fromPlayer.transform.position.x + 0.6f, fromPlayer.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Entertainments"))
        {
            
            UnlimitedGroupControlHandler unlimitedHandlerScript = 
            fromPlayer.GetComponent<UnlimitedGroupControlHandler>();
            
            unlimitedHandlerScript.held = false;
        }
    }
}
