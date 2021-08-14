using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Player Panels")]
    public SpriteRenderer player1Panel;
    public SpriteRenderer player2Panel;
    public SpriteRenderer player3Panel;
    public SpriteRenderer player4Panel;

    private SpriteRenderer changingPanel;

    private float panelAlpha = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActivateBirthdayEvent());
        playerDoors = new BoxCollider2D[4] { player1Door, player2Door, player3Door, player4Door };
        stsGameManager = FindObjectOfType<STSGameManager>();
        numberOfPlayers = stsGameManager.GetNumberPlayers();

        player1Panel.enabled = false;
        player2Panel.enabled = false;
        player3Panel.enabled = false;
        player4Panel.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (birthdayEventOngoing)
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                if (playersPresent[i] == true)
                {
                    allPresent++;
                }
            }
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

    IEnumerator blinkPanel()
    {
        while (birthdayEventOngoing)
        {
            changingPanel.enabled = !changingPanel.enabled;
            yield return new WaitForSeconds(0.5f);
        }
        yield break;
    }

    IEnumerator ActivateBirthdayEvent()
    {
        while (true)
        {
            yield return new WaitForSeconds(stsGameConstants.waitForBirthday);
            if (!birthdayEventOngoing)
            {
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
        playerChosen = Random.Range(1, numberOfPlayers+1);

        if(playerChosen == 1)
        {
            player1Panel.enabled = true;
            changingPanel = player1Panel;
        }
        if (playerChosen == 2)
        {
            player2Panel.enabled = true;
            changingPanel = player2Panel;
        }
        if (playerChosen == 3)
        {
            player3Panel.enabled = true;
            changingPanel = player3Panel;
        }
        if (playerChosen == 4)
        {
            player4Panel.enabled = true;
            changingPanel = player4Panel;
        }

        StartCoroutine(blinkPanel());

        birthdayGameEvent.Fire(playerChosen);
    }

    private void OnBirthdayEventOver()
    {
        playerDoors[playerChosen - 1].gameObject.SetActive(false);
        birthdayEventOngoing = false;

        player1Panel.enabled = false;
        player2Panel.enabled = false;
        player3Panel.enabled = false;
        player4Panel.enabled = false;
        changingPanel = null;

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
                playersPresent[playerID-1] = true;
            }
        }
        catch
        {
            Debug.LogWarning("Ignore this, not yet birthday event");
        }
    }

    public void PlayerExitedRoom(int playerID, string roomName)
    {
        try
        {
            if (allRooms[playerChosen - 1] == roomName)
            {
                playersPresent[playerID-1] = false;
            }
        }
        catch
        {
            Debug.LogWarning("Ignore this, not yet birthday event");
        }
    }
}
