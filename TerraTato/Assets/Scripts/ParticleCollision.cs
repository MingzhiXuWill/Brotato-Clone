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
    public float SlowTime;
    public int BulletsNumber;
    public float Range;

    [HideInInspector]
    public float BulletScatter;

    [HideInInspector]
    public bool CanFire;
    [HideInInspector]
    public float UseTimeCounter;

    // Sound
    public AudioClip FireSound;

    // Sprite
    public GameObject Sprite;

    void Start()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        UseTimeCounter = 0;
        CanFire = false;

        BulletScatter = ParticleSystem.shape.arc;
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Enemy")
        {
            Enemy Enemy = other.GetComponent<Enemy>();

            Enemy.TakeDamage(Damage);

            // Apply Slow
            if (SlowTime != 0)
            {
                Enemy.SetSlowTime(SlowTime);
            }
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

                // Get the target
                GameObject CurrentTargetMark = Player.GetComponent<PlayerController>().CurrentTarget.GetComponent<Enemy>().TargetMark;

                // Aim
                float DisX = transform.position.x - CurrentTargetMark.transform.position.x;
                float DisY = transform.position.y - CurrentTargetMark.transform.position.y;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-DisY, -DisX) * Mathf.Rad2Deg - BulletScatter / 2));

                // Change sprite rotation
                Debug.Log(transform.rotation.z);
                if (Mathf.Abs(transform.rotation.z) > 0.5)
                {
                    Sprite.transform.localScale = new Vector3(1.5f, -1.5f, 0);
                }
                else 
                {
                    Sprite.transform.localScale = new Vector3(1.5f, 1.5f, 0);
                }

                // Fire
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
        ParticleSystem.Emit(BulletsNumber);
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
