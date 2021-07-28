using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STSBirthdayActivity : MonoBehaviour
{
    [Header("Game Events")]
    public SingleIntegerGameEvent birthdayGameEvent;
    public SingleIntegerGameEvent birthdayScoreEvent; // event will give score to players that are present
    public GameEvent birthdayComplete;

    [Header("Others")]

    public STSGameConstants stsGameConstants;

    private STSGameManager stsGameManager;
    private int numberOfPlayers;
    private int playerChosen;

    private bool birthdayEventOngoing = false;

    private bool[] playersPresent = new bool[4] { false, false, false, false };
    private string[] allRooms = new string[4] { "Player 1 House", "Player 2 House", "Player 3 House", "Player 4 House" };

    private int allPresent;

    private bool celebrationsUnderway = false;

    [Header("Doors")]
    public BoxCollider2D player1Door;
    public BoxCollider2D player2Door;
    public BoxCollider2D player3Door;
    public BoxCollider2D player4Door;

    private BoxCollider2D[] playerDoors;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActivateBirthdayEvent());
        playerDoors = new BoxCollider2D[4] { player1Door, player2Door, player3Door, player4Door };
        stsGameManager = FindObjectOfType<STSGameManager>();
        numberOfPlayers = stsGameManager.GetNumberPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        if (birthdayEventOngoing)
        {
            for(int i = 0; i < numberOfPlayers; i++)
            {
                if (playersPresent[i] == true)
                {
                    allPresent++;
                }
            }

            //Debug.Log("All present: " + allPresent);
            //Debug.Log("Number of players: " + numberOfPlayers);

            if(allPresent == numberOfPlayers && !celebrationsUnderway)
            {
                allPresent = 0;
                celebrationsUnderway = true;
                playerDoors[playerChosen-1].gameObject.SetActive(true);
                BeginBirthdayCelebration();
            }

            allPresent = 0;
        }
    }

    IEnumerator ActivateBirthdayEvent()
    {
        while (true)
        {
            yield return new WaitForSeconds(stsGameConstants.waitForBirthday);
            if (!birthdayEventOngoing)
            {
                Debug.Log("Starting new birthday event");

                invokeBirthdayGameEvent();
                StartCoroutine(birthdayTimeout());
            }
        }
    }

    IEnumerator birthdayTimeout()
    {
        yield return new WaitForSeconds(stsGameConstants.birthdayInterval * 8);
        if (birthdayEventOngoing)
        {
            OnBirthdayEventOver();
        }
    }

    private void invokeBirthdayGameEvent()
    {
        birthdayEventOngoing = true;
        playerChosen = Random.Range(1, numberOfPlayers);

        birthdayGameEvent.Fire(playerChosen);
    }

    private void OnBirthdayEventOver()
    {
        Debug.Log("Birthday over, resetting");
        playerDoors[playerChosen - 1].gameObject.SetActive(false);
        birthdayEventOngoing = false;
        celebrationsUnderway = false;

        // give score to players that are present in the room
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (playersPresent[i] == true)
            {
                // fire event, players will listen to this event and score accordingly
                birthdayScoreEvent.Fire(i + 1);
            }
        }
    }

    private void BeginBirthdayCelebration()
    {
        // we should have like 3 seconds to sing birthday song or something
        // if players leave the room halfway the thing will stop
        StartCoroutine(CarryingOutBirthdayCelebration());
        Debug.Log("all players present, birthday celebrations has begun");
    }

    IEnumerator CarryingOutBirthdayCelebration()
    {
        yield return new WaitForSeconds(stsGameConstants.birthdaySongDuration);
        birthdayComplete.Fire();
        OnBirthdayEventOver();
    }

    public void PlayerEnteredRoom(int playerID, string roomName)
    {
        try
        {
            if (allRooms[playerChosen - 1] == roomName)
            {
                Debug.Log("Player " + playerID + " entered " + roomName);
                switch (playerID)
                {
                    case 1:
                        playersPresent[0] = true;
                        break;
                    case 2:
                        playersPresent[1] = true;
                        break;
                    case 3:
                        playersPresent[2] = true;
                        break;
                    case 4:
                        playersPresent[3] = true;
                        break;
                }
            }
        }
        catch 
        {
            Debug.Log("Not yet birthday event");
        }
    }

    public void PlayerExitedRoom(int playerID, string roomName)
    {
        try
        {
            if (allRooms[playerChosen - 1] == roomName)
            {
                Debug.Log("Player " + playerID + " left room " + roomName);
                switch (playerID)
                {
                    case 1:
                        playersPresent[0] = false;
                        break;
                    case 2:
                        playersPresent[1] = false;
                        break;
                    case 3:
                        playersPresent[2] = false;
                        break;
                    case 4:
                        playersPresent[3] = false;
                        break;
                }
            }
        }
        catch
        {
            Debug.Log("Not yet birthday event");
        }
    }
}
