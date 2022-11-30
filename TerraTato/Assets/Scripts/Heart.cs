using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [HideInInspector]
    public Transform Target;

    Rigidbody2D Rigidbody;

    public float MoveSpeed;

    public AudioClip HeartSound;

    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().TargetMark.transform;

        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 direction = (Target.position - transform.position).normalized;

        Rigidbody.velocity = direction * MoveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SoundManager.sndman.PlaySound(HeartSound, 1f);

            Destroy(gameObject);
        }
    }
}
