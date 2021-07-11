using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnlimitedGroupControlHandler : MonoBehaviour
{
    public GameConstants constants;

    [Header("Grab Attributes")]
    public Transform grabDetect;
    public bool held = false;

    private ShopHandler shopHandler;

    private GameObject pickedItem; // the item player picked up

    private PlayerInventory inventory;

    private PlayerStatsManager playerStatsManager;

    private PlayerZoneManager playerZoneManager;

    private PlayerController playerController;

    private int layerMask;



    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerZoneManager = GetComponent<PlayerZoneManager>();

        layerMask = LayerMask.GetMask("Entertainments");

    }

    private void Start()
    {

    }

    private void Update()
    {
        
        RaycastHit2D grabCheck = 
        Physics2D.CircleCast(grabDetect.position, constants.castRadius, Vector2.down * transform.localScale, constants.castRadius, layerMask);

        if(grabCheck.collider != null && grabCheck.collider.tag == "Entertainments")
        {
            
            if (held) {
                EntertainmentController entertainmentScript = 
                grabCheck.collider.gameObject.GetComponent<EntertainmentController>();

                entertainmentScript.fromPlayer = gameObject;
                entertainmentScript.MoveItem();

            }
            
        }
    }

    public void OnUse()
    {

    }

    public void onPickUpDrop()
    {

    }


    public void onShop()
    {

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
}
