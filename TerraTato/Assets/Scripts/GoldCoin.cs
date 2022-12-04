using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoin : MonoBehaviour
{
    [HideInInspector]
    public Transform Target;

    Rigidbody2D Rigidbody;

    [HideInInspector]
    public int CoinValue;

    public float MoveSpeed;

    public AudioClip[] CoinSound;

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
            SoundManager.sndman.PlaySound(CoinSound[(int)Random.Range(0, 4.99f)], 1f);

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().LootCoin(CoinValue);

            Destroy(gameObject);
        }
    }
}
