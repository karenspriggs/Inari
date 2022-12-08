using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;

public class KillCount : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public int counter;

    void Start()
    {
        GameObject.Find("kill count").GetComponent<TextMeshProUGUI>().SetText(counter.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        //ShowKills();
    }

   // private void ShowKills()
   // {
       // counterText.text = counter.ToString();
   // }

    public void AddKills()
    {
        counter++;
    }
}
