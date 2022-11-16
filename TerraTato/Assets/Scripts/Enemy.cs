using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform Player;
    public float MoveSpeed;

    public float MaxHealth;
    [HideInInspector]
    public float CurrentHealth;

    Rigidbody2D Rigidbody;

    void Start()
    {
        CurrentHealth = MaxHealth;

        Rigidbody = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
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
        Vector3 direction = (Player.position - transform.position).normalized;
        Rigidbody.velocity = direction * MoveSpeed;
    }
}
