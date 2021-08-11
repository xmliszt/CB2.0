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
    private Dictionary<SPZC, List<Vector3>> spawnMap;

    // possible item types to spawn
    private PickUpTypeEnum[] Spawnable = new PickUpTypeEnum[] {PickUpTypeEnum.toiletPaper,
                                                                PickUpTypeEnum.chicken,
                                                                PickUpTypeEnum.fish,
                                                                PickUpTypeEnum.cherry,
                                                                PickUpTypeEnum.broccoli};

    // parent transform
    public Transform parent;

    private void Start() {
        spawnMap = new Dictionary<SPZC, List<Vector3>>();
        foreach (SPZC zones in snhGameConstants.spawnZones)
        {
            spawnMap[zones] = new List<Vector3>();
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
                PickUpTypeEnum _pickupType = Spawnable[Random.Range(0, Spawnable.Length)];
                
                // instantiate and set the pickup type
                GameObject SpawnedPickup = Instantiate(pickupPrefab, location, pickupPrefab.transform.rotation, parent);
                SpawnedPickup.GetComponent<SnHPickUpController>().SetPickUp(_pickupType);

                // save to list
                spawnMap[snhGameConstants.spawnZones[selectedSpawnZone]].Add(SpawnedPickup.transform.position);
            }
        }
    }

    // remove the reference from the list
    public void PickupDestroyed(Vector3 destroyedPickupPosition)
    {
        foreach (List<Vector3> pu in spawnMap.Values)
        {
            if (pu.Contains(destroyedPickupPosition))
            {
                pu.Remove(destroyedPickupPosition);
            }
        }
    }

    // when pickup is dropped on the ground
    public void PickupAdded(Vector3 location)
    {

        // check which spawn area is appropriate
        foreach(SPZC area in spawnMap.Keys)
        {
            // if location matches
            if (location.x >= area.xMin && location.x <= area.xMax && location.y >= area.yMin && location.y <= area.yMax)
            {
                spawnMap[area].Add(location);
            }
        }
    }
}
