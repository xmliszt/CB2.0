using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZoneManager : MonoBehaviour
{
    private ZoneType zoneType = ZoneType.nullType;

    private ControlKeyIndicatorHandler controlKeyIndicatorHandler;
    
    private void Awake() {
        controlKeyIndicatorHandler = GetComponent<ControlKeyIndicatorHandler>();
    }

    public enum ZoneType
    {
        nullType = 0,
        droppedItem = 1,
        swabStickCollection = 2,
        testStation = 3,
        submissionStation = 4,
        dustbin = 5,
        shop = 6,
        clothChanger = 7,
        reception = 8,
        musicChanger = 9,
        ugsShop = 10,

        gameSelector = 11,
    }

    public ZoneType GetZone()
    {
        return zoneType;
    }

    public void SetZone(string zoneTag, GameObject zoneObject)
    {
        Debug.Log("Set zone: " + zoneTag);
        switch (zoneTag)
        {
            case "CollectionPoint":
                zoneType = ZoneType.swabStickCollection;
                break;
            case "Dustbin":
                zoneType = ZoneType.dustbin;
                break;
            case "TestStation":
                zoneType = ZoneType.testStation;
                gameObject
                    .GetComponent<SwabTestControlHandler>()
                    .SetTestStationProcessor(zoneObject
                        .GetComponent<TestSampleProcessor>());
                break;
            case "SubmissionDesk":
                zoneType = ZoneType.submissionStation;
                break;
            case "Shop":
                zoneType = ZoneType.shop;
                gameObject
                    .GetComponent<SwabTestControlHandler>()
                    .SetShopHandler(zoneObject.GetComponent<ShopHandler>());
                break;
            case "UGSShop":
                zoneType = ZoneType.ugsShop;
                gameObject
                    .GetComponent<UnlimitedGroupControlHandler>()
                    .SetShopHandler(zoneObject.GetComponent<ShopHandler>());
                break;
            case "SwabStick":
                gameObject.GetComponent<SwabTestControlHandler>().GetStunned();
                break;
            case "Item":
                zoneType = ZoneType.droppedItem;
                gameObject.GetComponent<SwabTestControlHandler>().SetPickedItem(zoneObject);
                break;
            case "ClothChanger":
                zoneType = ZoneType.clothChanger;
                controlKeyIndicatorHandler.TurnOnIndicator(ControllerKeyType.west);
                break;
            case "Reception":
                zoneType = ZoneType.reception;
                controlKeyIndicatorHandler.TurnOnIndicator(ControllerKeyType.west);
                break;
            case "MusicChanger":
                zoneType = ZoneType.musicChanger;
                controlKeyIndicatorHandler.TurnOnIndicator(ControllerKeyType.west);
                break;
            case "null":
                zoneType = ZoneType.nullType;
                controlKeyIndicatorHandler.TurnOffIndiciator();
                break;
            case "GameSelector":
                zoneType = ZoneType.gameSelector;
                controlKeyIndicatorHandler.TurnOnIndicator(ControllerKeyType.west);
                break;
        }
    }
}
