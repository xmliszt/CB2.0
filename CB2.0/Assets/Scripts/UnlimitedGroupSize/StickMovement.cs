using System.Collections;
using UnityEngine;


// Adapted from SwabStickMovement
public class StickMovement : MonoBehaviour
{
    public GameConstants constants;

    public Vector2 direction;

    public GameObject fromPlayer;

    public void StartFlying()
    {
        StartCoroutine(Fly());
    }

    IEnumerator Fly()
    {
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
        while (true)
        {
            yield return null;
            transform
                .Translate(Vector3.left *
                constants.swabStickFlyingSpeed *
                Time.deltaTime);

            if (IsOutOfBound()) Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy (gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fromPlayer.GetComponent<UnlimitedGroupControlHandler>().OnStickHit();
            other.gameObject.GetComponent<UnlimitedGroupControlHandler>().GetStickHit();
            Destroy (gameObject);
        }

        else if ((other.CompareTag("Wall")) 
        || (other.CompareTag("Decor")) 
        || (other.CompareTag("UGSShop")) 
        || (other.CompareTag("Entertainments"))) {
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
