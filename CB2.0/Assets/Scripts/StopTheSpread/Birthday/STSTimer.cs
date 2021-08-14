using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STSTimer : MonoBehaviour
{
    public Sprite countdown1;
    public Sprite countdown2;
    public Sprite countdown3;
    public Sprite countdown4;
    public Sprite countdown5;
    public Sprite countdown6;
    public Sprite countdown7;
    public Sprite countdown8;

    public SpriteRenderer timerSprite;

    public STSGameConstants stsGameConstants;

    private int countdownVal = 0;

    private Sprite[] allSprites;

    private bool birthdayEventDone = false;

    // Start is called before the first frame update
    void Start()
    {
        timerSprite.enabled = false;
        allSprites = new Sprite[8] { countdown1, countdown2, countdown3, countdown4, countdown5, countdown6, countdown7, countdown8 };
    }

    IEnumerator startCountingDown()
    {
        for (int i = 0; i < 8; i++)
        {
            if (birthdayEventDone)
            {
                timerSprite.enabled = false;
                yield break;
            }
            timerSprite.sprite = allSprites[i];
            yield return new WaitForSeconds(stsGameConstants.birthdayInterval);
        }

        timerSprite.enabled = false;
    }

    public void showTimer()
    {
        if (!birthdayEventDone) 
        { 
            timerSprite.enabled = true;
        }
    }

    public void hideTimer()
    {
        timerSprite.enabled = false;
    }

    public void BirthdayEventActivated()
    {
        timerSprite.enabled = true;
        birthdayEventDone = false;
        StartCoroutine(startCountingDown());
    }

    public void BirthdayEventCompleted()
    {
        birthdayEventDone = true;
    }
}
