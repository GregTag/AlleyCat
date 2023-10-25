using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public float moveSpeed = 0.2f;
    public float magnitude = 4.5f;
    private Vector3 startLocation;

    private void Start()
    {
        startLocation = transform.position;
    }

    private void Update()
    {
        var position = startLocation;
        position.x += Mathf.Sin(Time.time * moveSpeed) * magnitude;
        transform.position = position;
    }
}
