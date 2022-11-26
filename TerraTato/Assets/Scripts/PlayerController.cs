using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Weapon, Target
    public GameObject[] WeaponSlots;

    public GameObject[] Weapons;

    [HideInInspector]
    public GameObject CurrentTarget;

    public GameObject TargetMark;

    // Player movement speed
    public float MoveSpeed;

    // Total Coins
    public int TotalCoins;

    private void Start()
    {
        SpawnWeapon(WeaponSlots[0], Weapons[0], true);
    }

    void Update()
    {
        // Move
        float xMovement = Input.GetAxisRaw("Horizontal");
        float yMovement = Input.GetAxisRaw("Vertical");

        var Movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        transform.Translate(MoveSpeed * Movement.normalized * Time.deltaTime);

        // FindEnemy
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
