using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ItemSpawner : MonoBehaviourPun
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
                photonView.RPC("SpawnItem", RpcTarget.AllBuffered, index);
            }
        }
    }

    [PunRPC]
    private void SpawnItem(int locatorIdx)
    {
        if (spawnMap[locators[locatorIdx]] == null)
        {
            Vector3 location = locators[locatorIdx].position;
            GameObject itemSpawned =
                Instantiate(itemPrefab,
                location,
                itemPrefab.transform.rotation);
            spawnMap[locators[locatorIdx]] = itemSpawned;
        }
    }
}
