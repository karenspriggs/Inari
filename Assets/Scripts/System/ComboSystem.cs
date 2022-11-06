using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    [SerializeField]
    float comboTimer;
    [SerializeField]
    float comboResetTime;
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
        comboTimer = comboResetTime;
        comboUI.UpdateCounter(currentComboCount);

        if (currentComboCount > maxComboCount)
        {
            maxComboCount = currentComboCount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateComboTimer();

        if (comboTimer < comboResetTime)
        {
            comboUI.UpdateTimer(comboTimer);
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
                comboUI.UpdateCounter(currentComboCount);
                comboUI.ToggleTimerVisibility(false);
            }
        }
    }
}
