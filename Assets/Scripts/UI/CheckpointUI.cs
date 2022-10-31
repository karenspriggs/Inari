using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckpointUI : MonoBehaviour
{
    public TMP_Text CheckpointText;
    public TMP_Text RespawnText;

    public float CheckpointTextDuration;
    public float TextFadeDuration;

    private void OnEnable()
    {
        CheckpointManager.PlayerRespawnedAtCheckpoint += EnableRespawnText;
        Checkpoint.PlayerPassedCheckpoint += EnableCheckpointText;
    }

    private void OnDisable()
    {
        CheckpointManager.PlayerRespawnedAtCheckpoint -= EnableRespawnText;
        Checkpoint.PlayerPassedCheckpoint -= EnableCheckpointText;
    }

    void EnableRespawnText()
    {
        RespawnText.color = new Color(CheckpointText.color.r, CheckpointText.color.g, CheckpointText.color.b, 255);
        StartCoroutine(FadeInText(RespawnText));
    }

    void EnableCheckpointText()
    {
        CheckpointText.color = new Color(CheckpointText.color.r, CheckpointText.color.g, CheckpointText.color.b, 255);
        StartCoroutine(FadeInText(CheckpointText));
    }

    // Taken from : https://forum.unity.com/threads/real-fade-of-text-mesh-pro.620833/
    IEnumerator FadeInText(TMP_Text fadeText)
    {
        yield return new WaitForSeconds(CheckpointTextDuration);

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
