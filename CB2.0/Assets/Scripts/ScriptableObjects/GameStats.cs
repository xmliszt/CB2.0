using UnityEngine;

[
    CreateAssetMenu(
        fileName = "GameStats",
        menuName = "ScriptableObjects/GameStats",
        order = 9)
]
public class GameStats : ScriptableObject
{
    public enum Scene {
        gameLobby = 0,
        
        snatchAndHoard = 1,
        swabTestWar = 2,
        unlimitedGroupSize = 3,
        stopTheSpread = 4,
    }

    private Scene currentScene;

    public void SetCurrentScene(Scene _scene)
    {
        currentScene = _scene;
    }

    public Scene GetCurrentScene()
    {
        return currentScene;
    }
}
