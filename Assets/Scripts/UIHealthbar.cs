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

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    private void OnEnable()
    {
        PlayerData.InitializedPlayerHealth += SetMaxHPValue;
        PlayerData.PlayerTookDamage += SetValue;
    }

    private void OnDisable()
    {
        PlayerData.InitializedPlayerHealth -= SetMaxHPValue;
        PlayerData.PlayerTookDamage -= SetValue;
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
        Debug.Log("Setting health bar value");
        currentValue -= damage;
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * currentValue/maxHPValue);
    }
}
