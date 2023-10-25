using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public float moveSpeed = 0.2f;
    public float magnitude = 4.5f;
    private Vector3 startLocation;
    // private Rigidbody2D rigidBody;
    void Start()
    {
        // rigidBody = GetComponent<Rigidbody2D>();
        startLocation = transform.position;
    }
    void Update()
    {
        var position = startLocation;
        position.x += Mathf.Sin(Time.time * moveSpeed) * magnitude;
        transform.position = position;

    }
}
