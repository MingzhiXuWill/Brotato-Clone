using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextManager : MonoBehaviour
{
    // Floating Text
    public GameObject FloatingTextEnemy;
    public GameObject FloatingTextPlayer;
    public GameObject FloatingTextHeal;
    public GameObject Canvas;

    public static FloatingTextManager ftman;
    void Start()
    {
        ftman = this;
    }

    public void CreateText(GameObject FloatingTextMark, int Damage, int TextType)
    {
        float RandomFloatX = 0.5f;
        float RandomFloatY = 1f;

        Vector3 RandomFloatingTextPosition = FloatingTextMark.transform.position + new Vector3(Random.Range(-RandomFloatX, RandomFloatX), Random.Range(-RandomFloatY, RandomFloatY), 0);

        GameObject InsFloatingText;



        if (TextType == 1)
        {
            InsFloatingText = Instantiate(FloatingTextEnemy, RandomFloatingTextPosition, Quaternion.identity);

            TextMeshProUGUI Text = InsFloatingText.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            Text.text = Damage.ToString();

            InsFloatingText.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-5f, 5f)));
            InsFloatingText.transform.parent = Canvas.transform;
        }
        else if (TextType == 2)
        {
            InsFloatingText = Instantiate(FloatingTextPlayer, RandomFloatingTextPosition, Quaternion.identity);

            TextMeshProUGUI Text = InsFloatingText.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            Text.text = Damage.ToString();

            InsFloatingText.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-5f, 5f)));
            InsFloatingText.transform.parent = Canvas.transform;
        }
        else if (TextType == 3) 
        {
            InsFloatingText = Instantiate(FloatingTextHeal, RandomFloatingTextPosition, Quaternion.identity);

            TextMeshProUGUI Text = InsFloatingText.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            Text.text = Damage.ToString();

            InsFloatingText.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-5f, 5f)));
            InsFloatingText.transform.parent = Canvas.transform;
        }
    }
}
