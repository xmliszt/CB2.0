using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntertainmentController : MonoBehaviour
{
    [Header("Player")]
    public GameObject fromPlayer;

    [Header("UI")]   
    public Color color;
    public GameObject[] npcList;

    private SpriteRenderer entertainmentSprite;

    private int attractLevel;

    private GameObject npcUI;

    
    private int generateAttractLevel() {
        float rand = Random.value;
        
        // Probability: 1 (0.35), 2 (0.30), 3 (0.20), 4 (0.10), 5 (0.05)
        if (rand <= .35f) {
            return 1;
        }
        else if (rand <= .65f) {
            return 2;
        }
        else if (rand <= .85f) {
            return 3;
        }
        else if (rand <= .95f) {
            return 4;
        }
        return 5;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        entertainmentSprite = GetComponent<SpriteRenderer>();

        attractLevel = generateAttractLevel();

        for (int i = 0; i < attractLevel; i++) {
            npcList[i].SetActive(true);
        }

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
