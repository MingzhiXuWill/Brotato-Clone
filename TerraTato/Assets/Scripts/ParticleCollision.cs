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

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        // Play hit sound
        SoundManager.sndman.PlayHurtSounds();

        // Create Floating Damage Text
        float RandomValue = 1f;

        float PositionRandomX = Random.Range(-RandomValue, RandomValue);
        float PositionRandomY = Random.Range(-RandomValue, RandomValue);

        Vector3 PositionRandom = other.transform.position;

        PositionRandom += new Vector3(PositionRandomX, PositionRandomY, 0);

        Instantiate(FloatingText, PositionRandom, Quaternion.identity);

        // Create Floating Text
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        GameObject explosion = Instantiate(explosionPrefab, collisionEvents[0].intersection, Quaternion.identity);

        ParticleSystem p = explosion.GetComponent<ParticleSystem>();
        var pmain = p.main;

        if (other.GetComponent<Rigidbody2D>() != null)
        {
            other.GetComponent<Rigidbody2D>().AddForceAtPosition(collisionEvents[0].intersection * 10 - transform.position, collisionEvents[0].intersection + Vector3.up);
        }
    }
}
