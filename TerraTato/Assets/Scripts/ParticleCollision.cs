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

    public float GunSpriteSize;

    // Sound
    public AudioClip FireSound;

    // Sprite
    public GameObject Sprite;

    // Fire Cache
    [HideInInspector]
    public int BulletNeedToFire;
    [HideInInspector]
    public bool FiredThisFrame;

    void Start()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        UseTimeCounter = 0;
        CanFire = false;

        BulletScatter = ParticleSystem.shape.arc;

        Sprite.transform.localScale = new Vector3(GunSpriteSize, GunSpriteSize, 0);
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Enemy")
        {
            
            Enemy Enemy = other.GetComponent<Enemy>();

            LifeFruit LifeFruit = other.GetComponent<LifeFruit>();

            if (Enemy != null)
            {
                Enemy.TakeDamage(Damage);

                // Apply Slow
                if (SlowTime != 0)
                {
                    Enemy.SetSlowTime(SlowTime);
                }
            }
            else 
            {
                LifeFruit.TakeDamage(Damage);
            }
        }
    }

    private void Update()
    {
        UpdateUseTime();

        FiredThisFrame = false;

        CurrentTarget = Player.GetComponent<PlayerController>().CurrentTarget;

        if (CurrentTarget != null)
        {
            float Distance = Vector3.Distance(Player.position, CurrentTarget.transform.position);
            if (Distance < Range) {

                // Get the target
                GameObject CurrentTargetMark = Player.GetComponent<PlayerController>().CurrentTarget.transform.Find("FloatingTextMark").gameObject;

                // Aim
                float DisX = transform.position.x - CurrentTargetMark.transform.position.x;
                float DisY = transform.position.y - CurrentTargetMark.transform.position.y;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-DisY, -DisX) * Mathf.Rad2Deg - BulletScatter / 2));

                // Change sprite rotation
                if (Mathf.Abs(transform.rotation.z) > 0.5)
                {
                    Sprite.transform.localScale = new Vector3(GunSpriteSize, -GunSpriteSize, 0);
                }
                else 
                {
                    Sprite.transform.localScale = new Vector3(GunSpriteSize, GunSpriteSize, 0);
                }

                // Fire
                if (CanFire)
                {
                    BulletNeedToFire += BulletsNumber;
                    CanFire = false;
                    SoundManager.sndman.PlaySound(FireSound, 1f);
                }
            }
        }

        Fire();
    }

    public void Fire()
    {
        if (BulletNeedToFire > 0) {
            ParticleSystem.Emit(1);
            BulletNeedToFire -= 1;
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
