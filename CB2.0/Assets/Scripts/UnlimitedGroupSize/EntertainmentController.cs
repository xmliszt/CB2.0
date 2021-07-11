using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntertainmentController : MonoBehaviour
{
    public GameObject fromPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveItem()
    {
        gameObject.transform.position = 
        new Vector2(fromPlayer.transform.position.x + 0.6f, fromPlayer.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Entertainments"))
        {
            
            UnlimitedGroupControlHandler unlimitedHandlerScript = 
            fromPlayer.GetComponent<UnlimitedGroupControlHandler>();
            
            unlimitedHandlerScript.held = false;
        }
    }
}
