using UnityEngine;

public class FloatingAnimator : MonoBehaviour
{
    public GameConstants constants;
 
    // Position Storage Variables
    Vector3 posOffset = new Vector3 ();
    Vector3 tempPos = new Vector3 ();
 
    // Use this for initialization
    void Start () {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
    }
     
    // Update is called once per frame
    void Update () {
 
        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * constants.floatingFrequency) * constants.floatingAmplitude;
 
        transform.position = tempPos;
    }
}
