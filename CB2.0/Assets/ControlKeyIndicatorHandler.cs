using System.Collections.Generic;
using UnityEngine;

public class ControlKeyIndicatorHandler : MonoBehaviour
{
    public List<ControllerKey> controllerKeys;

    public SpriteRenderer genericSpriteRenderer;
    // public SpriteRenderer xboxSpriteRenderer;
    // public SpriteRenderer psSpriteRenderer;

    private void Start()
    {
        // xboxSpriteRenderer.enabled = false;
        // psSpriteRenderer.enabled = false;
        genericSpriteRenderer.enabled = false;
    }

    private Sprite GetSpriteFromKey(
        ControllerType controllerType,
        ControllerKeyType controllerKeyType
    )
    {
        foreach (ControllerKey keyItem in controllerKeys)
        {
            if (
                keyItem.controllerType == controllerType &&
                keyItem.keyType == controllerKeyType
            )
            {
                return keyItem.keySprite;
            }
        }

        return null;
    }

    public void TurnOnIndicator(ControllerKeyType keyType)
    {
    
        genericSpriteRenderer.sprite = GetSpriteFromKey(ControllerType.generic, keyType);
        genericSpriteRenderer.enabled = true;
    }

    public void TurnOffIndiciator()
    {
        genericSpriteRenderer.enabled = false;
    }
}
