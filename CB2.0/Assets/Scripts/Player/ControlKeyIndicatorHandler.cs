using System.Collections.Generic;
using UnityEngine;

public class ControlKeyIndicatorHandler : MonoBehaviour
{
    public GameStats gameStats;

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

    private Sprite
    GetSpriteFromKey(
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
        if (gameStats.controllerType == ControllerType.generic)
        {
            genericSpriteRenderer.sprite =
                GetSpriteFromKey(ControllerType.generic, keyType);
        }
        else if (gameStats.controllerType == ControllerType.keyboard)
        {
            genericSpriteRenderer.sprite =
                GetSpriteFromKey(ControllerType.keyboard, keyType);
        }
        genericSpriteRenderer.enabled = true;
    }

    public void TurnOffIndiciator()
    {
        genericSpriteRenderer.enabled = false;
    }
}
