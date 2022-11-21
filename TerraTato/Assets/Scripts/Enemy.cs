using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Player, Text, RB
    [HideInInspector]
    public Transform Player;

    public GameObject TargetMark;
    public GameObject FloatingTextMark;

    Rigidbody2D Rigidbody;

    public EnemyHealthBar HealthBar;

    // Enemy Stats
    public float MoveSpeed;

    public float MaxHealth;
    [HideInInspector]
    public float CurrentHealth;

    public float MinDistance;

    // Knock back
    float StoppingPowerTime = 0;

    // Sound
    public AudioClip HurtSound;
    public AudioClip KilledSound;

    void Start()
    {
        CurrentHealth = MaxHealth;

        Rigidbody = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        Physics2D.IgnoreCollision(Player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());

        SetHealthBar();
    }

    public void SetHealthBar() {
        HealthBar.SetHealth(CurrentHealth, MaxHealth);
    }

    private void Update()
    {
        Movement();
        CheckDeath();
        SetHealthBar();
    }

    private void CheckDeath()
    {
        if (CurrentHealth <= 0)
        {
            SoundManager.sndman.PlaySound(KilledSound, 1f);

            Destroy(gameObject);
        }
    }

    public void TakeDamage(float Damage)
    {
        // Create damage text

        FloatingTextManager.ftman.CreateText(FloatingTextMark, (int)Damage);

        // Play hit sound
        SoundManager.sndman.PlaySound(HurtSound, 1f);

        CurrentHealth -= Damage;
    }

    private void Movement()
    {
        //Chase player
        if (Vector3.Distance(Player.position, transform.position) >= MinDistance)
        {
            Vector3 direction = (Player.position - transform.position).normalized;

            // Apply Stopping Power
            if (StoppingPowerTime >= 0)
            {
                StoppingPowerTime -= Time.deltaTime;
                Rigidbody.velocity = direction * MoveSpeed / 3;
            }
            else{
                Rigidbody.velocity = direction * MoveSpeed;
            }       
        }
    }

    public void SetStoppingPower(float duration)
    {
        if (StoppingPowerTime < duration) {
            StoppingPowerTime = duration;
        }
    }
}
