using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;

    private Transform[] locators;

    public GameConstants constants;

    private Dictionary<Transform, GameObject> spawnMap;

    private bool isPaused = false;

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

    public void PauseSpawning()
    {
        isPaused = !isPaused;
    }

    IEnumerator StartSpawning()
    {
        while (true)
        {
            if (isPaused)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(constants.spawnFrequency);
                int index = Random.Range(0, locators.Length);
                if (spawnMap[locators[index]] == null)
                {
                    Vector3 location = locators[index].position;
                    GameObject itemSpawned =
                        Instantiate(itemPrefab,
                        location,
                        itemPrefab.transform.rotation);
                    spawnMap[locators[index]] = itemSpawned;
                }
            }
        }
    }
}
