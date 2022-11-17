using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayerExperienceBar : MonoBehaviour
{
    public Slider expBar;
    public TMP_Text levelUpText;
    public float LevelUpTextDuration;
    public float TextFadeDuration;

    float barMax;
    float currentValue;

    private void OnEnable()
    {
        PlayerLevelSystem.xpAmountInitialized += SetMaxBarXP;
        PlayerLevelSystem.PlayerGainedXP += UpdateBarXP;
        PlayerLevelSystem.PlayerLeveledUp += ShowLevelUpText;
    }

    private void OnDisable()
    {
        PlayerLevelSystem.xpAmountInitialized -= SetMaxBarXP;
        PlayerLevelSystem.PlayerGainedXP -= UpdateBarXP;
        PlayerLevelSystem.PlayerLeveledUp -= ShowLevelUpText;
    }

    void SetMaxBarXP(float currentXP, float maxXP)
    {
        currentValue = currentXP;
        barMax = maxXP;
        UpdateBarXP(currentValue);
    }

    void UpdateBarXP(float currentXP)
    {
        expBar.value = currentXP / barMax;
    }

    void ShowLevelUpText()
    {
        levelUpText.color = new Color(levelUpText.color.r, levelUpText.color.g, levelUpText.color.b, 255);
        StartCoroutine(FadeInText(levelUpText));
    }

    // Taken from : https://forum.unity.com/threads/real-fade-of-text-mesh-pro.620833/
    IEnumerator FadeInText(TMP_Text fadeText)
    {
        yield return new WaitForSeconds(LevelUpTextDuration);

        float currentTime = 0f;
        while (currentTime < TextFadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / TextFadeDuration);
            fadeText.color = new Color(fadeText.color.r, fadeText.color.g, fadeText.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
