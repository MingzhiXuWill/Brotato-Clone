using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider Slider;
    public Color HealthBarLow;
    public Color HealthBarHigh;
    public Color BackgroundLow;
    public Color BackgroundHigh;

    public Vector3 Offset;

    void Update()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
    }
}
