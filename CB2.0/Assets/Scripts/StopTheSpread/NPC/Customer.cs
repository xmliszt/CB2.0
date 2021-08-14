using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public SpriteRenderer thoughtBubbleRenderer;

    public STSGameConstants stsGameConstants;

    private bool isSatisfied = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator receivedFood()
    {
        yield return new WaitForSeconds(stsGameConstants.customerHungryTime);
        isSatisfied = false;
        thoughtBubbleRenderer.enabled = true;
    }

    public void PlayerGaveFood()
    {
        if (!isSatisfied)
        {
            isSatisfied = true;
            thoughtBubbleRenderer.enabled = false;

            StartCoroutine(receivedFood());
        }
    }

    public bool isCustomerFull()
    {
        return isSatisfied;
    }
}
