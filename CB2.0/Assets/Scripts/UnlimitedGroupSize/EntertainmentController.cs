using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntertainmentController : MonoBehaviour
{
    [Header("Player")]
    public GameObject fromPlayer = null;

    [Header("UI")]   
    public GameObject[] npcList;

    private SpriteRenderer entertainmentSprite;

    private int attractLevel;

    private GameObject npcUI;

    private UnlimitedGroupControlHandler ugsHandler;

    private SpriteOutlined entertainmentOutline;
    
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

        entertainmentOutline = GetComponent<SpriteOutlined>();
        entertainmentOutline.DisableOutline();

    }



    // Update is called once per frame
    void Update()
    {
        if(fromPlayer == null) {
            GetComponent<SpriteOutlined>().DisableOutline();
        }
    }

    public void MoveItem()
    {       
        gameObject.transform.position =
        new Vector2(fromPlayer.transform.position.x + 0.6f, fromPlayer.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Entertainments") && fromPlayer)
        {
            
            ugsHandler = fromPlayer.GetComponent<UnlimitedGroupControlHandler>();
            ugsHandler.held = false;
        }
    }

    public void AddScore(GameObject player) {
        Debug.Log("Add Player Score");

        ugsHandler = player.GetComponent<UnlimitedGroupControlHandler>();
        ugsHandler.updateScore(attractLevel);

    }

    public void RemoveScore(GameObject player) {
        Debug.Log("Remove Player Score");

        ugsHandler = player.GetComponent<UnlimitedGroupControlHandler>();
        int subtractScore = attractLevel * -1;
        ugsHandler.updateScore(subtractScore);

    }

    public void SetSpriteOutline() {
        entertainmentOutline
        .EnableOutline(fromPlayer.GetComponent<PlayerStatsManager>().GetPlayerStats());
    }

    public void DisableSpriteOutline() {
        entertainmentOutline.DisableOutline();
    }
}
