using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Spawn Stats
    public float SpawnTimeMax;
    [HideInInspector]
    public float SpawnTimeCount;
    [HideInInspector]
    public bool CanSpawn;

    // Enemies
    public GameObject[] SpawnerEnemyList;

    public GameObject SpawnerMark;

    // Boundary
    public float xBoundary;
    public float yBoundary;
    public float BoundaryFix;

    void Start()
    {
        SpawnTimeCount = 0;
        CanSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTimeUpdate();
        SpawnEnemy();
    }

    public void SpawnEnemy() {
        if (CanSpawn)
        {
            float xPos = Random.Range(-xBoundary + BoundaryFix, xBoundary - BoundaryFix);
            float yPos = Random.Range(-yBoundary + BoundaryFix, yBoundary - BoundaryFix);

            GameObject EnemyToSpawn = SpawnerEnemyList[Random.Range(0, SpawnerEnemyList.Length)];

            GameObject thisSpawnerMark =  Instantiate(SpawnerMark, new Vector2(xPos, yPos), Quaternion.identity);

            thisSpawnerMark.GetComponent<SpawnerMark>().EnemyToSpawn = EnemyToSpawn;

            CanSpawn = false;
        }
    }

    public void SpawnTimeUpdate() {
        if (SpawnTimeCount <= SpawnTimeMax && !CanSpawn)
        {
            SpawnTimeCount += Time.deltaTime;
        }
        else if (SpawnTimeCount > SpawnTimeMax)
        {
            CanSpawn = true;
            SpawnTimeCount = 0;
        }

    }
}
