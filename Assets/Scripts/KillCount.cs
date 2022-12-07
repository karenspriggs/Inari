using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;

public class KillCount : MonoBehaviour
{
    public Text counterText;
    int counter;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShowKills();
    }

    private void ShowKills()
    {
        counterText.text = counter.ToString();
    }

    public void AddKills()
    {
        counter++;
    }
}
