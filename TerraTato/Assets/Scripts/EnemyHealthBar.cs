using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider Slider;

    public Color HealthBarLow;
    public Color HealthBarHigh;

    public Vector3 Offset;

    void Update()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
    }

    public void SetHealth(float Health, float MaxHealth) 
    {
        this.gameObject.SetActive(Health < MaxHealth);
        //Slider.gameObject.SetActive(Health < MaxHealth);
        Slider.value = Health;
        Slider.maxValue = MaxHealth;

        Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(HealthBarLow, HealthBarHigh, Slider.normalizedValue);
    }
}
