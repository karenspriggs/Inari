using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinScreenUI : MonoBehaviour
{
    public GameObject winScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            winScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
