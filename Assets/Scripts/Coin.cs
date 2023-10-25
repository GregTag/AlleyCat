using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject clouds;
    public Object spikePrefab;
    public float frameDelay = 30;
    public float throwProbability = 0.05f;
    public float relocationProbability = 0.02f;

    public int cost = 0;
    private bool isFenceReached = false;
    private bool isVisible = false;

    void RandomLocation()
    {
        int index = Random.Range(0, clouds.transform.childCount);
        var cloud = clouds.transform.GetChild(index);
        var position = cloud.position;
        position.z -= 1;
        transform.position = position;
        transform.SetParent(cloud);
    }

    private void UpdateEnable()
    {
        enabled = isVisible && isFenceReached;
    }
    private void HandleFence(bool isReached)
    {
        isFenceReached = isReached;
        UpdateEnable();
    }

    private void Start()
    {
        RandomLocation();
        FindAnyObjectByType<PlayerControl>().OnPlayerFenceReached += HandleFence;
        enabled = false;
    }

    void Throw()
    {
        Instantiate(spikePrefab, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        if (Time.frameCount % frameDelay != 0)
        {
            return;
        }
        if (Random.Range(0f, 1f) < relocationProbability)
        {
            RandomLocation();
        }
        if (Random.Range(0f, 1f) < throwProbability)
        {
            Throw();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindAnyObjectByType<ScoreRenderer>().IncreaseScore(cost);
            FindAnyObjectByType<PlayerControl>().OnPlayerFenceReached -= HandleFence;
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
        UpdateEnable();
    }

    private void OnBecameVisible()
    {
        isVisible = true;
        UpdateEnable();
    }
}
