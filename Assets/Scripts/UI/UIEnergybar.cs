using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEnergybar : MonoBehaviour
{
    public static UIEnergybar instance { get; private set; }

    public Image mask;
    public TMP_Text energyText;

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
        PlayerData.InitializedPlayerEnergy += Init;
        PlayerData.PlayerUsedEnergy += SetValue;
    }

    private void OnDisable()
    {
        PlayerData.InitializedPlayerEnergy -= Init;
        PlayerData.PlayerUsedEnergy -= SetValue;
    }

    private void Init(float energy)
    {
        SetMaxEnergyValue(energy);
        SetValue(energy);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetEnergyText()
    {
        energyText.text = $"{currentValue}/{maxEnergyValue}";
    }

    public void SetMaxEnergyValue(float maxEnergy)
    {
        maxEnergyValue = maxEnergy;
        SetEnergyText();
    }

    public void SetValue(float energy)
    {
        currentValue = energy;
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * currentValue / maxEnergyValue);

        LeanTween.cancel(mask.gameObject);

        LeanTween.scaleX(mask.gameObject, currentValue / maxEnergyValue, 1.0f).setEasePunch();
        SetEnergyText();
    }
}
