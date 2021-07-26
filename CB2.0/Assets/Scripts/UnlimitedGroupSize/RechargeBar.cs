using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RechargeBar : MonoBehaviour
{
    public Slider rechargeBar;

    private int maxRecharge = 100; // Use game constants?
    private int currentRecharge;

    private WaitForSeconds rechargeTick = new WaitForSeconds(0.1f);

    // Start is called before the first frame update
    void Start()
    {
        currentRecharge = maxRecharge;
        rechargeBar.maxValue = maxRecharge;
        rechargeBar.value = maxRecharge;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseRecharge(int amount)
    {
        if(currentRecharge - amount >= 0)
        {
            currentRecharge -= amount;
            rechargeBar.value = currentRecharge;

            StartCoroutine(recharge());
        }
    }

    public int GetRecharge()
    {
        return currentRecharge;
    }

    private IEnumerator recharge()
    {
        // yield return new WaitForSeconds(0.1f);

        while(currentRecharge < maxRecharge)
        {
            currentRecharge += maxRecharge / 50;
            rechargeBar.value = currentRecharge;
            yield return rechargeTick;
        }
    }
}
