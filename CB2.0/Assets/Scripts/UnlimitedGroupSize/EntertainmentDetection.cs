using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntertainmentDetection : MonoBehaviour
{

    // TODO: To take player from some sort of manager
    public GameObject player;

    private bool zoneFull = false;

    private EntertainmentController entertainmentController;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Entertainments") && !zoneFull) {
            zoneFull = true;
            Debug.Log("ENTERTAINMENT ENTERS ZONE");

            // Update score
            entertainmentController = other.gameObject.GetComponent<EntertainmentController>();
            entertainmentController.AddScore(player);

        }    
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (entertainmentController == other.gameObject.GetComponent<EntertainmentController>() 
        && zoneFull) {
            zoneFull = false;
            Debug.Log("ENTERTAINMENT LEAVE ZONE");

            // Remove score
            entertainmentController.RemoveScore(player);
        }
    }
}
