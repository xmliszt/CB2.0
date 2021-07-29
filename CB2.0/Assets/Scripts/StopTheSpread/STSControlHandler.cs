using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STSControlHandler : MonoBehaviour
{
    private bool activityOnCooldown = false;

    public GameEvent STSTearDown;

    public STSGameConstants stsGameConstants;

    [SerializeField]
    private Slider completionBar;

    private ZoneType zoneType = ZoneType.nullType;

    [Header("Activity Types")]
    public STSActivity gym;

    public STSActivity computer;

    public STSActivity karaoke;

    public STSActivity toiletPaper;

    public STSActivity birthday;

    public STSActivity doingNothing;

    public STSActivity otherPeopleBirthday;

    private STSActivity[] allActivities;

    [Header("Player Attributes")]
    public SpriteRenderer thoughtBubbleRenderer;

    public SpriteRenderer stunnedIconRenderer;

    public SpriteRenderer playerSprite;

    private bool playerInvisible = false;

    public STSPlayerInventory stsInventory;

    public Image completionBarFill;

    private PlayerController playerController;

    private int desiredActivity;

    private float delay;

    private bool playerDoingActivity = false;

    private PlayerStatsManager playerStatsManager;

    private int playerID;

    private bool birthdayEventOngoing = false;

    private GameObject pickedItem;

    private GameObject InteractableObject;

    [Header("Food Items")]
    public STSFood Pizza;

    public STSFood Cake;

    public SpriteRenderer PizzaSprite;

    public SpriteRenderer CakeSprite;

    private STSItem stsItem;

    [Header("Physical Item Prefab")]
    public GameObject droppedPizzaPrefab;

    public GameObject droppedCakePrefab;

    private bool autoPickEnabled = true;

    private enum ZoneType
    {
        dumbbell = 0,
        computer = 1,
        karaoke = 2,
        toiletPaper = 3,
        grocerCake = 4,
        grocerPizza = 5,
        npcCake = 6,
        npcPizza = 7,
        droppedCake = 8,
        droppedPizza = 9,
        nullType = 10,
        playerOneHome = 11,
        playerTwoHome = 12,
        playerThreeHome = 13,
        playerFourHome = 14,
        shop = 15
    }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        delay = stsGameConstants.activityDelay;

        allActivities =
            new STSActivity[4] { gym, computer, karaoke, toiletPaper };
    }

    // Start is called before the first frame update
    void Start()
    {
        stunnedIconRenderer.enabled = false;

        GetComponent<SpriteOutlined>()
            .EnableOutline(playerStatsManager.GetPlayerStats());

        playerID = playerStatsManager.GetPlayerStats().playerID;

        completionBar.value = 0;
        completionBar.gameObject.SetActive(false);

        CakeSprite.enabled = false;
        PizzaSprite.enabled = false;
        stsInventory.holdingItem = false;
        stsItem = stsInventory.stsItem;
        completionBarFill.color =
            playerStatsManager.GetPlayerStats().playerAccent;
    }

    public void GenerateFirstActivity()
    {
        generateActivity();
    }

    private void generateActivity()
    {
        if (!birthdayEventOngoing)
        {
            desiredActivity = UnityEngine.Random.Range(0, 4);

            thoughtBubbleRenderer.sprite =
                allActivities[desiredActivity].thoughtBubbleSprite;
            if (!playerDoingActivity)
            {
                thoughtBubbleRenderer.enabled = true;
            }
            activityOnCooldown = false;
        }
    }

    public void OnUse()
    {
        if (zoneType == ZoneType.npcCake)
        {
            if (
                stsInventory.holdingItem &&
                stsInventory.inventoryItemType ==
                STSPlayerInventory.InventoryItemType.cake
            )
            {
                playerStatsManager.GetPlayerStats().coins++;
                stsInventory.inventoryItemType =
                    STSPlayerInventory.InventoryItemType.nullItem;
                stsInventory.holdingItem = false;

                InteractableObject.GetComponent<Customer>().PlayerGaveFood();

                CakeSprite.enabled = false;

                if (!activityOnCooldown)
                {
                    thoughtBubbleRenderer.enabled = true;
                }
                playerDoingActivity = false;
            }
        }

        if (zoneType == ZoneType.npcPizza)
        {
            if (
                stsInventory.holdingItem &&
                stsInventory.inventoryItemType ==
                STSPlayerInventory.InventoryItemType.pizza
            )
            {
                playerStatsManager.GetPlayerStats().coins++;
                stsInventory.inventoryItemType =
                    STSPlayerInventory.InventoryItemType.nullItem;
                stsInventory.holdingItem = false;

                InteractableObject.GetComponent<Customer>().PlayerGaveFood();

                PizzaSprite.enabled = false;

                if (!activityOnCooldown)
                {
                    thoughtBubbleRenderer.enabled = true;
                }
                playerDoingActivity = false;
            }
        }

        // should be using Enum.IsDefined, but then cause cannot check Enum of different type so using this inefficient method as of now
        if (
            zoneType == ZoneType.dumbbell ||
            zoneType == ZoneType.computer ||
            zoneType == ZoneType.karaoke ||
            zoneType == ZoneType.toiletPaper
        )
        {
            if (!playerDoingActivity)
            {
                switch (zoneType)
                {
                    case ZoneType.dumbbell:
                        if (desiredActivity == 0 && !activityOnCooldown)
                        {
                            StartCoroutine(UsingInteractable());
                        }
                        break;
                    case ZoneType.computer:
                        if (desiredActivity == 1 && !activityOnCooldown)
                        {
                            StartCoroutine(UsingInteractable());
                        }
                        break;
                    case ZoneType.karaoke:
                        if (desiredActivity == 2 && !activityOnCooldown)
                        {
                            StartCoroutine(UsingInteractable());
                        }
                        break;
                    case ZoneType.toiletPaper:
                        if (desiredActivity == 3 && !activityOnCooldown)
                        {
                            StartCoroutine(UsingInteractable());
                        }
                        break;
                }
            }
        }
    }

    public void onShop()
    {
        if (zoneType == ZoneType.shop)
        {
            // do shop stuff
            if (playerStatsManager.GetPlayerStats().coins > 0)
            {
                playerStatsManager.GetPlayerStats().coins--;
                playerInvisible = true;
                var tempColor = playerSprite.color;
                tempColor.a = 0.2f;
                playerSprite.color = tempColor;

                StartCoroutine(InvisibilityTimeUp(stsGameConstants
                    .invisibilityTime));
            }
        }
    }

    public bool GetPlayerInvisibilty()
    {
        return playerInvisible;
    }

    IEnumerator InvisibilityTimeUp(float duration)
    {
        yield return new WaitForSeconds(duration);

        var tempColor = playerSprite.color;
        tempColor.a = 1f;
        playerSprite.color = tempColor;
        playerInvisible = false;
    }

    public void onPickUpDrop()
    {
        // if not holding item, check
        if (!stsInventory.holdingItem)
        {
            // Check taking item from grocer
            if (zoneType == ZoneType.grocerCake)
            {
                stsItem.SetFood (Cake);
                stsInventory.holdingItem = true;
                stsInventory.inventoryItemType =
                    STSPlayerInventory.InventoryItemType.cake;

                CakeSprite.enabled = true;

                thoughtBubbleRenderer.enabled = false;
                playerDoingActivity = true;
            }
            if (zoneType == ZoneType.grocerPizza)
            {
                stsItem.SetFood (Pizza);
                stsInventory.holdingItem = true;
                stsInventory.inventoryItemType =
                    STSPlayerInventory.InventoryItemType.pizza;

                PizzaSprite.enabled = true;

                thoughtBubbleRenderer.enabled = false;
                playerDoingActivity = true;
            }

            // try to pick up dropped food item
            if (zoneType == ZoneType.droppedCake)
            {
                InteractableObject
                    .GetComponent<InteractableGameObjects>()
                    .DestroyThis();
                stsItem.SetFood (Cake);
                stsInventory.holdingItem = true;
                stsInventory.inventoryItemType =
                    STSPlayerInventory.InventoryItemType.cake;

                CakeSprite.enabled = true;

                thoughtBubbleRenderer.enabled = false;
                playerDoingActivity = true;
            }

            if (zoneType == ZoneType.droppedPizza)
            {
                InteractableObject
                    .GetComponent<InteractableGameObjects>()
                    .DestroyThis();
                stsItem.SetFood (Pizza);
                stsInventory.holdingItem = true;
                stsInventory.inventoryItemType =
                    STSPlayerInventory.InventoryItemType.pizza;

                PizzaSprite.enabled = true;

                thoughtBubbleRenderer.enabled = false;
                playerDoingActivity = true;
            }
        }
        else
        // if holding item, drop it
        {
            DropItem();
            autoPickEnabled = false;
            StartCoroutine(EnableAutoPickUp(1));
        }
    }

    IEnumerator EnableAutoPickUp(float delay)
    {
        yield return new WaitForSeconds(delay);
        autoPickEnabled = true;
    }

    private void DropItem()
    {
        if (stsInventory.holdingItem)
        {
            STSFood _food = stsItem.UseFood();
            GameObject droppedFoodPrefab;

            if ((int) _food.foodType == 1)
            {
                droppedFoodPrefab = droppedPizzaPrefab;
            }
            else
            {
                droppedFoodPrefab = droppedCakePrefab;
            }

            Vector2 idleDirection = playerController.GetIdleDirection();
            GameObject dropped =
                Instantiate(droppedFoodPrefab,
                transform.position +
                new Vector3(idleDirection.x,
                    idleDirection.y,
                    droppedFoodPrefab.transform.position.z) *
                0.2f,
                droppedFoodPrefab.transform.rotation);
            dropped.GetComponent<CollectableFood>().SetFood(_food);

            CakeSprite.enabled = false;
            PizzaSprite.enabled = false;

            stsInventory.holdingItem = false;
        }
    }

    IEnumerator ActivityCooldown()
    {
        if (!birthdayEventOngoing)
        {
            thoughtBubbleRenderer.sprite = doingNothing.thoughtBubbleSprite;
        }
        yield return new WaitForSeconds(stsGameConstants.activityCooldownTime);
        generateActivity();
    }

    IEnumerator UsingInteractable()
    {
        int useState =
            InteractableObject
                .GetComponent<InteractableGameObjects>()
                .PlayerUsing();
        if (useState == 1)
        {
            // if someone else is using, we cannot use
            yield break;
        }

        completionBar.gameObject.SetActive(true);
        playerDoingActivity = true;

        // wait for 3 seconds, check every 0.5s if the person is out of the box
        for (int i = 0; i < stsGameConstants.intervals; i++)
        {
            completionBar.value += stsGameConstants.doingActivityInterval;
            yield return new WaitForSeconds(stsGameConstants
                        .doingActivityInterval);
            if (zoneType == ZoneType.nullType)
            {
                playerDoingActivity = false;

                completionBar.value = 0;
                completionBar.gameObject.SetActive(false);

                yield break;
            }
        }

        completionBar.value = 0;
        completionBar.gameObject.SetActive(false);

        playerDoingActivity = false;

        playerStatsManager.GetPlayerStats().score++;
        InteractableObject
            .GetComponent<InteractableGameObjects>()
            .PlayerStopUsing();

        activityOnCooldown = true;

        StartCoroutine(ActivityCooldown());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SDA"))
        {
            if (!playerInvisible)
            {
                // send the player to their home, with 3 seconds waiting time before they can move
                gameObject.GetComponent<PlayerController>().DisableController();
                StartCoroutine(EnabledControllerAgain(3));

                // send player home
                Vector3 respawnLocation =
                    FindObjectOfType<STSGameManager>()
                        .GetPlayerLocation(playerID);
                gameObject.transform.position = respawnLocation;

                playerSprite.enabled = false;
                thoughtBubbleRenderer.enabled = false;
            }
        }
    }

    IEnumerator EnabledControllerAgain(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.GetComponent<PlayerController>().EnableController();
        playerSprite.enabled = true;
        thoughtBubbleRenderer.enabled = true;
    }

    public void PlayerOnTriggerEnterInteractable(
        String otherTag,
        GameObject otherGameObject
    )
    {
        InteractableObject = otherGameObject;

        switch (otherTag)
        {
            case "Dumbbell":
                zoneType = ZoneType.dumbbell;
                break;
            case "Computer":
                zoneType = ZoneType.computer;
                break;
            case "Karaoke":
                zoneType = ZoneType.karaoke;
                break;
            case "ToiletPaper":
                zoneType = ZoneType.toiletPaper;
                break;
            case "GrocerCake":
                zoneType = ZoneType.grocerCake;
                break;
            case "GrocerPizza":
                zoneType = ZoneType.grocerPizza;
                break;
            case "NPCCake":
                zoneType = ZoneType.npcCake;
                break;
            case "NPCPizza":
                zoneType = ZoneType.npcPizza;
                break;
            case "DroppedCake":
                zoneType = ZoneType.droppedCake;
                break;
            case "DroppedPizza":
                zoneType = ZoneType.droppedPizza;
                break;
            case "PlayerOneHome":
                zoneType = ZoneType.playerOneHome;
                break;
            case "PlayerTwoHome":
                zoneType = ZoneType.playerTwoHome;
                break;
            case "PlayerThreeHome":
                zoneType = ZoneType.playerThreeHome;
                break;
            case "PlayerFourHome":
                zoneType = ZoneType.playerFourHome;
                break;
            case "Shop":
                zoneType = ZoneType.shop;
                break;
        }
    }

    public void PlayerOnTriggerExitInteractable()
    {
        zoneType = ZoneType.nullType;
        InteractableObject = null;
    }

    // Birthday event will override all other activities
    public void BirthdayEventActivated(int birthdayChild)
    {
        birthdayEventOngoing = true;
        GetComponentInChildren<STSTimer>().BirthdayEventActivated();

        // it's my birthday
        if (birthdayChild == playerID)
        {
            thoughtBubbleRenderer.sprite = birthday.thoughtBubbleSprite;
            thoughtBubbleRenderer.enabled = true;
            activityOnCooldown = true;
        }
        else
        // not my birthday, put some other player sprite
        {
            thoughtBubbleRenderer.sprite =
                otherPeopleBirthday.thoughtBubbleSprite;
            thoughtBubbleRenderer.enabled = true;
            activityOnCooldown = true;
        }

        StartCoroutine(BirthdayTimeout());
    }

    public void BirthdayScoreEvent(int _playerID)
    {
        if (playerID == _playerID)
        {
            playerStatsManager.GetPlayerStats().score += 2;
        }
    }

    // method to be called when celebrations are over so player don't have to wait 8 * birthdayInterval
    public void birthdayCelebrationsCompleted()
    {
        if (birthdayEventOngoing)
        {
            ResetBirthdayValues();
        }
    }

    private void ResetBirthdayValues()
    {
        if (birthdayEventOngoing)
        {
            GetComponentInChildren<STSTimer>().BirthdayEventCompleted();
            activityOnCooldown = false;
            birthdayEventOngoing = false;
            generateActivity();
        }
    }

    IEnumerator BirthdayTimeout()
    {
        yield return new WaitForSeconds(stsGameConstants.birthdayInterval * 8);
        ResetBirthdayValues();
    }

    public int GetPlayerID()
    {
        return playerID;
    }

    public bool IsPlayerHome()
    {
        switch (playerID)
        {
            case 1:
                if (zoneType == ZoneType.playerOneHome) return true;
                break;
            case 2:
                if (zoneType == ZoneType.playerTwoHome) return true;
                break;
            case 3:
                if (zoneType == ZoneType.playerThreeHome) return true;
                break;
            case 4:
                if (zoneType == ZoneType.playerFourHome) return true;
                break;
        }
        return false;
    }

    // Reset all minigame-specific player appearance
    public void onMinigameOver()
    {
        thoughtBubbleRenderer.enabled = false;
        stunnedIconRenderer.enabled = false;
        CakeSprite.enabled = false;
        PizzaSprite.enabled = false;
        completionBar.gameObject.SetActive(false);
        var tempColor = playerSprite.color;
        tempColor.a = 1.0f;
        playerSprite.color = tempColor;
        StopAllCoroutines();
        STSTearDown.Fire();
    }
}
