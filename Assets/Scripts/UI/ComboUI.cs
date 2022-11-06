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
    public float maxComboTimer;

    public ParticleSystem comboParticles;
    public Slider comboTimerSlider;
    
    float decentComboFraction;
    float highComboFraction;
    float highestComboFraction;

    public int minimumComboNumber;
    public int decentComboNumber;
    public int highComboNumber;
    public int highestComboNumber;

    public Color startingColor;
    public Color decentComboColor;
    public Color highComboColor;
    public Color highestComboColor;

    int currentscore = 0;
    bool isTimerVisible = false;
    Color currentColor;

    // Start is called before the first frame update
    void Start()
    {
        decentComboFraction = 1 / decentComboNumber;
        Debug.Log(decentComboFraction);
        highComboFraction = 1 / (highComboNumber - decentComboNumber);
        currentColor = startingColor;
        ToggleTimerVisibility(false);
    }

    public void ToggleTimerVisibility(bool shouldBeVisible)
    {
        comboTimerSlider.gameObject.SetActive(shouldBeVisible);
        isTimerVisible = shouldBeVisible;
    }

    public void UpdateTimer(float comboTimer)
    {
        if (!isTimerVisible && currentscore >= minimumComboNumber)
        {
            ToggleTimerVisibility(true);
        }

        if (isTimerVisible)
        {
            comboTimerSlider.value = comboTimer / maxComboTimer;
        }
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

        if (currentscore != 0)
        {
            TweenScore();
            ToggleTimerVisibility(false);
        }

        if (currentscore == 0)
        {
            currentColor = startingColor;
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
        currentColor = Color.Lerp(currentColor, newcolor, 0.167f);
        comboText.color = currentColor;
    }

    void TweenScore()
    {
        LeanTween.scale(comboText.gameObject, Vector3.one * tweenIntensity, tweenTime).setEasePunch();
    }
}
