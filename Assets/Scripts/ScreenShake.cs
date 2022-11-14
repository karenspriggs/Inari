using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin noisePerlin;
    public float ampletudeGain = 2f, frequencyGain = 2f, shakeTime = 1f;
    bool isShaking = false;
    float shakeTimeElasped;

    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        noisePerlin = vcam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
    }

    private void OnEnable()
    {
        EnemyData.EnemyKilled += StartShaking;
    }

    private void OnDisable()
    {
        EnemyData.EnemyKilled -= StartShaking;
    }

    public void ShakeIfTarget(float damage, GameObject gameObject) //only placeholders for now
    {
        if(vcam.Follow == gameObject.transform)
        {
            StartShaking();
        }
    }

    public void StartShaking()
    {
        noisePerlin.m_AmplitudeGain = ampletudeGain;
        noisePerlin.m_FrequencyGain = frequencyGain;
        isShaking = true;
        shakeTimeElasped = 0f;
    }

    public void StopShake()
    {
        noisePerlin.m_AmplitudeGain = 0f;
        noisePerlin.m_FrequencyGain = 0f;
        isShaking = false;
        shakeTimeElasped = 0f;
    }

    private void Update()
    {
        shakeTimeElasped += Time.deltaTime;
        if(shakeTimeElasped > shakeTime && isShaking) { StopShake(); }
    }
}
