using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableSpike : MonoBehaviour
{
    public float impulse = 10f;
    public bool isStraight = false;
    public float offset = -90f;
    private Rigidbody2D rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        var player = GameObject.FindGameObjectWithTag("Player");
        var direction = player.transform.position - transform.position;
        if (!isStraight)
        {
            direction += Vector3.up * direction.magnitude;
        }
        rigidBody.velocity = direction * impulse;
    }
    private void Update()
    {
        var direction = rigidBody.velocity;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + offset;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerControl>().Die();
        }
    }
}
