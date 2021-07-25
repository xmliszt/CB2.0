using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnlimitedGroupControlHandler : MonoBehaviour
{
    public GameConstants constants;

    [Header("Item Types")]

    public Item shopItem;

    [Header("Grab Attributes")]
    public Transform grabDetect;
    public bool held = false;

    [Header("Player Attributes")]
    public SpriteRenderer thoughtBubbleRenderer;

    private ShopHandler shopHandler;

    private GameObject pickedItem; // the item player picked up

    private PlayerInventory inventory;

    private PlayerStatsManager playerStatsManager;

    private PlayerZoneManager playerZoneManager;

    private PlayerController playerController;

    private int layerMask;

    private EntertainmentController entertainmentController;

    private bool available = true;



    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerZoneManager = GetComponent<PlayerZoneManager>();

        layerMask = LayerMask.GetMask("Entertainments");

    }

    private void Start()
    {
        thoughtBubbleRenderer.enabled = false;
        GetComponent<SpriteOutlined>()
            .EnableOutline(playerStatsManager.GetPlayerStats());
        inventory = playerStatsManager.GetPlayerStats().inventory;

        // To Change
        playerStatsManager.GetPlayerStats().coins += 100;
    }

    private void Update()
    {
        
        RaycastHit2D grabCheck = 
        Physics2D.CircleCast(grabDetect.position, constants.castRadius, Vector2.down * transform.localScale, constants.castRadius, layerMask);

        if(grabCheck.collider != null && grabCheck.collider.tag == "Entertainments")
        {
            
            if (available) {
                entertainmentController = grabCheck.collider.gameObject.GetComponent<EntertainmentController>();
                entertainmentController.fromPlayer = gameObject;
                entertainmentController.SetSpriteOutline();
                available = false;
            }
            
            // Can check if entertainment controller is "held" too
            if (held && entertainmentController) {
                // Slow down player & Disable dash
                playerController.SlowMovement(0.3f);
                playerController.DisableDash();

                // Move entertainment object
                entertainmentController.MoveItem();

            }
            
        }
        // Object out of range: Deselect 
        else {
            if (entertainmentController != null) {
                entertainmentController.DisableSpriteOutline();
                entertainmentController = null;
                available = true;
            }
        }

        // Edge case where player is detecting another entertainment: Deselect
        if (grabCheck.collider != null && 
        grabCheck.collider.tag == "Entertainments" && 
        entertainmentController != grabCheck.collider.gameObject.GetComponent<EntertainmentController>()) {
            entertainmentController.DisableSpriteOutline();
            entertainmentController = null;
            available = true;
        }

        if (!held) {
            // Player resumes normal speed and dash
            playerController.EnableMovement();
            playerController.EnableDash();

        }
    }

    public void OnUse()
    {

    }

    public void OnPickUpDrop()
    {

    }


    public void OnShop()
    {
        if (
            !inventory.hasItem() &&
            playerZoneManager.GetZone() == PlayerZoneManager.ZoneType.shop
        )
        {
            Debug.Log("IM USING SHOP");
            ShopItem boughtItem = shopHandler.BuyItem(gameObject);
            if (boughtItem != null)
            {
                inventory.SetItem (shopItem);

                Debug.Log("ITEM BOUGHT");

                thoughtBubbleRenderer.sprite = shopItem.thoughtBubbleSprite;
                thoughtBubbleRenderer.enabled = true;

                Debug.Log(playerStatsManager.GetPlayerStats().coins);
            }
        }
    }

    public void OnHold(InputAction.CallbackContext context)
    {   
        held = context.ReadValueAsButton();
    }

    public void SetPickedItem(GameObject _pickedItem)
    {
        pickedItem = _pickedItem;
    }

    public void SetShopHandler(ShopHandler _shopHandler)
    {
        shopHandler = _shopHandler;
    }

    public void updateScore(int score) {
        playerStatsManager.GetPlayerStats().score += score;
        Debug.Log(playerStatsManager.GetPlayerStats().score);
    }
}
