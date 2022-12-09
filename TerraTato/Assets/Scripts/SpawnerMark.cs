using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMark : MonoBehaviour
{
    [HideInInspector]
    public GameObject EnemyToSpawn;

    public float SpawnTime;
    [HideInInspector]
    public float SpawnTimeCount;

    [HideInInspector]
    public float Wave;
    [HideInInspector]
    public float WavePercentage;

    void Update()
    {
        if (SpawnTimeCount > SpawnTime) 
        {
            Enemy enemy = Instantiate(EnemyToSpawn, transform.position, Quaternion.identity).GetComponent<Enemy>();

            enemy.MaxHealth = (int)(enemy.MaxHealth * (Wave * WavePercentage + 1));

            float tempMS = enemy.MoveSpeed * (Wave * WavePercentage / 4 + 1);
            if (tempMS <= tempMS + 1)
            {
                enemy.MoveSpeed = tempMS;
            }

            int tempDamage = (int)(enemy.AttackDmg * (Wave * WavePercentage / 3 + 1));
            if (tempDamage <= enemy.AttackDmg * 3) {
                enemy.AttackDmg = tempDamage;
            }

            if (enemy.RangeAttack != null) {
                enemy.RangeAttack.GetComponent<ParticleCollisionEnemy>().Damage = enemy.AttackDmg;
            }

            enemy.CurrentHealth = enemy.MaxHealth;

            Destroy(gameObject);
        }
        else 
        {
            SpawnTimeCount += Time.deltaTime;
        }
    }
}
