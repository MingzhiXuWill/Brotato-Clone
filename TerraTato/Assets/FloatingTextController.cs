using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextController : MonoBehaviour
{
    TextMeshProUGUI FloatingText;
    public Color StartColor;
    public Color EndColor;
    public float ColorChangingSpeed;

    float StartingTime;

    void Start()
    {
        StartingTime = Time.time;
        FloatingText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float val = Mathf.PingPong(Time.time - StartingTime * ColorChangingSpeed, 1.0f);

        FloatingText.color = Color.Lerp(StartColor, EndColor, val);

        if (FloatingText.fontSize == 0) {
            Destroy(gameObject.transform.parent.gameObject);
        } 
    }
}
