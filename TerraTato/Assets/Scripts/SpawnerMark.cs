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

    void Update()
    {
        if (SpawnTimeCount > SpawnTime) 
        {
            Instantiate(EnemyToSpawn, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
        else 
        {
            SpawnTimeCount += Time.deltaTime;
        }
    }
}
