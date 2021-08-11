using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnHNPCManager : MonoBehaviour
{
    public SnHGameConstants snhGameConstants;

    public GameObject npcPrefab;

    private Dictionary<NPCZC, int> npcSpawnMap;

    public Transform parent;

    private int currentlySpawned = 0;

    public void onStart()
    {
        npcSpawnMap = new Dictionary<NPCZC, int>();
        foreach (NPCZC zones in snhGameConstants.npcZones)
        {
            npcSpawnMap[zones] = 0;
        }

        while (currentlySpawned != snhGameConstants.NPCs)
        {
            // randomly select an area
            int idx = Random.Range(0, snhGameConstants.npcZones.Length - 1);

            // spawn if nothing yet
            if (npcSpawnMap[snhGameConstants.npcZones[idx]] == 0)
            {
                NPCZC zone = snhGameConstants.npcZones[idx];
                Vector3 location = new Vector3(zone.x, zone.y, 0);

                // instantiate the prefab
                GameObject npc = Instantiate(npcPrefab, location, npcPrefab.transform.rotation, parent);
                npc.GetComponent<SnHNPCController>().direction = zone.direction;
                npc.GetComponent<SnHNPCController>().onStart();

                // update the counter
                currentlySpawned += 1;
                npcSpawnMap[snhGameConstants.npcZones[idx]] = 1;
            }
        }
    }
}
