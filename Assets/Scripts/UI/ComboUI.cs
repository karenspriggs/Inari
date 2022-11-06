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

    public Color startingColor;
    public Color decentComboColor;
    public Color highestComboColor;

    // Start is called before the first frame update
    void Start()
    {
        
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
            comboText.color = startingColor;
        }

        if (currentScore >= 6)
        {
            comboText.color = decentComboColor;
        }

        if (currentScore >= 12)
        {
            comboText.color = highestComboColor;
        }
    }

    void TweenScore()
    {
        LeanTween.scale(comboText.gameObject, Vector3.one * tweenIntensity, tweenTime).setEasePunch();
    }
}
