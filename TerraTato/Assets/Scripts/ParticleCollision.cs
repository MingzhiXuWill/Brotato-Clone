using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    // Default
    private ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public GameObject explosionPrefab;

    // Floating Text
    public GameObject FloatingText;
    public GameObject Canvas;

    // Gun Stats
    public int Damage;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        
        if (other.tag == "Enemy") {
            // Create damage text
            FloatingTextManager.ftman.CreateText(other, Damage);

            // Play hit sound
            SoundManager.sndman.PlayHurtSounds();
        }

        Enemy Enemy = other.GetComponent<Enemy>();

        Enemy.CurrentHealth -= Damage;


        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        //GameObject explosion = Instantiate(explosionPrefab, collisionEvents[0].intersection, Quaternion.identity);

        //ParticleSystem p = explosion.GetComponent<ParticleSystem>();
        //var pmain = p.main;

        if (other.GetComponent<Rigidbody2D>() != null)
        {
            other.GetComponent<Rigidbody2D>().AddForceAtPosition(collisionEvents[0].intersection * 10 - transform.position, collisionEvents[0].intersection + Vector3.up);
        }
    }
}
