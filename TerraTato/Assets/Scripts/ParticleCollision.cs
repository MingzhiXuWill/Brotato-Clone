using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    // Player, Self
    [HideInInspector]
    public ParticleSystem ParticleSystem;
    [HideInInspector]
    public Transform Player;
    [HideInInspector]
    public GameObject CurrentTarget;

    // Gun Stats
    public string Name;
    public string TooltipText;
    public float Damage;
    public float UseTime;
    public float StoppingPowerTime;
    public float BulletsNumber;
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
        if (other.tag == "Enemy")
        {
            Enemy Enemy = other.GetComponent<Enemy>();

            Enemy.TakeDamage(Damage);

            // Apply push back force
            Enemy.SetStoppingPower(StoppingPowerTime);
        }
    }

    private void Update()
    {
        UpdateUseTime();

        CurrentTarget = Player.GetComponent<PlayerController>().CurrentTarget;

        if (CurrentTarget != null)
        {
            float Distance = Vector3.Distance(Player.position, CurrentTarget.transform.position);
            if (Distance < Range) {
                GameObject CurrentTargetMark = Player.GetComponent<PlayerController>().CurrentTarget.GetComponent<Enemy>().TargetMark;

                float DisX = transform.position.x - CurrentTargetMark.transform.position.x;
                float DisY = transform.position.y - CurrentTargetMark.transform.position.y;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-DisY, -DisX) * Mathf.Rad2Deg));

                if (CanFire)
                {
                    Fire();
                    CanFire = false;
                    SoundManager.sndman.PlaySound(FireSound, 1f);
                }
            }
        }
    }

    public void Fire() {
        ParticleSystem.Emit((int)BulletsNumber);
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
