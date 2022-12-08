using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public int WeaponNumber = 0;
    [HideInInspector]
    public int AccessoryNumber = 0; 

    #region Weapon, Target
    public GameObject[] WeaponSlots;

    public GameObject[] Weapons;

    public GameObject[] Accessories;

    [HideInInspector]
    public GameObject CurrentTarget;

    public GameObject TargetMark;
    public GameObject FloatingTextMark;
    #endregion Weapon, Target

    #region Animation
    private SpriteRenderer SpriteRenderer;

    private Animator Animator;

    public RuntimeAnimatorController AnimationMoving;

    public RuntimeAnimatorController AnimationIdle;

    public float AnimationSpeed;

    public float xBoundary;
    public float yBoundary;
    #endregion Animation

    #region Player

    public float BaseMoveSpeed;
    [HideInInspector]
    public float MoveSpeed;

    public int BaseHealth;
    [HideInInspector]
    public int MaxHealth;
    [HideInInspector]
    public int CurrentHealth;

    [HideInInspector]
    public float DamageMulti;

    public float InvincibilityDuration;
    [HideInInspector]
    public float InvincibilityCount;
    [HideInInspector]
    public bool Invincibility;

    public float HealthAmount;
    #endregion Player

    // Total Coins
    [HideInInspector]
    public int TotalCoins;

    // Sounds
    public AudioClip[] HurtSound;

    public AudioClip FootStepSound;

    void Start()
    {
        SortAccessories(); // delete this after

        StatsUpdate();

        // Weapons spawn
        if (Weapons[0] != null)
        {
            SpawnWeapon(WeaponSlots[0], Weapons[0], true);
        }
        if (Weapons[1] != null)
        {
            SpawnWeapon(WeaponSlots[1], Weapons[1], true);
        }
        if (Weapons[2] != null)
        {
            SpawnWeapon(WeaponSlots[2], Weapons[2], true);
        }
        if (Weapons[3] != null)
        {
            SpawnWeapon(WeaponSlots[3], Weapons[3], true);
        }

        // Animation
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();


        // Health
        CurrentHealth = MaxHealth;

        // Invincibility
        Invincibility = false;
        InvincibilityCount = 0;

        TotalCoins = 100;
    }

    void Update()
    {
        Animator.speed = AnimationSpeed;

        Movement();

        InvincibilityUpdate();

        CheckDeath();

        CurrentTarget = FindClosestEnemy();
    }

    public void StatsUpdate() {
        int tempHealth = 0;
        float tempMS = 0;
        float tempDamage = 0;
        for (int i1 = 0; i1 < Accessories.Length; i1++)
        {
            if (Accessories[i1] != null) 
            {
                Accessory Accessory = Accessories[i1].GetComponent<Accessory>();

                tempHealth += Accessory.Health;
                tempMS += Accessory.MoveSpeed;
                tempDamage += Accessory.Damage;
            }
            else 
            {
                MaxHealth = BaseHealth + tempHealth;
                DamageMulti = 100 + tempDamage;
                MoveSpeed = BaseMoveSpeed + tempMS;
                break;
            }
        }
    }

    public void SortWeapons() {
        List<GameObject> gameObjectList = new List<GameObject>(Weapons);
        gameObjectList.RemoveAll(x => x == null);  
        GameObject[] TempWeapons = gameObjectList.ToArray();

        WeaponNumber = 0;

        for (int i1 = 0; i1 < Weapons.Length; i1++)
        {
            if (i1 < TempWeapons.Length)
            {
                WeaponNumber ++;
                Weapons[i1] = TempWeapons[i1];
            }
            else {
                Weapons[i1] = null;
            }
        }
    }

    public void SortAccessories()
    {
        List<GameObject> gameObjectList = new List<GameObject>(Accessories);
        gameObjectList.RemoveAll(x => x == null);
        GameObject[] TempAccessories = gameObjectList.ToArray();

        AccessoryNumber = 0;

        for (int i1 = 0; i1 < Accessories.Length; i1++)
        {
            if (i1 < TempAccessories.Length)
            {
                AccessoryNumber ++;
                Accessories[i1] = TempAccessories[i1];
            }
            else
            {
                Accessories[i1] = null;
            }
        }
    }

    public void PlayFootStep()
        {
        SoundManager.sndman.PlaySound(FootStepSound, 1f);
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

        if (transform.position.x >= xBoundary) {
            transform.position -= new Vector3(transform.position.x - xBoundary, 0, 0);
        }
        else if (transform.position.x <= -xBoundary)
        {
            transform.position -= new Vector3(transform.position.x + xBoundary, 0, 0);
        }

        if (transform.position.y >= (yBoundary - 2))
        {
            transform.position -= new Vector3(0, transform.position.y - (yBoundary - 2), 0);
        }
        else if (transform.position.y <= -(yBoundary + 2))
        {
            transform.position -= new Vector3(0, transform.position.y + (yBoundary + 2), 0);
        }

        // Flip Sprite
        if (Time.timeScale == 1) {
            if (xMovement < 0)
            {
                SpriteRenderer.flipX = true;
            }
            else if (xMovement > 0)
            {
                SpriteRenderer.flipX = false;
            }
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
        GameObject[] Enemies;
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in Enemies)
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

    public void TakeDamage(int Damage) 
    {
        if (!Invincibility)
        {
            Invincibility = true;

            // Create damage text
            FloatingTextManager.ftman.CreateText(FloatingTextMark, (int)Damage, 2);

            // Play hit sound
            SoundManager.sndman.PlaySound(HurtSound[(int)Random.Range(0, 2.99f)], 1f);

            // Take damage
            CurrentHealth -= Damage;

            CheckHealth();
        }
    }

    public void HealthDamage() 
    {
        int HealNumber = (int)(MaxHealth / 100 * HealthAmount);

        // Health damage
        CurrentHealth += HealNumber;

        // Create damage text
        FloatingTextManager.ftman.CreateText(FloatingTextMark, HealNumber, 3);       

        CheckHealth();
    }

    public void LootCoin(int Coin) 
    {
        TotalCoins += Coin;
    }

    public void CheckHealth() 
    {
        if (CurrentHealth > MaxHealth) {
            CurrentHealth = MaxHealth;
        }
        else if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
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

    public void ResetPos() {
        transform.position = new Vector3(0, 0, 0);
    }
}
