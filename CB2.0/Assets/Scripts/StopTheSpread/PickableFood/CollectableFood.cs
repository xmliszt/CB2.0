using UnityEngine;

public class CollectableFood : MonoBehaviour
{
    public STSFood foodMeta;

    public SpriteRenderer spriteRenderer;

    public void SetFood(STSFood _stsFood)
    {
        foodMeta = _stsFood;
        spriteRenderer.sprite = _stsFood.itemSprite;
    }
}
