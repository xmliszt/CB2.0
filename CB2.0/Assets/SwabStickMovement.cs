using UnityEngine;

public class SwabStickMovement : MonoBehaviour
{
    public GameConstants constants;

    public Vector2Variable playerFacingDirection;

    void Update()
    {
        Vector2 direction = playerFacingDirection.Value;
        // rotate the stick, default facing left
        if (direction == Vector2.up)
        {
            transform.Rotate(0, 0, 90, Space.World);
        }
        else if (direction == Vector2.right)
        {
            transform.Rotate(0, 0, 180, Space.World);
        }
        else if (direction == Vector2.down)
        {
            transform.Rotate(0, 0, -90, Space.World);
        }
        
        transform.Translate(direction * constants.swabStickFlyingSpeed * Time.deltaTime);
    }
}
