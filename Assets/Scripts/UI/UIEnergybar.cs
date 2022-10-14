using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnergybar : MonoBehaviour
{
    public static UIEnergybar instance { get; private set; }

    public Image mask;
    float originalSize;
    float maxEnergyValue;
    float currentValue;

    public float energyShakeRate = 2.0f;

    public CanvasGroup canvasGroup;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
        canvasGroup = mask.GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        PlayerData.InitializedPlayerEnergy += SetMaxEnergyValue;
        PlayerData.PlayerUsedEnergy += SetValue;
    }

    private void OnDisable()
    {
        PlayerData.InitializedPlayerEnergy -= SetMaxEnergyValue;
        PlayerData.PlayerUsedEnergy -= SetValue;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetMaxEnergyValue(float maxEnergy)
    {
        maxEnergyValue = maxEnergy;
        currentValue = maxEnergy;
    }

    public void SetValue(float energy)
    {
        Debug.Log("Setting health bar value");
        currentValue -= energy;
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * currentValue / maxEnergyValue);

        LeanTween.cancel(mask.gameObject);

        LeanTween.scaleX(mask.gameObject, currentValue / maxEnergyValue, 1.0f).setEasePunch();
    }
}
