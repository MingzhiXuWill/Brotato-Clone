using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Weapon, Target
    public GameObject[] WeaponSlots;

    public GameObject[] Weapons;
    [HideInInspector]
    public GameObject CurrentTarget;

    public GameObject TargetMark;

    // Animation
    private SpriteRenderer SpriteRenderer;

    private Animator Animator;

    public RuntimeAnimatorController AnimationMoving;

    public RuntimeAnimatorController AnimationIdle;

    public float AnimationSpeed;

    // Player stats
    public float MoveSpeed;

    public float MaxHealth;
    [HideInInspector]
    public float CurrentHealth;

    public float InvincibilityDuration;
    [HideInInspector]
    public float InvincibilityCount;
    [HideInInspector]
    public bool Invincibility;

    // Total Coins
    [HideInInspector]
    public int TotalCoins;

    // Sounds
    public AudioClip HurtSound;

    void Start()
    {
        // Weapons
        SpawnWeapon(WeaponSlots[0], Weapons[0], true);
        SpawnWeapon(WeaponSlots[1], Weapons[1], true);

        // Animation
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        Animator.speed = AnimationSpeed;

        // Health
        CurrentHealth = MaxHealth;

        // Invincibility
        Invincibility = false;
        InvincibilityCount = 0;
    }

    void Update()
    {
        Movement();

        InvincibilityUpdate();

        CheckDeath();

        CurrentTarget = FindClosestEnemy();
    }

    public void SpawnWeapon(GameObject WeaponSlot, GameObject Weapon, bool Replace) {
        if (WeaponSlot.transform.childCount == 0)
        {
            Instantiate(Weapon, WeaponSlot.transform.position, WeaponSlot.transform.rotation, WeaponSlot.transform);
        }
        if (WeaponSlot.transform.childCount >= 0 && Replace)
        {
            foreach (Transform child in WeaponSlot.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            Instantiate(Weapon, WeaponSlot.transform.position, WeaponSlot.transform.rotation, WeaponSlot.transform);
        }
    }

    public void Movement() {
        // Move
        float xMovement = Input.GetAxisRaw("Horizontal");
        float yMovement = Input.GetAxisRaw("Vertical");

        var Movement = new Vector3(xMovement, yMovement, 0);
        transform.Translate(MoveSpeed * Movement.normalized * Time.deltaTime);

        // Set Boundary
        if (transform.position.x >= 30) {
            transform.position -= new Vector3(transform.position.x - 30, 0, 0);
        }
        else if (transform.position.x <= -30)
        {
            transform.position -= new Vector3(transform.position.x + 30, 0, 0);
        }

        if (transform.position.y >= 18)
        {
            transform.position -= new Vector3(0, transform.position.y - 18, 0);
        }
        else if (transform.position.y <= -22)
        {
            transform.position -= new Vector3(0, transform.position.y + 22, 0);
        }

        // Flip Sprite
        if (xMovement < 0)
        {
            SpriteRenderer.flipX = true;
        }
        else if (xMovement > 0)
        {
            SpriteRenderer.flipX = false;
        }

        // Change animation between idle and moving
        if (xMovement == 0 && yMovement == 0)
        {
            Animator.runtimeAnimatorController = AnimationIdle;
        }
        else
        {
            Animator.runtimeAnimatorController = AnimationMoving;
        }
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    private void CheckDeath()
    {
        if (CurrentHealth <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void TakeDamage(float Damage) 
    {
        if (!Invincibility)
        {
            Invincibility = true;

            // Play hit sound
            SoundManager.sndman.PlaySound(HurtSound, 1f);

            // Take damage
            CurrentHealth -= Damage;

            Debug.Log("take " + Damage + " damage");
        }
    }

    public void InvincibilityUpdate() 
    {
        if (Invincibility)
        {
            InvincibilityCount += Time.deltaTime;
            if (InvincibilityCount >= InvincibilityDuration) {
                Invincibility = false;
                InvincibilityCount = 0;
            }
        }
    }
}
