using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntertainmentSpawner : MonoBehaviour
{
    public GameObject[] prefabList;

    private Transform[] locators;

    public GameConstants constants;

    private Dictionary<Transform, GameObject> spawnMap;

    private void Start()
    {
        spawnMap = new Dictionary<Transform, GameObject>();
        locators = GetComponentsInChildren<Transform>();
        foreach (Transform locator in locators)
        {
            spawnMap[locator] = null;
        }
        StartCoroutine(StartSpawning());
    }

    IEnumerator StartSpawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(constants.entertainmentSpawnFreq);
            int index = Random.Range(0, locators.Length);
            
            if (spawnMap[locators[index]] == null)
            {
                int prefabIndex = Random.Range(0, prefabList.Length);
                Vector3 location = locators[index].position;
                GameObject itemSpawned =
                    Instantiate(prefabList[prefabIndex],
                    location,
                    prefabList[prefabIndex].transform.rotation);
                spawnMap[locators[index]] = itemSpawned;
            }
        }
    }
}
