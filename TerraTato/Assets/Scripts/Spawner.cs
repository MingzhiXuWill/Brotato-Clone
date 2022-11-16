using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float SpawnTimeMax;

    public float SpawnTimeCount;

    bool CanSpawn;

    public GameObject Zombie;

    void Start()
    {
        SpawnTimeCount = 0;
        CanSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnTimeCount <= SpawnTimeMax && !CanSpawn)
        {
            SpawnTimeCount += Time.deltaTime;
        }
        else if (SpawnTimeCount > SpawnTimeMax)
        {
            CanSpawn = true;
            SpawnTimeCount = 0;
        }

        if (CanSpawn)
        {
            Instantiate(Zombie, transform.position, Quaternion.identity);
            CanSpawn = false;
        }
    }
}
