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

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        // Create damage text
        float RandomFloat = 1;
        Vector3 RandomFloatingTextPosition = other.transform.position + new Vector3(Random.Range(-RandomFloat, RandomFloat), Random.Range(-RandomFloat, RandomFloat), 0);

        GameObject InsFloatingText = Instantiate(FloatingText, RandomFloatingTextPosition, Quaternion.identity);
        InsFloatingText.transform.parent = Canvas.transform;

        // Play hit sound
        SoundManager.sndman.PlayHurtSounds();

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
