using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnHGameManager : MonoBehaviour
{
    public SnHGameConstants gameConstants;
    public GameStats gameStats;
    public SnHPlayerStats P1Stats, P2Stats, P3Stats, P4Stats;
    public SnHPlayerControlHandler P1CH, P2CH, P3CH, P4CH;
    public SnHPlayerStatsController P1StatsController, P2StatsController, P3StatsController, P4StatsController;
    public SnHBasketController P1BC, P2BC, P3BC, P4BC;
    public SpawnedPickupManager spawnManager;
    public ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        // state this scene
        gameStats.SetCurrentScene(GameStats.Scene.snatchAndHoard);

        // select random item for game constant
        int otheritem = Random.Range(0, 3); // game constant have to follow pickup index
        gameConstants.OtherIndex = otheritem + 2; 

        // reset all player stats
        P1Stats.coinsCollected = 0;
        P1Stats.TPCollected = 0;
        P1Stats.otherObjectCollected = 0;
        P1Stats.zoneType = SnHPlayerStats.ZoneType.NotInAnyZone;
        P2Stats.coinsCollected = 0;
        P2Stats.TPCollected = 0;
        P2Stats.otherObjectCollected = 0;
        P2Stats.zoneType = SnHPlayerStats.ZoneType.NotInAnyZone;
        P3Stats.coinsCollected = 0;
        P3Stats.TPCollected = 0;
        P3Stats.otherObjectCollected = 0;
        P3Stats.zoneType = SnHPlayerStats.ZoneType.NotInAnyZone;
        P4Stats.coinsCollected = 0;
        P4Stats.TPCollected = 0;
        P4Stats.otherObjectCollected = 0;
        P4Stats.zoneType = SnHPlayerStats.ZoneType.NotInAnyZone;


        // assign playeractive to playerstats
        P1Stats.isActive = gameConstants.P1Playing;
        P2Stats.isActive = gameConstants.P2Playing;
        P3Stats.isActive = gameConstants.P3Playing;
        P4Stats.isActive = gameConstants.P4Playing;

        // assign playerstats to playercontrolhandler
        P1CH.snhPlayerStats = P1Stats;
        P2CH.snhPlayerStats = P2Stats;
        P3CH.snhPlayerStats = P3Stats;
        P4CH.snhPlayerStats = P4Stats;

        // assign playerstats to playerstatscontroller
        P1StatsController.uniquePlayerStats = P1Stats;
        P2StatsController.uniquePlayerStats = P2Stats;
        P3StatsController.uniquePlayerStats = P3Stats;
        P4StatsController.uniquePlayerStats = P4Stats;

        // assign playerstats to basketcontroller
        P1BC.snhPlayerStats = P1Stats;
        P2BC.snhPlayerStats = P2Stats;
        P3BC.snhPlayerStats = P3Stats;
        P4BC.snhPlayerStats = P4Stats;

        // spawn objects
        spawnManager.onStart();

        // initialise the scores
        scoreManager.onStart();

        // initialise the baskets
        P1BC.onStart();
        P2BC.onStart();
        P3BC.onStart();
        P4BC.onStart();

        // initialise the players
        P1CH.onStart();
        P2CH.onStart();
        P3CH.onStart();
        P4CH.onStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
