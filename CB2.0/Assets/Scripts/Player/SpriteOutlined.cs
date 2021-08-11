using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutlined : MonoBehaviour
{
    [Range(0, 16)]
    public int outlineSize = 1;

    private SpriteRenderer spriteRenderer;

    private PlayerStats playerStats;

    private Color color;

    private bool isRainbowEffect = false;

    private bool outline;

    private void Start()
    {
        outline = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void EnableOutline(PlayerStats playerStats)
    {
        color = playerStats.playerAccent;
        outline = true;
    }

    public void DisableOutline()
    {
        outline = false;
    }

    public void TurnOnRainbowEffect()
    {
        isRainbowEffect = true;
    }

    public void TurnOffRainbowEffect()
    {
        isRainbowEffect = false;
    }

    void Update()
    {
        if (spriteRenderer)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock (mpb);
            mpb.SetFloat("_Outline", outline ? 1f : 0);
            if (isRainbowEffect)
            {
                float h, s, v;
                Color.RGBToHSV(mpb.GetColor("_OutlineColor"), out h, out s, out v);
                mpb.SetColor("_OutlineColor", Color.HSVToRGB(h + Time.deltaTime * .25f, s, v));
            }
            else
            {
                mpb.SetColor("_OutlineColor", color);
            }
            mpb.SetFloat("_OutlineSize", outlineSize);
            spriteRenderer.SetPropertyBlock (mpb);
        }
    }
}
