using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snhPlayerInventory
{
    public SnHPickUps pickup; 

    // constructor
    public snhPlayerInventory()
    {
        pickup = null;
    }

    public SnHPickUps GetCurrentPickup()
    {
        return pickup;
    }

    public void HoldPickup(SnHPickUps _pickup)
    {
        pickup = _pickup;
    }

    public void RemovePickup()
    {
        pickup = null;
    }

    // only for shopItem usage
    public SnHPickUps UsePickup()
    {
        SnHPickUps _pickup = pickup;
        pickup = null;
        return _pickup;
    }

    public bool CheckHoldingPickup()
    {
        return pickup != null;
    }
}

