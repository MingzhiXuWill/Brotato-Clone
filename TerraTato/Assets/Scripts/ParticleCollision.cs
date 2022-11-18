﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    // Default
    private ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    public Transform Player;

    // Floating Text
    public GameObject FloatingText;
    public GameObject Canvas;

    // Gun Stats
    public int Damage;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnParticleCollision(GameObject other)
    {
        // Hit a enemy
        if (other.tag == "Enemy") {
            // Create damage text
            FloatingTextManager.ftman.CreateText(other, Damage);

            // Play hit sound
            SoundManager.sndman.PlayHurtSounds();
        }

        Enemy Enemy = other.GetComponent<Enemy>();

        Enemy.CurrentHealth -= Damage;

        // Apply push back force
        //Vector2 direction = (other.transform.position - Player.position).normalized;
        //other.GetComponent<Enemy>().SetKnockBack(direction, 0.1f);
    }
}
