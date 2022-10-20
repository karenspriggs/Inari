using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthbar : MonoBehaviour
{
    public static UIHealthbar instance { get; private set; }

    public Image mask;
    float originalSize;
    float maxHPValue;
    float currentValue;

    public float healthShakeRate = 2.0f;

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
        PlayerData.InitializedPlayerHealth += SetMaxHPValue;
        PlayerData.PlayerTookDamage += SetValue;
        PlayerData.PlayerRegainedHP += SetValue;
    }

    private void OnDisable()
    {
        PlayerData.InitializedPlayerHealth -= SetMaxHPValue;
        PlayerData.PlayerTookDamage -= SetValue;
        PlayerData.PlayerRegainedHP -= SetValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetMaxHPValue(float maxHP)
    {
        maxHPValue = maxHP;
        currentValue = maxHP;
    }

    public void SetValue(float damage)
    {
        currentValue -= damage;

        if (currentValue > maxHPValue)
        {
            return;
        }

        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * currentValue / maxHPValue);

        //LeanTween.alphaCanvas(canvasGroup, 0.1f, 0.6f);

        LeanTween.cancel(mask.gameObject);
        
        LeanTween.scaleX(mask.gameObject, currentValue / maxHPValue, 1.0f).setEasePunch();
    }
}
