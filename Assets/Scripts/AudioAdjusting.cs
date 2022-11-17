using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioAdjusting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetMaster(float sliderValue)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
    }
    public void SetMusic(float sliderValue)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
    }
    public void SetSFX(float sliderValue)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
    }

}
