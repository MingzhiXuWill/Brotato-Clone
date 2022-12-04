using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeFruit : MonoBehaviour
{
    // Player, Text, RB
    [HideInInspector]
    public GameObject Player;

    public GameObject TargetMark;
    public GameObject FloatingTextMark;

    // Enemy Stats

    public float MaxHealth;
    [HideInInspector]
    public float CurrentHealth;

    // Sound
    public AudioClip KilledSound;

    // Life Fruit
    public GameObject LifeFruitLoot;

    void Start()
    {
        CurrentHealth = MaxHealth;

        Player = GameObject.FindGameObjectWithTag("Player");

        Physics2D.IgnoreCollision(Player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
    }

    private void Update()
    {
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (CurrentHealth <= 0)
        {
            SoundManager.sndman.PlaySound(KilledSound, 1f);

            Destroy(gameObject);

            Instantiate(LifeFruitLoot, transform.position, transform.rotation);
        }
    }

    public void TakeDamage(float Damage)
    {
        // Create damage text
        FloatingTextManager.ftman.CreateText(FloatingTextMark, (int)Damage, 1);

        // Take damage
        CurrentHealth -= Damage;
    }
}
