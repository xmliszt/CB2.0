using System.Collections;
using UnityEngine;

public class SwabStickMovement : MonoBehaviour
{
    public GameConstants constants;

    public SingleIntegerGameEvent swabStickHitGameEvent;

    public Vector2Variable playerFacingDirection;

    private Vector2 direction;

    private void Start()
    {
        direction = playerFacingDirection.Value;

        // rotate the stick, default facing left
        if (direction == Vector2.up)
        {
            transform.Rotate(0, 0, -90, Space.World);
        }
        else if (direction == Vector2.right)
        {
            transform.Rotate(0, 0, 180, Space.World);
        }
        else if (direction == Vector2.down)
        {
            transform.Rotate(0, 0, 90, Space.World);
        }
    }

    void Update()
    {
        transform
            .Translate(Vector3.left *
            constants.swabStickFlyingSpeed *
            Time.deltaTime);

        if (IsOutOfBound()) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.CompareTag("Wall")) {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            swabStickHitGameEvent.Fire(transform.GetInstanceID());
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        if (other.CompareTag("Wall")) {
            Destroy(gameObject);
        }
    }
    private bool IsOutOfBound()
    {
        return (
        Mathf.Abs(transform.position.x) > constants.gameBoundX ||
        Mathf.Abs(transform.position.y) > constants.gameBoundY
        );
    }
}
