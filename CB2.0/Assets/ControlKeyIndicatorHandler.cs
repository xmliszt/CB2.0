using System.Collections.Generic;
using UnityEngine;

public class ControlKeyIndicatorHandler : MonoBehaviour
{
    public List<ControllerKey> controllerKeys;

    public SpriteRenderer xboxSpriteRenderer;
    public SpriteRenderer psSpriteRenderer;

    private void Start()
    {
        xboxSpriteRenderer.enabled = false;
        psSpriteRenderer.enabled = false;
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
    
        xboxSpriteRenderer.sprite = GetSpriteFromKey(ControllerType.xbox, keyType);
        psSpriteRenderer.sprite = GetSpriteFromKey(ControllerType.playStation, keyType);
        xboxSpriteRenderer.enabled = true;
        psSpriteRenderer.enabled = true;
    }

    public void TurnOffIndiciator()
    {
        xboxSpriteRenderer.enabled = false;
        psSpriteRenderer.enabled = false;
    }
}
