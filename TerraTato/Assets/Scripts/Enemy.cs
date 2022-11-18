using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Player Location
    [HideInInspector]
    public Transform Player;

    public GameObject TargetMark;
    public GameObject FloatingTextMark;

    // Speed
    public float MoveSpeed;

    // Health
    public float MaxHealth;
    [HideInInspector]
    public float CurrentHealth;

    // Self Rigidbody
    Rigidbody2D Rigidbody;

    // The min distance when start charging
    float MinDistance = 2; 

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
        if (Vector3.Distance(Player.position, transform.position) >= MinDistance) {
            Vector3 direction = (Player.position - transform.position).normalized;
            Rigidbody.velocity = direction * MoveSpeed;
        }
    }
}
