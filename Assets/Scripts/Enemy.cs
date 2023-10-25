using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyState
{
    Respawning,
    Walking,
}

public class Enemy : MonoBehaviour
{
    public float moveSpeed = -5f;
    public float offset = 9f;
    public int frameDelay = 300;
    public float respawnProbability = 0.3f;
    private Rigidbody2D rigidBody;
    private Vector3 startLocation;
    private EnemyState state = EnemyState.Respawning;

    private void Flip()
    {
        moveSpeed *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Respawn()
    {
        int spawnIndex = Random.Range(0, 1);
        var position = transform.position;
        position.x = offset * (spawnIndex == 0 ? -1 : 1);
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        FindAnyObjectByType<PlayerControl>().OnPlayerFenceReached += isReached => enabled = !isReached;
        startLocation = transform.position;
    }

    private void Update()
    {
        if (Time.frameCount % frameDelay != 0 || state != EnemyState.Respawning)
        {
            return;
        }

        if (Random.Range(0f, 1f) < respawnProbability)
        {
            state = EnemyState.Walking;
            Respawn();
            if (transform.position.x * moveSpeed > 0)
            {
                Flip();
            }
            rigidBody.velocity = new Vector2(moveSpeed, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerControl>().Die();
        }
    }

    private void OnBecameInvisible()
    {
        state = EnemyState.Respawning;
        rigidBody.velocity = Vector2.zero;
        transform.position = startLocation;
    }
}
