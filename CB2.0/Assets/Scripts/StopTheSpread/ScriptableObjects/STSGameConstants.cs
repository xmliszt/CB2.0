using UnityEngine;

[
    CreateAssetMenu(
        fileName = "STSGameConstants",
        menuName = "ScriptableObjects/STS/STSGameConstants",
        order = 2)
]

public class STSGameConstants : ScriptableObject
{
    public float activityDelay = 5.0f;

    public int maxSliderVal = 3;
    public float doingActivityInterval = 0.25f;
    public int intervals = 12; // intervals = maxSliderVal / doingActivityInterval

    public int activityCooldownTime = 5;

    public int customerHungryTime = 5;

    public float AISpeed = 400.0f;

    public float AIFastSpeed = 800.0f;

    public float birthdayInterval = 1f;

    public float waitForBirthday = 35f;

    public float birthdaySongDuration = 3.0f;

    public float AIChaseDuration = 3.0f;

    public float invisibilityTime = 10.0f;
}
