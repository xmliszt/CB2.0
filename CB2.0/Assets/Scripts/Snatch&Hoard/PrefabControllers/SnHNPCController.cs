using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SnHNPCController : MonoBehaviourPun
{

    public int engagedWithPlayer;

    public SpriteRenderer NPCSprite;

    public GameObject RightItemBubble;
    public SpriteRenderer RightItemSprite;
    public Text RightItemQuantity;
    public GameObject LeftItemBubble;
    public SpriteRenderer LeftItemSprite;
    public Text LeftItemQuantity;

    public Animator animator;

    public GameObject LeftThinkingBubble;
    public GameObject RightThinkingBubble;
    public GameObject LeftAngryBubble;
    public GameObject RightAngryBubble;

    public bool isThinking;
    public int quantityRemaining;
    public PickUpTypeEnum expectedPickup; 

    public List<Sprite> npcLeftSprites;
    public List<Sprite> npcRightSprites;
    public List<Sprite> npcUpSprites;
    public List<Sprite> itemSprites;
    public List<RuntimeAnimatorController> avatarAnimatorControllers;

    public string direction;

    private int minSeconds = 5;
    private int maxSeconds = 20;

    private int minQuantity = 3;
    private int maxQuantity = 5;

    private PlayerStatsManager collidedPlayerStatsManager;

    private PlayerAudioController collidedPlayerAudioController;

    // called by the NPC Manager
    public void onStart()
    {
        // set the npc sprite and default thinking bubble
        if (direction == "up")
        {
            int idx = Random.Range(0, npcUpSprites.Count - 1);
            NPCSprite.sprite = npcUpSprites[idx];
            animator.runtimeAnimatorController = avatarAnimatorControllers[idx];
            animator.SetFloat("vertical_idle", 1.0f);
        }

        else if (direction == "left")
        {
            int idx = Random.Range(0, npcLeftSprites.Count - 1);
            NPCSprite.sprite = npcLeftSprites[idx];
            animator.runtimeAnimatorController = avatarAnimatorControllers[idx];
            animator.SetFloat("horizontal_idle", -1.0f);
        }

        else if (direction == "right")
        {
            int idx = Random.Range(0, npcRightSprites.Count - 1);
            NPCSprite.sprite = npcRightSprites[idx];
            animator.runtimeAnimatorController = avatarAnimatorControllers[idx];
            animator.SetFloat("horizontal_idle", 1.0f);
        }

        setThinkingBubble();

        // initially no quantity
        quantityRemaining = 0;
        expectedPickup = PickUpTypeEnum.noneType;

        // not engaged with any player
        engagedWithPlayer = -1;

        // start coroutine
        StartCoroutine(Thinking());
    }

    // player enters zone
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && engagedWithPlayer == -1)
        {
            engagedWithPlayer = collision.GetComponent<PlayerStatsManager>().GetPlayerStats().playerID;
            collidedPlayerStatsManager = collision.GetComponent<PlayerStatsManager>();
            collidedPlayerAudioController = collision.GetComponent<PlayerAudioController>();
        }
    }

    // player exits zone
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && engagedWithPlayer == collision.GetComponent<PlayerStatsManager>().GetPlayerStats().playerID)
        {
            engagedWithPlayer = -1;
            collidedPlayerStatsManager = null;

            if (!isThinking)
            {
                setItemBubble();
            }
        }
    }

    public void WrongPickupGiven()
    {
        setAngryBubble();
    }

    private void Update()
    {
        LeftItemQuantity.text = quantityRemaining.ToString();
        RightItemQuantity.text = quantityRemaining.ToString();
    }

    // assumes the player is cleared to be interacted with and is in the zone currently
    public void CorrectPickupGiven()
    {
        quantityRemaining -= 1;

        // if quota met
        if (quantityRemaining == 0)
        {
            // give coin
            collidedPlayerStatsManager.GetPlayerStats().coins += 1;

            collidedPlayerAudioController.PlaySFX(SFXType.coin);

            // remove expected pickup
            expectedPickup = PickUpTypeEnum.noneType;

            // enter thinking mode with corresponding direction bubble
            setThinkingBubble();

            // coroutine
            StartCoroutine(Thinking());
        }
    }

    IEnumerator Thinking()
    {
        // change to thinking state
        isThinking = true;

        // set thinking bubble
        setThinkingBubble();

        // go to sleep
        int sleepfor = Random.Range(minSeconds, maxSeconds);
        yield return new WaitForSeconds(sleepfor);

        // finished thinking. set required pickup sprite and type
        int itemIDX = Random.Range(1, 3);
        // determine the required quantity
        int qnty = Random.Range(minQuantity, maxQuantity);
        
        photonView.RPC("SetThinkingItem", RpcTarget.AllBuffered, itemIDX, qnty);
    }

    [PunRPC]
    private void SetThinkingItem(int itemIDX, int qnty)
    {
        RightItemSprite.sprite = itemSprites[itemIDX]; 
        LeftItemSprite.sprite = itemSprites[itemIDX];
        expectedPickup = (PickUpTypeEnum)itemIDX;

        quantityRemaining = qnty;
        RightItemQuantity.text = quantityRemaining.ToString();
        LeftItemQuantity.text = quantityRemaining.ToString();

        // switch on the item bubble
        setItemBubble();

        // set bool
        isThinking = false;
    }    


    // up, right: right
    // left: left
    private void setThinkingBubble()
    {
        if (direction == "up")
        {
            LeftThinkingBubble.SetActive(false);
            RightThinkingBubble.SetActive(true);
            LeftItemBubble.SetActive(false);
            RightItemBubble.SetActive(false);
            LeftAngryBubble.SetActive(false);
            RightAngryBubble.SetActive(false);
        }

        if (direction == "left")
        {
            LeftThinkingBubble.SetActive(true);
            RightThinkingBubble.SetActive(false);
            LeftItemBubble.SetActive(false);
            RightItemBubble.SetActive(false);
            LeftAngryBubble.SetActive(false);
            RightAngryBubble.SetActive(false);
        }

        if (direction == "right")
        {
            LeftThinkingBubble.SetActive(false);
            RightThinkingBubble.SetActive(true);
            LeftItemBubble.SetActive(false);
            RightItemBubble.SetActive(false);
            LeftAngryBubble.SetActive(false);
            RightAngryBubble.SetActive(false);
        }
    }

    private void setAngryBubble()
    {
        if (direction == "up" || direction == "right")
        {
            LeftThinkingBubble.SetActive(false);
            RightThinkingBubble.SetActive(false);
            LeftItemBubble.SetActive(false);
            RightItemBubble.SetActive(false);
            LeftAngryBubble.SetActive(false);
            RightAngryBubble.SetActive(true);
        }
        else if (direction == "left")
        {
            LeftThinkingBubble.SetActive(false);
            RightThinkingBubble.SetActive(false);
            LeftItemBubble.SetActive(false);
            RightItemBubble.SetActive(false);
            LeftAngryBubble.SetActive(true);
            RightAngryBubble.SetActive(false);
        }
    }

    private void setItemBubble()
    {
        if (direction == "up" || direction == "right")
        {
            LeftThinkingBubble.SetActive(false);
            RightThinkingBubble.SetActive(false);
            LeftItemBubble.SetActive(false);
            RightItemBubble.SetActive(true);
            LeftAngryBubble.SetActive(false);
            RightAngryBubble.SetActive(false);
        }
        else if (direction == "left")
        {
            LeftThinkingBubble.SetActive(false);
            RightThinkingBubble.SetActive(false);
            LeftItemBubble.SetActive(true);
            RightItemBubble.SetActive(false);
            LeftAngryBubble.SetActive(false);
            RightAngryBubble.SetActive(false);
        }
    }

}
