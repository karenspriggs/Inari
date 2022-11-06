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
    
    float decentComboFraction;
    float highComboFraction;
    float highestComboFraction;

    public int decentComboNumber;
    public int highComboNumber;
    public int highestComboNumber;

    public Color startingColor;
    public Color decentComboColor;
    public Color highComboColor;
    public Color highestComboColor;

    Color currentColor;

    // Start is called before the first frame update
    void Start()
    {
        decentComboFraction = 1 / decentComboNumber;
        Debug.Log(decentComboFraction);
        highComboFraction = 1 / (highComboNumber - decentComboNumber);
        currentColor = startingColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCounter(int currentScore)
    {
        comboText.text = $"{currentScore} hit combo!";

        if (currentScore != 0)
        {
            TweenScore();
        }

        if (currentScore == 0)
        {
            currentColor = startingColor;
            comboText.color = startingColor;
        }

        if (0 < currentScore && currentScore <= decentComboNumber)
        {
            UpdateTextColor(decentComboColor);
        }

        if (currentScore <= highComboNumber && currentScore > decentComboNumber)
        {
            UpdateTextColor(highComboColor);
        }

        if (currentScore <= highestComboNumber && currentScore > highComboNumber)
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
