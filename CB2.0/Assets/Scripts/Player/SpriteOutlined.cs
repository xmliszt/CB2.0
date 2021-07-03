using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutlined : MonoBehaviour {

    private Color color;
    [Range(0, 16)]
    public int outlineSize = 1;

    private SpriteRenderer spriteRenderer;

    private PlayerStats playerStats;

    public void EnableOutline(PlayerStats playerStats)
    {
        color = playerStats.playerAccent;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateOutline(true);
    }

    public void DisableOutline() {
        UpdateOutline(false);
    }

    void Update() {
        UpdateOutline(true);
    }

    void UpdateOutline(bool outline) {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}