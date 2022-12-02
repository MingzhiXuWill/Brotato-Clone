using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextManager : MonoBehaviour
{
    // Floating Text
    public GameObject FloatingTextEnemy;
    public GameObject FloatingTextPlayer;
    public GameObject CanvasEnemy;
    public GameObject CanvasPlayer;

    public static FloatingTextManager ftman;
    void Start()
    {
        ftman = this;
    }

    public void CreateText(GameObject FloatingTextMark, int Damage, bool Player)
    {
        float RandomFloatX = 0.5f;
        float RandomFloatY = 1f;

        Vector3 RandomFloatingTextPosition = FloatingTextMark.transform.position + new Vector3(Random.Range(-RandomFloatX, RandomFloatX), Random.Range(-RandomFloatY, RandomFloatY), 0);

        GameObject InsFloatingText;



        if (Player) 
        {
            InsFloatingText = Instantiate(FloatingTextPlayer, RandomFloatingTextPosition, Quaternion.identity);

            TextMeshProUGUI Text = InsFloatingText.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            Text.text = Damage.ToString();

            InsFloatingText.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-5f, 5f)));
            InsFloatingText.transform.parent = CanvasEnemy.transform;
        }
        else
        {
            InsFloatingText = Instantiate(FloatingTextEnemy, RandomFloatingTextPosition, Quaternion.identity);

            TextMeshProUGUI Text = InsFloatingText.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            Text.text = Damage.ToString();

            InsFloatingText.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-5f, 5f)));
            InsFloatingText.transform.parent = CanvasPlayer.transform;
        }
    }
}
