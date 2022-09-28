using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private bool slowmoOn = false;
    public float slowmoTimeScale = 0.5f;

    public void SetTimeScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
    }

    public void ResetTimeScale()
    {
        SetTimeScale(1f);
    }

    public void ToggleSlowmo()
    {
        Debug.Log("Toggle slowmo");
        if (slowmoOn)
        {
            ResetTimeScale();
        }
        else
        {
            SetTimeScale(slowmoTimeScale);
        }
        slowmoOn = !slowmoOn;
    }

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        
    }
}
