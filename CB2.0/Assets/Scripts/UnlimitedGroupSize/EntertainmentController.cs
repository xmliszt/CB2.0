using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class EntertainmentController : MonoBehaviour
{
    public GameConstants constants;

    [Header("Player")]
    public GameObject fromPlayer = null;

    [Header("Entertainment Attributes")]
    public bool locked = false;

    public bool upgraded = false;

    [Header("UI")]

    public TMP_Text attractText;

    public GameObject lockObject;

    private SpriteRenderer entertainmentSprite;

    private int attractLevel;

    private GameObject npcUI;

    private UnlimitedGroupControlHandler ugsHandler;

    private SpriteOutlined entertainmentOutline;

    private bool inZone = false;

    private PlayerStats assignedPlayerStats = null;
    
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

        attractText.text = string.Format("x {0}", attractLevel);
        
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
        if (!locked) {

            Vector2 offsetPos = new Vector2(fromPlayer.transform.position.x - gameObject.transform.position.x, 
            fromPlayer.transform.position.y - gameObject.transform.position.y);

            float xOffset = 0.0f;
            float yOffset = 0.0f;

            float offset = 0.6f;

            if (offsetPos.x > 0)
            {
                xOffset = -1 * offset;
            } 
            else 
            {
                xOffset = offset;
            }

            if (offsetPos.y >0)
            {
                yOffset = -1 * offset;
            }
            else
            {
                yOffset = offset;
            }

            gameObject.transform.position =
            new Vector2(fromPlayer.transform.position.x + xOffset, fromPlayer.transform.position.y + yOffset);
        
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Entertainments") && fromPlayer)
        {
            
            ugsHandler = fromPlayer.GetComponent<UnlimitedGroupControlHandler>();
            ugsHandler.held = false;
        }
    }

    public void AddScore(PlayerStats playerStats) {
        
        assignedPlayerStats = playerStats;
        
        inZone = true;

        assignedPlayerStats.score += attractLevel;

        Debug.Log(assignedPlayerStats.score);
    }

    public void RemoveScore(PlayerStats playerStats) {
        
        inZone = false;

        assignedPlayerStats = playerStats;
        assignedPlayerStats.score -= attractLevel;
        Debug.Log(assignedPlayerStats.score);
        assignedPlayerStats = null;

    }

    public void SetSpriteOutline() {
        entertainmentOutline
        .EnableOutline(fromPlayer.GetComponent<PlayerStatsManager>().GetPlayerStats());
    }

    public void DisableSpriteOutline() {
        entertainmentOutline.DisableOutline();
    }
    
    public void SetLock() {
        locked = true;
        lockObject.SetActive(true);
        gameObject.GetComponent<Renderer>().material.color = new Color32(121, 121, 0, 255);
    }

    public void SetUpgrade() {
        upgraded = true;

        if (inZone) {
            assignedPlayerStats.score += attractLevel;
            Debug.Log(assignedPlayerStats.score);

        }
        attractLevel *= constants.upgradeMultiplier;
        attractText.text = string.Format("x {0}", attractLevel);
        attractText.color = new Color32(255, 33, 0, 255);
    }
}
