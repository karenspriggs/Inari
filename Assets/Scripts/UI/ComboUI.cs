using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboUI : MonoBehaviour
{
    public TMP_Text comboText;

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
        comboText.text = currentScore.ToString();
    }
}
