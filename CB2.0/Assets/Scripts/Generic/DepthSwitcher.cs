using UnityEngine;

public class DepthSwitcher : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sortingLayerName = "Foreground";
            spriteRenderer.sortingOrder = 0;
        }       
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sortingLayerName = "Background";
            spriteRenderer.sortingOrder = 5;
        }  
    }
}
