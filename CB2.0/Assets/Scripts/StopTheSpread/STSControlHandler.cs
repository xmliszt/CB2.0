using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class STSControlHandler : MonoBehaviour
{
    public STSGameConstants stsGameConstants;

    [SerializeField] private Slider completionBar;

    private ZoneType zoneType = ZoneType.nullType;

    [Header("Activity Types")]
    public STSActivity gym;
    public STSActivity computer;
    public STSActivity karaoke;
    public STSActivity toiletPaper;

    private STSActivity[] allActivities;

    [Header("Player Attributes")]
    public SpriteRenderer thoughtBubbleRenderer;

    private PlayerControllerSTS playerControllerSTS;

    private int desiredActivity;

    private float delay;

    private bool playerDoingActivity = false;

    private enum ZoneType
    {
        dumbbell = 0,
        computer = 1,
        karaoke = 2,
        toiletPaper = 3,
        grocer = 4,
        NPCCustomer = 5,
        nullType = 6
    }


    private void Awake()
    {
        playerControllerSTS = GetComponent<PlayerControllerSTS>();
        delay = stsGameConstants.activityDelay;

        allActivities = new STSActivity[4] { gym, computer, karaoke, toiletPaper };
    }

    // Start is called before the first frame update
    void Start()
    {
        desiredActivity = UnityEngine.Random.Range(0, 4);
        thoughtBubbleRenderer.sprite = allActivities[desiredActivity].thoughtBubbleSprite;

        completionBar.value = 0;
        completionBar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void generateActivity()
    {
        desiredActivity = UnityEngine.Random.Range(0, 4);
        thoughtBubbleRenderer.sprite = allActivities[desiredActivity].thoughtBubbleSprite;
    }

    public void OnUse()
    {
        // should be using Enum.IsDefined, but then cause cannot check Enum of different type so using this inefficient method as of now
        if (zoneType == ZoneType.dumbbell || zoneType == ZoneType.computer || zoneType == ZoneType.karaoke || zoneType == ZoneType.toiletPaper || zoneType == ZoneType.grocer || zoneType == ZoneType.NPCCustomer)
        {
            if (!playerDoingActivity)
            {
                switch (zoneType)
                {
                    case ZoneType.dumbbell:
                        if (desiredActivity == 0)
                        {
                            StartCoroutine(UsingInteractable());
                        }
                        break;
                    case ZoneType.computer:
                        if (desiredActivity == 1)
                        {
                            StartCoroutine(UsingInteractable());
                        }
                        break;
                    case ZoneType.karaoke:
                        if (desiredActivity == 2)
                        {
                            StartCoroutine(UsingInteractable());
                        }
                        break;
                    case ZoneType.toiletPaper:
                        if (desiredActivity == 3)
                        {
                            StartCoroutine(UsingInteractable());
                        }
                        break;
                }
            }
        }
    }

    IEnumerator UsingInteractable()
    {
        completionBar.gameObject.SetActive(true);
        playerDoingActivity = true;


        // wait for 3 seconds, check every 0.5s if the person is out of the box
        for (int i = 0; i < stsGameConstants.intervals; i++)
        {
            completionBar.value += stsGameConstants.doingActivityInterval;
            yield return new WaitForSeconds(stsGameConstants.doingActivityInterval);
            if (zoneType == ZoneType.nullType)
            {
                Debug.Log("Player stopped halfway");
                playerDoingActivity = false;

                completionBar.value = 0;
                completionBar.gameObject.SetActive(false);

                yield break;
            }
        }

        completionBar.value = 0;
        completionBar.gameObject.SetActive(false);

        //thoughtBubbleRenderer.enabled = false;
        playerDoingActivity = false;
        
        generateActivity();

        Debug.Log("Player completed activity");

    }

    public void PlayerOnTriggerEnterInteractable(String otherTag)
    {
        switch (otherTag)
        {
            case "Dumbbell":
                zoneType = ZoneType.dumbbell;
                break;
            case "Computer":
                zoneType = ZoneType.computer;
                break;
            case "Karaoke":
                zoneType = ZoneType.karaoke;
                break;
            case "ToiletPaper":
                zoneType = ZoneType.toiletPaper;
                break;
            case "Grocer":
                zoneType = ZoneType.grocer;
                break;
            case "NPCCustomer":
                zoneType = ZoneType.NPCCustomer;
                break;
        }
    }

    public void PlayerOnTriggerExitInteractable()
    {
        zoneType = ZoneType.nullType;
    }
}
