using UnityEngine;

public class ZoneDetection : MonoBehaviour
{
    public ZoneChangeGameEvent zoneChangeGameEvent;

    public GameConstants constants;
    public SpriteRenderer spriteRenderer;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, constants.playerInZoneOpacity);
            spriteRenderer.color = newColor;
            GameObject player = other.gameObject;

            zoneChangeGameEvent.Fire(player.GetComponent<PlayerStatsManager>().GetPlayerStats().playerID, gameObject.tag, gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
            GameObject player = other.gameObject;
            
            zoneChangeGameEvent.Fire(player.GetComponent<PlayerStatsManager>().GetPlayerStats().playerID,"null", gameObject);
        }
    }
}
