using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Player, Text, RB
    [HideInInspector]
    public GameObject Player;

    public GameObject TargetMark;
    public GameObject FloatingTextMark;

    public GameObject RangeAttack;

    Rigidbody2D Rigidbody;

    public EnemyHealthBar HealthBar;

    // Enemy Stats
    public float MoveSpeed;

    public float AttackDmg;

    public float AttackDisMin;

    public float MaxHealth;
    [HideInInspector]
    public float CurrentHealth;

    public int GoldCoinCarried;

    // Animation
    private SpriteRenderer SpriteRenderer;

    private Animator Animator;

    public float AnimationSpeed;

    // Slow
    float SlowTime = 0;

    // Sound
    public AudioClip HurtSound;
    public AudioClip KilledSound;

    // Coin
    public GameObject GoldCoin;

    void Start()
    {
        CurrentHealth = MaxHealth;

        Rigidbody = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");

        Physics2D.IgnoreCollision(Player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());

        SetHealthBar();

        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();

        Animator.speed = AnimationSpeed;
    }

    private void Update()
    {
        Movement();
        CheckDeath();
        SetHealthBar();

        CheckAttack();
    }

    public void CheckAttack()
    {
        float Distance = Vector2.Distance(Player.transform.position, transform.position);
        if (Distance < AttackDisMin) 
        {
            Player.GetComponent<PlayerController>().TakeDamage(AttackDmg);
        }
    }

    public void SetHealthBar()
    {
        HealthBar.SetHealth(CurrentHealth, MaxHealth);
    }

    private void CheckDeath()
    {
        if (CurrentHealth <= 0)
        {
            SoundManager.sndman.PlaySound(KilledSound, 1f);

            Destroy(gameObject);

            GameObject Coin = Instantiate(GoldCoin, transform.position, transform.rotation);

            Coin.GetComponent<GoldCoin>().CoinValue = GoldCoinCarried;
        }
    }

    public void TakeDamage(float Damage)
    {
        // Create damage text
        FloatingTextManager.ftman.CreateText(FloatingTextMark, (int)Damage, 1);

        // Play hit sound
        SoundManager.sndman.PlaySound(HurtSound, 1f);

        // Take damage
        CurrentHealth -= Damage;
    }

    private void Movement()
    {
        Vector3 direction = (Player.transform.position - transform.position).normalized;

        // Apply Stopping Power
        if (SlowTime >= 0)
        {
            SlowTime -= Time.deltaTime;
            Rigidbody.velocity = direction * MoveSpeed / 5;
        }
        else
        {
            Rigidbody.velocity = direction * MoveSpeed;
        }

        // Flip Sprite
        if (direction.x > 0)
        {
            SpriteRenderer.flipX = true;
        }
        else if (direction.x < 0)
        {
            SpriteRenderer.flipX = false;
        }
    }

    public void SetSlowTime(float duration)
    {
        if (SlowTime < duration) {
            SlowTime = duration;
        }
    }

    public void ResetHealth() 
    {
        CurrentHealth = MaxHealth;
    }

    
}
