using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public float offset = 5f;
    private void HandleFence(bool isReached)
    {
        var position = transform.position;
        position.y += isReached ? offset : -offset;
        transform.position = position;
    }

    private void Start()
    {
        FindAnyObjectByType<PlayerControl>().OnPlayerFenceReached += HandleFence;
    }
}
