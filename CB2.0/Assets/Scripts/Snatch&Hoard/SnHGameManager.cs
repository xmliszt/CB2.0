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

        // assign spawnmanager to playercontrolhandler
        P1CH.spawnManager = spawnManager;
        P2CH.spawnManager = spawnManager;
        P3CH.spawnManager = spawnManager;
        P4CH.spawnManager = spawnManager;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
