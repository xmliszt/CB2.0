using UnityEngine;

public class ZoneDetection : MonoBehaviour
{
    [Range(0, 16)]
    private int outlineSize = 2;

    private PlayerStats _playerStats;

    private SpriteOutlined zoneOutlined;

    private Color color;

    private bool outline = false;

    public GameConstants constants;

    public SpriteRenderer spriteRenderer;

    void Update()
    {
        if (spriteRenderer)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_Outline", outline ? 1f : 0);

            mpb.SetColor("_OutlineColor", color);
            mpb.SetFloat("_OutlineSize", outlineSize);

            spriteRenderer.SetPropertyBlock(mpb);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Color newColor =
                new Color(spriteRenderer.color.r,
                    spriteRenderer.color.g,
                    spriteRenderer.color.b,
                    constants.playerInZoneOpacity);
            spriteRenderer.color = newColor;
            GameObject player = other.gameObject;
            player
                .GetComponent<PlayerZoneManager>()
                .SetZone(gameObject.tag, gameObject);
            
            if (this.CompareTag("Pickup") || this.CompareTag("Basket"))
            {
                EnableOutline(other.GetComponent<PlayerStatsManager>().GetPlayerStats());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.color =
                new Color(spriteRenderer.color.r,
                    spriteRenderer.color.g,
                    spriteRenderer.color.b,
                    1);
            GameObject player = other.gameObject;
            player
                .GetComponent<PlayerZoneManager>()
                .SetZone("null", gameObject);

            DisableOutline();
        }
    }

    public void EnableOutline(PlayerStats playerStats)
    {
        _playerStats = playerStats;
        color = playerStats.playerAccent;
        outline = true;
    }

    public void DisableOutline()
    {
        outline = false;
    }
}
