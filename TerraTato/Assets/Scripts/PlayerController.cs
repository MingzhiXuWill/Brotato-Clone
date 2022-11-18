using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Weapon, Target
    public GameObject WeaponSlot1;
    public GameObject WeaponSlot2;
    public GameObject WeaponSlot3;
    public GameObject WeaponSlot4;

    [HideInInspector]
    public GameObject CurrentTarget;

    // Player movement speed
    public float MoveSpeed;

    private void Start()
    {

    }

    void Update()
    {
        // Move
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(x * MoveSpeed * Time.deltaTime, y * MoveSpeed * Time.deltaTime, 0);

        // FindEnemy
        CurrentTarget = FindClosestEnemy();
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
}
