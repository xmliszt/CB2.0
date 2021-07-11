using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // The scriptable objects that will be remembered throughout the game
    public GameStats gameStats;
    
    // Start is called before the first frame update
    void Start()
    {
        gameStats.SetCurrentScene(GameStats.Scene.unlimitedGroupSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
