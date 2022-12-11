using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboUI : MonoBehaviour
{
    public TMP_Text comboText;
    public float tweenIntensity;
    public float tweenTime;
    public float timerColorChangeRate;

    public ParticleSystem comboParticles;
    public Slider comboTimerSlider;
    public Image comboFillSlider;

    float decentComboFraction;
    float highComboFraction;
    float highestComboFraction;

    public int minimumComboNumber;
    public int decentComboNumber;
    public int highComboNumber;
    public int highestComboNumber;

    public Color sliderStartingColor;
    public Color sliderWarningColor;

    public Color startingColor;
    public Color decentComboColor;
    public Color highComboColor;
    public Color highestComboColor;

    int currentscore = 0;
    bool isTimerVisible = false;

    Color currentSliderColor;
    Color currentTextColor;

    // Start is called before the first frame update
    void Start()
    {
        decentComboFraction = 1 / decentComboNumber;
        Debug.Log(decentComboFraction);
        highComboFraction = 1 / (highComboNumber - decentComboNumber);
        currentTextColor = startingColor;
        ToggleTimerVisibility(false);
    }

    public void ToggleTimerVisibility(bool shouldBeVisible)
    {
        comboTimerSlider.gameObject.SetActive(shouldBeVisible);
        isTimerVisible = shouldBeVisible;

        if (shouldBeVisible)
        {
            UpdateTimerColor(true);
        }
    }

    public void UpdateTimer(float comboTimer, float maxComboTimer)
    {
        if (!isTimerVisible && currentscore >= minimumComboNumber)
        {
            ToggleTimerVisibility(true);
        }

        if (isTimerVisible)
        {
            comboTimerSlider.value = comboTimer / maxComboTimer;
            UpdateTimerColor(false);
        }
    }

    public void UpdateTimerColor(bool isRestarting)
    {
        if (isRestarting)
        {
            currentSliderColor = sliderStartingColor;
        } else
        {
            currentSliderColor = Color.Lerp(currentSliderColor, sliderWarningColor, timerColorChangeRate);
        }

        comboFillSlider.color = currentSliderColor;
    }

    public void UpdateCounter(int currentScore)
    {
        currentscore = currentScore;

        if (currentscore >= minimumComboNumber)
        {
            comboText.text = $"{currentscore} hit combo!";
        } else
        {
            comboText.text = "";
            ToggleTimerVisibility(false);
        }

        if (currentscore > PlayerSaveSystem.SessionSaveData.playerStats.HighestComboCount)
        {
            PlayerSaveSystem.SessionSaveData.playerStats.HighestComboCount = currentscore;
        }

        if (currentscore != 0)
        {
            TweenScore();
            ToggleTimerVisibility(false);
        }

        if (currentscore == 0)
        {
            currentTextColor = startingColor;
            comboText.color = startingColor;
        }

        if (0 < currentscore && currentscore <= decentComboNumber)
        {
            UpdateTextColor(decentComboColor);
        }

        if (currentscore <= highComboNumber && currentscore > decentComboNumber)
        {
            UpdateTextColor(highComboColor);
        }

        if (currentscore <= highestComboNumber && currentscore > highComboNumber)
        {
            UpdateTextColor(highestComboColor);
        }

    }

    void UpdateTextColor(Color newcolor)
    {
        currentTextColor = Color.Lerp(currentTextColor, newcolor, 0.167f);
        comboText.color = currentTextColor;
    }

    void TweenScore()
    {
        LeanTween.scale(comboText.gameObject, Vector3.one * tweenIntensity, tweenTime).setEasePunch();
    }
}
