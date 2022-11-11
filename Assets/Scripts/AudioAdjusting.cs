using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioAdjusting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetVolume(float sliderValue)
    {
        audioMixer.SetFloat("Master(music)", Mathf.Log10(sliderValue) * 20);
    }

}
