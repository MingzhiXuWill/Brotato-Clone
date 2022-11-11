using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player movement speed
    public float MovingSpeed;

    // A testing gun
    public ParticleSystem Gun;

    public float FireTimeMax;

    public float FireTimeCount;

    bool CanFire;

    // Target
    GameObject CurrentTarget;

    private void Start()
    {
        FireTimeCount = 0;
        CanFire = false;
    }

    void Update()
    {
        CurrentTarget = FindClosestEnemy();

        if (FireTimeCount <= FireTimeMax && !CanFire) {
            FireTimeCount += Time.deltaTime;
        }
        else if(FireTimeCount > FireTimeMax){
            CanFire = true;
            FireTimeCount = 0;
        }

        // Move
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(x * MovingSpeed * Time.deltaTime, y * MovingSpeed * Time.deltaTime, 0);

        // Gun
        //float DisX = Gun.transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        //float DisY = Gun.transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

        if (CurrentTarget != null) {
            float DisX = Gun.transform.position.x - CurrentTarget.transform.position.x;
            float DisY = Gun.transform.position.y - CurrentTarget.transform.position.y;
            Gun.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-DisY, -DisX) * Mathf.Rad2Deg));

            if (CanFire)
            {
                Gun.Emit(1);
                CanFire = false;
                SoundManager.sndman.PlayFireSounds();
            }
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
