using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EntertainmentSpawner : MonoBehaviourPun
{
    public GameObject[] prefabList;

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
                yield return new WaitForSeconds(constants
                            .entertainmentSpawnFreq);
                int index = Random.Range(0, locators.Length);
                photonView.RPC("SpawnItem", RpcTarget.AllBuffered, index);
            }
        }
    }

    [PunRPC]
    private void SpawnItem(int index)
    {
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
