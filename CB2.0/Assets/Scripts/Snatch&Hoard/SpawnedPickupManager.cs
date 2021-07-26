using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedPickupManager : MonoBehaviour
{
    // read the spawn zone info
    public SnHGameConstants snhGameConstants;

    // pickup prefab
    public GameObject pickupPrefab;

    // dictionary to track the number of pickups spawned per area. should only have a max of 2
    private Dictionary<SPZC, List<GameObject>> spawnMap;

    // possible item types to spawn
    public SnHPickUps[] Spawnable;

    // parent transform
    public Transform parent;


    // Change to onStart later
    public void onStart()
    {
        spawnMap = new Dictionary<SPZC, List<GameObject>>();
        foreach (SPZC zones in snhGameConstants.spawnZones)
        {
            spawnMap[zones] = new List<GameObject>();
        }
        StartCoroutine(SpawnPickups());
    }

    // keep spawning
    IEnumerator SpawnPickups()
    {
        while (true)
        {
            yield return new WaitForSeconds(snhGameConstants.spawnFrequency);
            int selectedSpawnZone = Random.Range(0, snhGameConstants.spawnZones.Length - 1);

            // spawn if less than limit
            if (spawnMap[snhGameConstants.spawnZones[selectedSpawnZone]].Count < snhGameConstants.spawnPerZone)
            {
                // select zone
                SPZC zone = snhGameConstants.spawnZones[selectedSpawnZone];
                float x = Random.Range(zone.xMin, zone.xMax);
                float y = Random.Range(zone.yMin, zone.yMax);
                Vector3 location = new Vector3(x, y, 0);

                // select item type
                SnHPickUps.PickUpType _pickupType = Spawnable[Random.Range(0, Spawnable.Length)].pickUpType;
                
                // instantiate and set the pickup type
                GameObject SpawnedPickup = Instantiate(pickupPrefab, location, pickupPrefab.transform.rotation, parent);
                SpawnedPickup.GetComponent<SnHPickUpController>().SetPickUp(_pickupType);
                Debug.Log("spawned new");

                // save to list
                spawnMap[snhGameConstants.spawnZones[selectedSpawnZone]].Add(SpawnedPickup);
            }
        }
    }

    // remove the reference from the list
    public void PickupDestroyed(GameObject destroyedPickup)
    {
        foreach (List<GameObject> pu in spawnMap.Values)
        {
            if (pu.Contains(destroyedPickup))
            {
                pu.Remove(destroyedPickup);
            }
        }
    }

    // when pickup is dropped on the ground
    public void PickupAdded(GameObject newPickup)
    {
        // get the location of the pickup
        Vector3 location = newPickup.transform.position;

        // check which spawn area is appropriate
        foreach(SPZC area in spawnMap.Keys)
        {
            // if location matches
            if (location.x >= area.xMin && location.x <= area.xMax && location.y >= area.yMin && location.y <= area.yMax)
            {
                spawnMap[area].Add(newPickup);
            }
        }
    }
}
