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
    float shakeTimeElasped = 0f ;

    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        noisePerlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        //CharacterEvents.CharacterDamaged += ShakeIfTarget;
        //Character action whatever that may be.^
        //"CharacterEvents" = Class, "CharacterDamage" = Method... just incase people were curious.
    }

    //void OnDistroy(){ CharacterEvents.CharacterDamaged -= ShakeIfTarget; }
    //this is to remove it in the list of methods to be reinvoked (basically to stop it from looping))
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
        if(shakeTimeElasped > shakeTime) { StopShake(); }
    }
}
