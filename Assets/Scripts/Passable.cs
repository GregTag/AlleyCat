using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passable : MonoBehaviour
{
    private new Collider2D collider;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    public void Pass()
    {
        collider.isTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        collider.isTrigger = false;
    }
}
