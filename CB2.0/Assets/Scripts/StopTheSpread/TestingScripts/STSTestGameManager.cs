using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class STSTestGameManager : MonoBehaviour
{
    // The list of player profiles to select from
    public PlayerStats[] playerProfiles;

    // Keep track of the spawned player gameobject
    private Dictionary<int, Transform> playerObjects;

    // Start is called before the first frame update
    void Start()
    {
        playerObjects = new Dictionary<int, Transform>();
        //playerInputManager = PlayerInputManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int playerID = playerInput.playerIndex + 1;
        playerObjects[playerID] = playerInput.gameObject.transform;
    }
}
