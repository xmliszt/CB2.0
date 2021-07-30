using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RechargeBar : MonoBehaviour
{
    public Slider rechargeBar;

    public GameConstants constants;

    private int currentRecharge;

    private WaitForSeconds rechargeTick = new WaitForSeconds(0.1f);

    // Start is called before the first frame update
    void Start()
    {
        currentRecharge = constants.maxRecharge;
        rechargeBar.maxValue = constants.maxRecharge;
        rechargeBar.value = constants.maxRecharge;
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
        while(currentRecharge < constants.maxRecharge)
        {
            currentRecharge += constants.rechargePerTick;
            rechargeBar.value = currentRecharge;
            yield return rechargeTick;
        }
    }
}
