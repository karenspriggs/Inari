using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    [SerializeField]
    float comboTimer;
    [SerializeField]
    float defaultComboResetTime;
    [SerializeField]
    float currentComboResetTime;
    [SerializeField]
    float minimumComboResetTime;
    [SerializeField]
    float comboTimeDecreaseFactor;
    [SerializeField]
    ComboUI comboUI;

    int currentComboCount = 0;
    int maxComboCount = 0;

    private void OnEnable()
    {
        EnemyData.EnemyHit += UpdateCombo;
    }

    private void OnDisable()
    {
        EnemyData.EnemyHit -= UpdateCombo;
    }

    void UpdateCombo()
    {
        currentComboCount++;
        comboTimer = currentComboResetTime;
        comboUI.UpdateCounter(currentComboCount);

        LowerComboTime();

        if (currentComboCount > maxComboCount)
        {
            maxComboCount = currentComboCount;
        }
    }

    void LowerComboTime()
    {
        if (currentComboResetTime - comboTimeDecreaseFactor > minimumComboResetTime)
        {
            currentComboResetTime -= comboTimeDecreaseFactor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateComboTimer();

        if (comboTimer < defaultComboResetTime)
        {
            comboUI.UpdateTimer(comboTimer, currentComboResetTime);
        }
    }

    void UpdateComboTimer()
    {
        if (comboTimer > 0)
        {
            comboTimer -= 1 * Time.deltaTime;

            if (comboTimer <= 0)
            {
                currentComboCount = 0;
                currentComboResetTime = defaultComboResetTime;
                comboUI.UpdateCounter(currentComboCount);
                comboUI.ToggleTimerVisibility(false);
            }
        }
    }
}
