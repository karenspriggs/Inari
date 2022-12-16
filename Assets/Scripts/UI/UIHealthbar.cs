using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHealthbar : MonoBehaviour
{
    public static UIHealthbar instance { get; private set; }

    public Image mask;
    public TMP_Text healthText;
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

    private void Init(float health)
    {

        SetMaxHPValue(health);
        SetValue(health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaxHPValue(float maxHP)
    {
        maxHPValue = maxHP;
        currentValue = maxHP;
        SetHPText();
    }

    void SetHPText()
    {
        healthText.text = $"{currentValue}/{maxHPValue}";
    }

    public void SetValue(float damage)
    {
        Debug.Log("Updating Value");

        currentValue -= damage;

        if (currentValue > maxHPValue)
        {
            return;
        }

        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * currentValue / maxHPValue);

        //LeanTween.alphaCanvas(canvasGroup, 0.1f, 0.6f);

        LeanTween.cancel(mask.gameObject);
        
        LeanTween.scaleX(mask.gameObject, currentValue / maxHPValue, 1.0f).setEasePunch();
        SetHPText();
    }
}
