using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class STSControlHandler : MonoBehaviour
{
    private bool activityOnCooldown = false;

    public STSGameConstants stsGameConstants;

    [SerializeField] private Slider completionBar;

    private ZoneType zoneType = ZoneType.nullType;

    [Header("Activity Types")]
    public STSActivity gym;
    public STSActivity computer;
    public STSActivity karaoke;
    public STSActivity toiletPaper;
    public STSActivity doingNothing;

    private STSActivity[] allActivities;

    [Header("Player Attributes")]
    public SpriteRenderer thoughtBubbleRenderer;

    public STSPlayerInventory stsInventory;

    private PlayerControllerSTS playerControllerSTS;

    private int desiredActivity;

    private float delay;

    private bool playerDoingActivity = false;

    private PlayerStatsManager playerStatsManager;

    private GameObject pickedItem;

    private GameObject InteractableObject;

    [Header("Food Items")]
    public STSFood Pizza;
    public STSFood Cake;

    public SpriteRenderer PizzaSprite;
    public SpriteRenderer CakeSprite;

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
        nullType = 8
    }


    private void Awake()
    {
        playerControllerSTS = GetComponent<PlayerControllerSTS>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        delay = stsGameConstants.activityDelay;

        allActivities = new STSActivity[4] { gym, computer, karaoke, toiletPaper };
    }

    // Start is called before the first frame update
    void Start()
    {
        desiredActivity = UnityEngine.Random.Range(0, 4);
        thoughtBubbleRenderer.sprite = allActivities[desiredActivity].thoughtBubbleSprite;

        GetComponent<SpriteOutlined>()
            .EnableOutline(playerStatsManager.GetPlayerStats());

        completionBar.value = 0;
        completionBar.gameObject.SetActive(false);

        CakeSprite.enabled = false;
        PizzaSprite.enabled = false;

        stsInventory.holdingItem = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void generateActivity()
    {
        Debug.Log("Generating new activity");
        desiredActivity = UnityEngine.Random.Range(0, 4);
        
        thoughtBubbleRenderer.sprite = allActivities[desiredActivity].thoughtBubbleSprite;
        if (!playerDoingActivity)
        {
            thoughtBubbleRenderer.enabled = true;
        }
        activityOnCooldown = false;
    }

    public void OnUse()
    {
        // Check taking item from grocer
        if(zoneType == ZoneType.grocerCake)
        {
            // can only pick up if player not holding anything
            if(!stsInventory.holdingItem)
            {
                stsInventory.holdingItem = true;
                stsInventory.inventoryItemType = STSPlayerInventory.InventoryItemType.cake;
                
                CakeSprite.enabled = true;
                
                thoughtBubbleRenderer.enabled = false;
                playerDoingActivity = true;
            }
        }
        if(zoneType == ZoneType.grocerPizza)
        {
            if (!stsInventory.holdingItem)
            {
                stsInventory.holdingItem = true;
                stsInventory.inventoryItemType = STSPlayerInventory.InventoryItemType.pizza;
                
                PizzaSprite.enabled = true;
                
                thoughtBubbleRenderer.enabled = false;
                playerDoingActivity = true;
            }
        }

        if(zoneType == ZoneType.npcCake)
        {
            Debug.Log(stsInventory.inventoryItemType);
            if(stsInventory.holdingItem && stsInventory.inventoryItemType == STSPlayerInventory.InventoryItemType.cake)
            {
                playerStatsManager.GetPlayerStats().coins++;
                stsInventory.inventoryItemType = STSPlayerInventory.InventoryItemType.nullItem;
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
            if (stsInventory.holdingItem && stsInventory.inventoryItemType == STSPlayerInventory.InventoryItemType.pizza)
            {
                playerStatsManager.GetPlayerStats().coins++;
                stsInventory.inventoryItemType = STSPlayerInventory.InventoryItemType.nullItem;
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
        if (zoneType == ZoneType.dumbbell || zoneType == ZoneType.computer || zoneType == ZoneType.karaoke || zoneType == ZoneType.toiletPaper)
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

    IEnumerator ActivityCooldown()
    {
        thoughtBubbleRenderer.sprite = doingNothing.thoughtBubbleSprite;
        yield return new WaitForSeconds(stsGameConstants.activityCooldownTime);
        generateActivity();
    }

    IEnumerator UsingInteractable()
    {
        int useState = InteractableObject.GetComponent<InteractableGameObjects>().PlayerUsing();
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
            yield return new WaitForSeconds(stsGameConstants.doingActivityInterval);
            if (zoneType == ZoneType.nullType)
            {
                Debug.Log("Player stopped halfway");
                playerDoingActivity = false;

                completionBar.value = 0;
                completionBar.gameObject.SetActive(false);

                yield break;
            }
        }

        completionBar.value = 0;
        completionBar.gameObject.SetActive(false);

        thoughtBubbleRenderer.enabled = false;
        playerDoingActivity = false;

        playerStatsManager.GetPlayerStats().score++;
        InteractableObject.GetComponent<InteractableGameObjects>().PlayerStopUsing();

        Debug.Log("Player completed activity");

        activityOnCooldown = true;

        StartCoroutine(ActivityCooldown());
    }

    public void PlayerOnTriggerEnterInteractable(String otherTag, GameObject otherGameObject)
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
        }
    }

    public void PlayerOnTriggerExitInteractable()
    {
        zoneType = ZoneType.nullType;
        InteractableObject = null;
    }
}
