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

    // Enemy Stats
    public float MoveSpeed;

    public float MaxHealth;
    [HideInInspector]
    public float CurrentHealth;

    public float MinDistance;

    // Knock back
    float StoppingPowerTime = 0;

    void Start()
    {
        CurrentHealth = MaxHealth;

        Rigidbody = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        Physics2D.IgnoreCollision(Player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
    }

    private void Update()
    {
        Movement();
        CheckDeath();
    }

    private void CheckDeath() {
        if (CurrentHealth <= 0) {
            SoundManager.sndman.PlayKilledSounds();
            Destroy(gameObject);
        }
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
