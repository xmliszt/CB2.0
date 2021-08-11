using UnityEngine;
using UnityEngine.UI;

public class ZoneColour : MonoBehaviour
{
    public int playerID;

    public Players players;

    private Color32 playerColor;

    private RawImage rawImage;

    void Start()
    {
        if (players.GetPlayers().ContainsKey(playerID))
        {
            rawImage = GetComponent<RawImage>();
            playerColor =
                players.GetPlayers()[playerID].playerStats.playerAccent;
            rawImage.color =
                new Color32(playerColor.r, playerColor.g, playerColor.b, 50);
        }
    }
}
