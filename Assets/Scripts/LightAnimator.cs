using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAnimator : MonoBehaviour
{
    private Animator lightAnimator;

    private int animatorFadeInBool;
    private int animatorStartOnBool;

    // Start is called before the first frame update
    void Awake()
    {
        lightAnimator = GetComponent<Animator>();

        animatorFadeInBool = Animator.StringToHash("StartFadeIn");
        animatorStartOnBool = Animator.StringToHash("StartLightOn");
    }

    public void StartFadeAnimation()
    {
        lightAnimator.SetBool(animatorFadeInBool, true);
    }

    public void StartLightOnAnimation()
    {
        lightAnimator.SetBool(animatorStartOnBool, true);
    }
}
