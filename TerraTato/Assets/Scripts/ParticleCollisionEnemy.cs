using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionEnemy : MonoBehaviour
{
    // Player, Self
    [HideInInspector]
    public ParticleSystem ParticleSystem;
    [HideInInspector]
    public Transform Player;
    [HideInInspector]
    public GameObject CurrentTarget;

    // Gun Stats
    [HideInInspector]
    public int Damage;
    public float UseTime;
    public float Range;

    [HideInInspector]
    public bool CanFire;
    [HideInInspector]
    public float UseTimeCounter;

    // Sound
    public AudioClip FireSound;

    void Start()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        UseTimeCounter = 0;
        CanFire = false;
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player")
        {
            PlayerController Player = other.GetComponent<PlayerController>();

            Player.TakeDamage(Damage);
        }
    }

    private void Update()
    {
        UpdateUseTime();

        CurrentTarget = Player.gameObject;

        if (CurrentTarget != null)
        {
            float Distance = Vector3.Distance(Player.position, CurrentTarget.transform.position);
            if (Distance < Range) {

                // Get the target
                GameObject CurrentTargetMark = Player.gameObject;

                // Aim
                float DisX = transform.position.x - CurrentTargetMark.transform.position.x;
                float DisY = transform.position.y - CurrentTargetMark.transform.position.y;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-DisY, -DisX) * Mathf.Rad2Deg));

                // Fire
                if (CanFire)
                {
                    Debug.Log("Shot");
                    ParticleSystem.Emit(1);
                    CanFire = false;
                    SoundManager.sndman.PlaySound(FireSound, 1f);
                }
            }
        }
    }

    public void UpdateUseTime()
    {
        if (UseTimeCounter <= UseTime && !CanFire)
        {
            UseTimeCounter += Time.deltaTime;
        }
        else if (UseTimeCounter > UseTime)
        {
            CanFire = true;
            UseTimeCounter = 0;
        }
    }
}
