using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3;

    public ParticleSystem Gun;

    void Start()
    {

    }

    void Update()
    {
        // Move
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(x * speed * Time.deltaTime, y * speed * Time.deltaTime, 0);

        // Gun
        float DisX = Gun.transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        float DisY = Gun.transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

        Gun.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-DisY, -DisX) * Mathf.Rad2Deg));

        if (Input.GetMouseButtonDown(0))
        {
            Gun.Play();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Gun.Stop();
        }
    }
}
