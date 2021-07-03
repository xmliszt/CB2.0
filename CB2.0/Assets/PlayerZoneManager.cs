using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZoneManager : MonoBehaviour
{
    private ZoneType zoneType = ZoneType.nullType;

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
    }

    public ZoneType GetZone()
    {
        return zoneType;
    }

    public void SetZone(string zoneTag, GameObject zoneObject)
    {
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
                    .GetComponent<SwabTestPlayerController>()
                    .testStationProcessor =
                    zoneObject.GetComponent<TestSampleProcessor>();
                break;
            case "SubmissionDesk":
                zoneType = ZoneType.submissionStation;
                break;
            case "Shop":
                zoneType = ZoneType.shop;
                gameObject
                    .GetComponent<SwabTestPlayerController>()
                    .shopHandler = zoneObject.GetComponent<ShopHandler>();
                break;
            case "SwabStick":
                gameObject
                    .GetComponent<SwabTestPlayerController>()
                    .GetStunned();
                break;
            case "Item":
                zoneType = ZoneType.droppedItem;
                gameObject.GetComponent<SwabTestPlayerController>().pickedItem =
                    zoneObject;
                break;
            case "ClothChanger":
                zoneType = ZoneType.clothChanger;
                break;
            case "Reception":
                zoneType = ZoneType.reception;
                break;
            case "MusicChanger":
                zoneType = ZoneType.musicChanger;
                break;
            case "null":
                zoneType = ZoneType.nullType;
                break;
        }
    }
}
