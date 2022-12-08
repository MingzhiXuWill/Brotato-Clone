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
            enemy.MoveSpeed = enemy.MoveSpeed * (Wave * WavePercentage / 2 + 1);
            enemy.AttackDmg = (int)(enemy.AttackDmg * (Wave * WavePercentage + 1));

            enemy.CurrentHealth = enemy.MaxHealth;

            Destroy(gameObject);
        }
        else 
        {
            SpawnTimeCount += Time.deltaTime;
        }
    }
}
